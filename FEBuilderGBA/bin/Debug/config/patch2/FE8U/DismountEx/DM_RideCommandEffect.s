
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.macro blh_lr to, reg=r3
  mov \reg, pc
  add \reg, #0x8+1

  push {\reg}
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

@Call $24924        @
@r1   gActionData

push {r5}           @ push r4はもともとされているので、不足するr5を確保

MOV  r0, #0x21
STRB r0, [r1, #0x11]

ldr  r5,=0x03004E50 @  操作キャラのワークメモリへのポインタ
ldr  r5,[r5]

@バリスタがあれば優先するべきだよなあ
MOV r2, r5         @壊す命令の再送
LDR r0, [r2, #0x0]
ldr r0, [r0, #0x28]
LDR r1, [r2, #0x4]
blh_lr  0x080248D0  @ バリスタチェックの続きを行う
cmp r0,#0x1
beq RideBlista


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

ldr  r2,=0x202BCF0  @ Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;


CheckFlag:
ldrh r0,[r4,#0x04]  @     W2:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

blh  0x08083DA8     @ CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq  Loop           @     条件不一致なので、次のループへ continue;

Found:              @探索したデータにマッチした。

ldrb r0,[r4,#0x02]  @ Table->To Class

blh  0x08019444     @ GetROMClassStruct
cmp  r0,#0x00
beq  NotMatch

str  r0,[r5, #0x04] @ Update Class

ldrh r0,[r4,#0x08]  @     W8:SONG=To SE(0=None)
cmp  r0,#0x00
beq  UpdateUnitIcon

blh  0x080D01FC     @ m4aSongNumStart

UpdateUnitIcon:
blh  0x080790a4     @ ClearMOVEUNITs
mov  r0, r5         @Active Unit
blh  0x08078464     @MakeMOVEUNITForMapUnit

NotMatch:
mov r0, #0x17
pop {r4, r5}
pop {r1}
mov pc,r1

RideBlista:
ldr r4,=0x03004E50 @  操作キャラのワークメモリへのポインタ
ldr r0, [r4]       @ 壊すコードの再送(必ずr4にはポインタを入れておくこと)

LDR r3, =0x0802492C @ バリスタに乗る
pop {r5}
mov pc,r3

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
