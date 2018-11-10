.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

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
	cmp  r0,#0xF
	beq  GetUnitInfoByCurrent

	ldrb r0, [r4, #0x2]       @引数1 [UNIT]
	b   GetUnitInfo

GetUnitInfoByCurrent:
@	ldr  r0,=#0x03004DF0      @操作中のユニット {J}
	ldr  r0,=#0x03004E50      @操作中のユニット {U}
	ldr  r0,[r0]
	b   CheckUnitInfo

GetUnitInfoBySVAL1:
	ldr  r0,=#0xFFFFFFFF
	b   GetUnitInfo

GetUnitInfoByCoord:
	ldr  r0,=#0xFFFFFFFE
	@b   GetUnitInfo

GetUnitInfo:
@	blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
	blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}
	                          @RAM UNIT POINTERの取得
CheckUnitInfo:
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了

Change:
@	blh 0x0809C804            @進撃準備画面で、全預け	r0=@UNIT	{J}
	blh 0x0809A538            @進撃準備画面で、全預け	r0=@UNIT	{U}

Term:

	mov r0 ,#0x0
	pop {r4}
	pop	{r1}
	bx	r1
