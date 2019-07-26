@
@輸送隊とユニットの持ち物をスキャンして、指定したものを売る
@
@売り物の指定は、メモリスロット0x1から1バイト単位で指定。null終端。byte[] = {...}
@SVAL 0x01 ITEM1 ITEM2 ITEM3 ITEM4
@SVAL 0x02 ITEM5 ITEM6 ITEM7 ITEM8
@SVAL 0x03 ITEM9 ITEM10 ITEM11 0x00
@
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
	push	{r4,r5,r6,r7, lr}

	@トータルの売値を格納する
	mov		r7,#0x0

	@輸送隊アドレスの取得
	ldr		r0, =0x08031470	@{J}
@	ldr		r0, =0x08031524	@{U}
	ldr		r2, [r0]		@輸送隊アドレス
	ldrb	r3, [r0,#0x4]	@最大個数

	@ループ上限 = 開始アドレス+(個数*2バイト)
	lsl		r3,r3,#0x1		@r3 * 2
	add		r6,r3,r2		@終端

	@輸送隊アドレス
	mov		r5,r2

	@輸送隊書き込みアドレス
	@アイテムを没収する場合、余白ができるので詰めていく
	mov		r4,r2

yusotai_loop:
	ldrb	r0,[r5,#0x0] @アイテムID
	ldrb	r1,[r5,#0x1] @個数
	add		r5,#0x2		@アイテムID 個数の2バイト配列のため

	cmp		r0,#0x00		@アイテム終端
	beq		yusotai_item_zeropadding

	yusotai_is_target_item:
		@r0 アイテムID		@値を壊してはいけない.
		@r1 アイテム個数	@値を壊してはいけない.
		ldr	r2, =0x030004B0 + 0x4	@MemorySlot 1 @{J}
@		ldr	r2, =0x030004B8 + 0x4	@MemorySlot 1 @{U}

	yusotai_is_target_item_loop:
		ldrb r3,[r2]
		cmp r3,#0x00
		beq yusotai_is_target_item_false
		cmp r3,r0
		beq yusotai_item_pickup

		add r2,r2,#0x01
		b   yusotai_is_target_item_loop

yusotai_is_target_item_false:

	@特定のアイテム以外はコピーする
	strb	r0,[r4,#0x0]
	strb	r1,[r4,#0x1]
	add		r4,#0x2

	b		yusotai_next

yusotai_item_pickup:
	@売値を求める
	lsl		r1,r1,#0x8	@売値を求める関数は 引数r0に 以下のようにセットしなければいけない。
	add		r0,r0,r1	@個数 << 8 + アイテムID 例:レイピア60個  0x3c09 3c=60 09=レイピアのアイテムID
						@戻り値はr0に売値が返ってくる.
						@この関数はr1,r2,r3を破壊する
	
	blh		0x080B9DEC ,r1	@{J}
@	blh		0x080B5268 ,r1	@{U}

	add		r7,r7,r0	@累積売値

yusotai_next:
	cmp		r5,r6
	blt		yusotai_loop		@上限アドレスを超えていれなければ続く

yusotai_item_zeropadding:
@	@輸送隊は、ゼロ終端ではない。
	@特効薬を売って余白ができていた場合、
	@輸送隊欄に余白ができないように0で埋めないとダメ。
	cmp		r6,r4
	ble		yusotai_end

	mov		r0,#0x00
	strb	r0,[r4,#0x0]
	strb	r0,[r4,#0x1]
	add		r4,r4,#0x2
	b		yusotai_item_zeropadding

yusotai_end:


@個別キャラの持ち物スタート
	ldr		r5, =0x0202BE48	@{J}
@	ldr		r5, =0x0202BE4C	@{U}

character_loop:
	ldrh	r0,[r5]

	mov		r1,#0x00
	cmp		r0,r1
	beq		character_end

	add		r5,#0x1E	@アイテム1 アイテムIDへ
	mov		r4,r5		@書込
	
	mov		r6,r5		@個数5つまでループする
	add		r6,#0xA		@5個*2バイト = 0xA

item_loop:
	ldrb	r0,[r5,#0x00]	@アイテムID
	ldrb	r1,[r5,#0x01]	@アイテム個数
	add		r5,#0x2
	
	cmp		r0,#0x00     @アイテム終端
	beq		character_item_zeropadding

	is_target_item:
		@r0 アイテムID		@値を壊してはいけない.
		@r1 アイテム個数	@値を壊してはいけない.
		ldr	r2, =0x030004B0 + 0x4	@MemorySlot 1 @{J}
@		ldr	r2, =0x030004B8 + 0x4	@MemorySlot 1 @{U}

	is_target_item_loop:
		ldrb r3,[r2]
		cmp r3,#0x00
		beq is_target_item_false
		cmp r3,r0
		beq character_item_pickup

		add r2,r2,#0x01
		b   is_target_item_loop

	is_target_item_false:

	@特定のアイテム以外はコピーする
	strb	r0,[r4,#0x0]
	strb	r1,[r4,#0x1]
	add		r4,#0x2			@アイテムID 個数の2バイト配列のため

	b		character_item_next

character_item_pickup:
	@売値を求める
	lsl		r1,r1,#0x8	@売値を求める関数は 引数r0に 以下のようにセットしなければいけない。
	add		r0,r0,r1	@個数 << 8 + アイテムID 例:レイピア60個  0x3c09 3c=60 09=レイピアのアイテムID
						@戻り値はr0に売値が返ってくる.
						@この関数はr1,r2,r3を破壊する
	
	blh		0x080B9DEC ,r1	@{J}
@	blh		0x080B5268 ,r1	@{U}

	add		r7,r7,r0	@累積売値

character_item_next:
	cmp		r5,r6
	blt		item_loop		@アイテム5を処理するまでループ

character_item_zeropadding:
	@アイテム欄も、ゼロ終端ではない。
	@特効薬を売って余白ができていた場合、
	@5つのアイテム欄に余白ができないように0で埋めないとダメ。
	cmp		r6,r4
	ble		character_next

	mov		r0,#0x00
	strb	r0,[r4,#0x0]
	strb	r0,[r4,#0x1]
	add		r4,r4,#0x2
	b		character_item_zeropadding

character_next:

	mov		r5, #0x20	@次の人へ #0x48 - (#0xA + #0x1E) = 0x20
	add		r5,r5, r6
	b		character_loop

character_end:

	@所持金を加算する
	ldr	r2, =0x0202BCF4	@所持金を取得	@{J}
@	ldr	r2, =0x0202BCF8	@所持金を取得	@{U}
	ldr	r0, [r2]
	add	r0,r0,r7			@所持金を加算
	ldr	 r1,=0x000F423F	@MAX999999ゴールド
	cmp r0,r1			@所持金が 999999ゴールドを超えるか？
	blt gold_append
	mov r0,r1			@超えるなら補正
gold_append:
	str	r0, [r2]		@所持金の書き込み

@#IFDEF FIND
@	@売値のゴールドを @0080@0005で取れるように書き込む
@	@注意:@0080@0005は会話でしか取れない。システムメッセージでは読めないらしい
@	mov		r0,r7
@
@	blh		0x08008914 ,r1	@{J}
@@	blh		0x08008A18 ,r1	@{U}
@
@	@イベント命令の条件式でぜひが取れるようにする
@	mov	r0, #0x0
@	cmp r7,#0x0		@売るものがあるかどうか判定
@	beq not_sell
@	mov	r0, #0x1	@売るものがある
@not_sell:
@	ldr	r2, =0x030004B0 + 0x30  @MemorySlot C @{J}
@	ldr	r2, =0x030004B8 + 0x30	@MemorySlot C @{U}
@	str	r0, [r2, #0x0]	@条件式 SlotC領域に書き込む
@#ENDIF FIND

exit_return:
	mov	r0, #0x0

	pop	{pc,r7, r6 , r5 ,r4 }
