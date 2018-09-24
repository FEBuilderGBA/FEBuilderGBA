@
@ユニットのHPを取得
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
	push	{r4,lr}

	ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

	ldrb r0, [r4, #0x0]       @引数0 [FLAG]

	mov  r1,#0xf
	and  r0,r1

	cmp  r0,#0x1
	beq  GetUnitInfoBySVAL1
	cmp  r0,#0xB
	beq  GetUnitInfoByCoord

	ldrb r0, [r4, #0x2]       @引数1 [UNIT]
	b   GetUnitInfo

GetUnitInfoBySVAL1:
	ldr  r0,=#0xFFFFFFFF
	b   GetUnitInfo

GetUnitInfoByCoord:
	ldr  r0,=#0xFFFFFFFE
	@b   GetUnitInfo

GetUnitInfo:
@	blh  0x0800bf3c           @UNITIDの解決	{J}
	blh  0x0800bc50           @UNITIDの解決	{U}
	                          @RAM UNIT POINTERの取得
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了

	ldrb r0, [r0, #0x13]      @Unit->HP

MakeResult:
	@イベント命令の条件式でぜひが取れるようにする
@	ldr	r2, =0x030004B0  @ + #0x30	{J}
	ldr	r2, =0x030004B8  @ + #0x30	{U}
	str	r0, [r2, #0x30]  @MemorySlotC
	pop	{pc,r4}
