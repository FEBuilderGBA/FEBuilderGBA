
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

push {lr,r4,r5,r6}

	ldr  r6, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

	ldrb r0, [r6, #0x0]       @引数0 [FLAG]

	mov  r1,#0xf
	and  r0,r1

	cmp  r0,#0x1
	beq  GetUnitInfoBySVAL1
	cmp  r0,#0xB
	beq  GetUnitInfoByCoord
	cmp  r0,#0xF
	beq  GetUnitInfoByCurrent

	ldrb r0, [r6, #0x2]       @引数1 [UNIT]
	b   GetUnitInfo

GetUnitInfoByCurrent:
	ldr  r0,=#0x03004DF0      @FE8J  操作中のユニット
	ldr  r0,[r0]
	b   ProcessMain

GetUnitInfoBySVAL1:
	ldr  r0,=#0xFFFFFFFF
	b   GetUnitInfo

GetUnitInfoByCoord:
	ldr  r0,=#0xFFFFFFFE
	@b   GetUnitInfo

GetUnitInfo:
	blh  0x0800bf3c           @FE8J UNITIDの解決
	                          @RAM UNIT POINTERの取得
ProcessMain:
	cmp  r0,#0x00
	beq  NotMatch             @取得できなかったら終了

mov  r5,r0          @ RAMUnit

ldr  r4, Table
sub  r4, #0xC       @ 面倒なので最初にひいておく

Loop:
add  r4, #0xC
ldr  r0, [r4]       @ 終端チェック
cmp  r0, #0x00
beq  NotMatch

CheckUnit:
ldrb r0,[r4,#0x00]  @     B0:UNIT=UNITID(0x00=ANY)
cmp  r0, #0x00      @     ANY
beq  CheckFromClass

ldr  r3,[r5,#0x00]  @     Get Unit Struct
ldrb r1,[r3,#0x04]  @     Unit->ID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;


CheckFromClass:
ldrb r0,[r4,#0x01]  @     B1:CLASS=Form Class

ldr  r3,[r5,#0x04]  @     Get Class Struct
ldrb r1,[r3,#0x04]  @     Class->ID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;


CheckMapID:
ldrb r0,[r4,#0x03]  @     B0:MAP=MAPID(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckFlag

ldr  r2,=#0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;


CheckFlag:
ldrh r0,[r4,#0x04]  @     W2:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

blh  0x080860D0     @FE8J CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq  Loop           @     条件不一致なので、次のループへ continue;

Found:              @探索したデータにマッチした。

ldrb r0,[r4,#0x02]  @ Table->To Class

blh  0x0801911C     @FE8J GetROMClassStruct
cmp  r0,#0x00
beq  NotMatch

str  r0,[r5, #0x04] @ Update Class

ldrb r0, [r6, #0x0]       @引数0 [FLAG]
mov  r1,#0xf
and  r0,r1
cmp  r0,#0x0C             @バッチモード?
beq  BatchModeSkip        @バッチモードならば、音の再生とユニットの再描画をスキップする

ldrh r0,[r4,#0x08]  @     W8:SONG=To SE(0=None)
cmp  r0,#0x00
beq  UpdateUnitIcon

blh  0x080d4ef4     @FE8J m4aSongNumStart


UpdateUnitIcon:
blh  0x0807b4b8     @FE8J ClearMOVEUNITs
mov  r0, r5         @Active Unit
blh  0x0807a888     @MakeMOVEUNITForMapUnit

BatchModeSkip:
NotMatch:
mov r0, #0x0
pop {r4 ,r5 ,r6 }
pop {r1}
mov pc,r1

.ltorg
.align
Table:
@B0:UNIT=Unit(0=Any)
@B1:CLASS=From Class
@B2:CLASS=To Class
@B3:MAP=Chaptor ID(FF=Any)
@W4:FLAG=Judgment flag(0=Any)
@W6:SONG=From SE(0=None)
@W8:SONG=To SE(0=None)
@B10=00
@B11=00
@sizeof() == 12
@
