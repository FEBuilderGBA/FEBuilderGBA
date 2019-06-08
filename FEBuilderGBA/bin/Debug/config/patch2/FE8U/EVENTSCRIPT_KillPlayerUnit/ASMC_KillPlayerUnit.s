@
@プレイヤーユニットを殺します
@
@復活させる場合は、UNCR(死亡|非表示) で、復活させてください。
@この命令は、BWLに、どのマップで死んだのかを記録します。
@

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

push {lr,r4}

	ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

	ldrb r0, [r4, #0x0]       @引数0 [FLAG]

	mov  r1,#0xf
	and  r0,r1

	cmp  r0,#0xF
	beq  GetUnitInfoByCurrent

	ldrb r0, [r4, #0x2]       @引数1 [UNIT] dest unit
	b   GetUnitInfo

GetUnitInfoByCurrent:
@	ldr  r0,=#0x03004DF0      @操作中のユニット {J}
	ldr  r0,=#0x03004E50      @操作中のユニット {U}
	ldr  r0,[r0]
	b   ProcessMain

GetUnitInfo:
	                          @RAM UNIT POINTERの取得
@	blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
	blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了

ProcessMain:

	mov  r3,r0
	ldr  r0,[r3,#0xC]         @ステータスを死亡非表示にする
	mov  r1,#0x5              @5 = 死亡 | 非表示
	orr  r0,r1
	str  r0,[r3,#0xC]

	ldr  r1,[r3,#0x0]         @Unit
	ldrb r0,[r1,#0x4]         @Unit->UnitID

	mov r1, #0x0
	mov r2, #0x3
@	blh 0x080a90c8            @BWL_AddWinOrLossIdk	{J}
	blh 0x080a4684            @BWL_AddWinOrLossIdk	{U}

Term:

pop {r4}
pop {r1}
mov pc,r1
