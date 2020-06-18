@
@特定のユニットの持ち物をスキャンして、装備できる武器をもっているか調べます
@
@40 0D [UNIT] 00 [ASM+1]
@41 0D 00 00 [ASM+1]  (Load SVAL1 ID)
@4B 0D 00 00 [ASM+1]  (Load SVALB coord)
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
	push	{r6,r5,r4,lr}

	ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

	ldrb r0, [r4, #0x0]       @引数0 [FLAG]

	mov  r1,#0xf
	and  r0,r1

	cmp  r0,#0x1
	beq  GetUnitInfoBySVAL1
	cmp  r0,#0xB
	beq  GetUnitInfoByCoord
	cmp  r0,#0xF
	beq  GetUnitInfoByCurrent

	ldrb r0, [r4, #0x2]       @引数1 [UNIT]
	b   GetUnitInfo

GetUnitInfoByCurrent:
	@ldr  r0,=#0x03004DF0	@{J}
	ldr  r0,=#0x03004E50	@{U}
	ldr  r0,[r0]
	b   GetUnitInfo

GetUnitInfoBySVAL1:
	@ldr r1, =0x030004B0	@MemorySlot0	{J}
	ldr r1, =0x030004B8	@MemorySlot0	{U}
	ldrb r0, [r1,#0x1 * 4]	@MemorySlot1
	b   GetUnitInfo

GetUnitInfoByCoord:
	ldr  r0,=#0xFFFFFFFE
	@b   GetUnitInfo

GetUnitInfo:
	@blh  0x0800bf3c           @UNITIDの解決	{J}
	blh  0x0800bc50           @UNITIDの解決	{U}
	                          @RAM UNIT POINTERの取得
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了

	mov		r4, r0		@RAMUnitを保存
	mov		r6,r0
	add		r6,#0x1E	@アイテム1 アイテムIDへ

	mov		r5,r6		@個数5つまでループする

	add		r5,#0xA		@5個*2バイト = 0xA アイテム終端
item_loop:
	ldrb	r1,[r6,#0x00]	@アイテムID
	add		r6,#0x2

	cmp		r1,#0x00     @アイテム終端
	beq		Term

	mov		r0, r4  @RAMUnit
	@blh 0x0801631c   @{J} CanUnitUseAsWeapon
	blh 0x08016574   @{U} CanUnitUseAsWeapon

	cmp		r0,#0x1
	beq		Found

character_item_next:
	cmp		r6,r5
	blt		item_loop		@アイテム5を処理するまでループ

Term:
	mov	r0, #0x0		@装備できるアイテムを所持していない
	b		MakeResult

Found:
	mov	r0, #0x1		@装備できるアイテムを所持している

MakeResult:
	@イベント命令の条件式でぜひが取れるようにする
	@ldr r2, =0x030004B0	@MemorySlot0	{J}
	ldr r2, =0x030004B8	@MemorySlot0	{U}
	str	r0, [r2, #0x30]	@条件式で取れる領域に書き込む
						@400CXX00 0C000000	未達成ならジャンプ  / @410CXX00 0C000000	達成ならジャンプ
	pop {pc,r4,r5,r6}
