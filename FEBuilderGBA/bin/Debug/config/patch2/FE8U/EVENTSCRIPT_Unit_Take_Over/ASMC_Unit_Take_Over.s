
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

push {lr,r4,r5}

	ldr  r5, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

	ldrb r0, [r5, #0x0]       @引数0 [FLAG]

	mov  r1,#0xf
	and  r0,r1

	cmp  r0,#0xF
	beq  GetUnitInfoByCurrent

	ldrb r0, [r5, #0x2]       @引数1 [UNIT] dest unit
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
	mov  r4,r0                @UnitIDの保存 dest unit


	ldrb r0, [r5, #0x3]       @引数2 [UNIT] src unit
	                          @RAM UNIT POINTERの取得
@	blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
	blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了
	mov  r5,r0                @UnitIDの保存 src unit

	
	ldrh r0,[r5,#0x08]        @Copy Lv and Exp
	strh r0,[r4,#0x08]
	ldrh r0,[r5,#0x12]        @Copy MAXHP and HP
	strh r0,[r4,#0x12]
	ldr  r0,[r5,#0x14]        @Copy pow,skil,spd,def
	str  r0,[r4,#0x14]
	ldrh r0,[r5,#0x18]        @Copy res,luck
	strh r0,[r4,#0x18]
	ldrb r0,[r5,#0x1A]        @Copy con
	strb r0,[r4,#0x1A]
	ldrb r0,[r5,#0x1D]        @Copy mov
	strb r0,[r4,#0x1D]
	ldrh r0,[r5,#0x1E]        @Copy item1
	strh r0,[r4,#0x1E]
	ldr  r0,[r5,#0x20]        @Copy item2 and item3
	str  r0,[r4,#0x20]
	ldr  r0,[r5,#0x24]        @Copy item4 and item5
	str  r0,[r4,#0x24]
	ldr  r0,[r5,#0x28]        @Copy weapon lv1
	str  r0,[r4,#0x28]
	ldr  r0,[r5,#0x2C]        @Copy weapon lv2
	str  r0,[r4,#0x2C]
	ldrh r0,[r5,#0x32]        @Copy support1,2
	strh r0,[r4,#0x32]
	ldr  r0,[r5,#0x34]        @Copy support3,4,5,6
	str  r0,[r4,#0x34]
	ldrh r0,[r5,#0x38]        @Copy support7 and support flag
	strh r0,[r4,#0x38]

Term:

pop {r4 ,r5}
pop {r1}
mov pc,r1
