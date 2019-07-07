@健康状態の変更
@
@40 0D [UNIT] [SICK] [ASM+1]
@41 0D 00 [SICK] [ASM+1]  (Load SVAL1 ID)
@4B 0D 00 [SICK] [ASM+1]  (Load SVALB coord)
@
@Author 7743
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
.thumb
	push	{r4,lr}

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
@@	ldr  r0,=#0x03004DF0      @操作中のキャラへのポインタ	{J}
@	ldr  r0,=#0x03004E50      @操作中のキャラへのポインタ	{U}
@	ldr  r0,[r0]
@	b   GetUnitInfo

GetUnitInfoBySVAL1:
	ldr  r0,=#0xFFFFFFFF
	b   GetUnitInfo

GetUnitInfoByCoord:
	ldr  r0,=#0xFFFFFFFE
	@b   GetUnitInfo

GetUnitInfo:
@	blh  0x0800bf3c           @UNITIDの解決	{J}
	blh  0x0800bc50           @GetUnitStructFromEventParameter	{U}
	                          @RAM UNIT POINTERの取得
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了

	ldrb r1, [r4, #0x3]       @引数2
	mov  r3,r1
	mov  r2,#0x0f             @check bad status
	and  r1,r2                
	cmp  r1,#0x00
	beq  Change               @状態なしにするらしい

	ldrb r1, [r4, #0x3]       @引数2
	mov  r2,#0xf0             @check turn
	and  r2,r3                
	cmp  r2,#0x00
	bne  Change               @ターン指定されているのでそのまま採用.

	mov  r2,#0x30             @ターンが入っていないので、3ターンに自動設定
	orr  r1,r2
	@b    Change

Change:
	@r0  ram unit pointer
	@r1  status.
	mov  r2,#0x30
	strb r1,[r0,r2]

Term:
@	blh  0x08019ecc   @RefreshFogAndUnitMaps    {J}
@	blh  0x08027144   @SMS_UpdateFromGameData   {J}
@	blh  0x08019914   @UpdateGameTilesGraphics  {J}
	blh  0x0801a1f4   @RefreshFogAndUnitMaps    {U}
	blh  0x080271a0   @SMS_UpdateFromGameData   {U}
	blh  0x08019c3c   @UpdateGameTilesGraphics  {U}

	mov	r0, #0
	pop {pc,r4}
