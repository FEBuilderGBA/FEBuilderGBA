@
@特定のユニットの持ち物をスキャンして、アイテムを没収する
@
@40 0D [UNIT] [ITEM] [ASM+1]
@41 0D 00 [ITEM] [ASM+1]  (Load SVAL1 ID)
@4B 0D 00 [ITEM] [ASM+1]  (Load SVALB coord)
@

@Author 7743
@
.align 4
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
	push	{r5,r4,lr}

	ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

	ldrb r0, [r4, #0x0]       @引数0 [FLAG]

	mov  r1,#0xf
	and  r0,r1

	cmp  r0,#0x1
	beq  GetUnitInfoBySVAL1
	cmp  r0,#0xB
	beq  GetUnitInfoByCoord
@	cmp  r0,#0xF
@	beq  GetUnitInfoByCurrent

	ldrb r0, [r4, #0x2]       @引数1 [UNIT]
	b   GetUnitInfo

@GetUnitInfoByCurrent:
@	ldr  r0,=#0x03004E50
@	ldr  r0,[r0]
@	b   GetUnitInfo

GetUnitInfoBySVAL1:
	ldr  r0,=#0xFFFFFFFF
	b   GetUnitInfo

GetUnitInfoByCoord:
	ldr  r0,=#0xFFFFFFFE
	@b   GetUnitInfo

GetUnitInfo:
	blh  0x0800bc50           @UNITIDの解決
	                          @RAM UNIT POINTERの取得
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了

	ldrb r4, [r4, #0x3]       @引数2 ITEM

	mov		r2,r0
	add		r2,#0x1E	@アイテム1 アイテムIDへ

	mov		r5,r2		@個数5つまでループする
@#IFDEF Confiscation
	mov		r3,r5       @書き込みアドレス
@#ENDIF Confiscation

	add		r5,#0xA		@5個*2バイト = 0xA アイテム終端
item_loop:
	ldrb	r0,[r2,#0x00]	@アイテムID
	ldrb	r1,[r2,#0x01]	@アイテム個数
	add		r2,#0x2
	
	cmp		r0,r4
	beq		character_item_next

	cmp		r0,#0x00     @アイテム終端
	beq		character_item_zeropadding

@#IFDEF Confiscation
@	@特定のアイテム以外はコピーする
	strb	r0,[r3,#0x0]
	strb	r1,[r3,#0x1]
	add		r3,#0x2			@アイテムID 個数の2バイト配列のため
@#ENDIF Confiscation

character_item_next:
	cmp		r2,r5
	blt		item_loop		@アイテム5を処理するまでループ

character_item_zeropadding:
@#IFDEF Confiscation
@	@アイテム欄も、ゼロ終端ではない。
@	@特効薬を売って余白ができていた場合、
@	@5つのアイテム欄に余白ができないように0で埋めないとダメ。
	cmp		r5,r3
	ble		Term

	mov		r0,#0x00
	strb	r0,[r3,#0x0]
	strb	r0,[r3,#0x1]
	add		r3,r3,#0x2
	b		character_item_zeropadding
@#ENDIF Confiscation


Term:
	pop {pc,r4,r5}
