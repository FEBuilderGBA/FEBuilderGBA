
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

@Call $24950        @
@r0   操作キャラのワークメモリへのポインタ

push {r4, r5}
ldr  r4, Table
ldr  r5, [r0]

sub  r4, #0xC       @ 面倒なので最初にひいておく

Loop:
add  r4, #0xC
ldr  r0, [r4]       @ 終端チェック
cmp  r0, #0x00
beq  NotMatch

CheckUnit:
ldrb r0,[r4,#0x00]  @     B0:UNIT=UNITID(0x00=ANY)
cmp  r0, #0x00      @     ANY
beq  CheckToClass

ldr  r3,[r5,#0x00]  @     Get Unit Struct
ldrb r1,[r3,#0x04]  @     Unit->ID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;


CheckToClass:
ldrb r0,[r4,#0x02]  @     B2:CLASS=To Class

ldr  r3,[r5,#0x04]  @     Get Class Struct
ldrb r1,[r3,#0x04]  @     Class->ID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;


CheckMapID:
ldrb r0,[r4,#0x03]  @     B0:MAP=MAPID(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckFlag

ldr  r2,=#0x202BCF0 @ Chaptor Pointer  (@ChapterData)
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

MOV r0, #0x1        @有効
pop {r4, r5}
pop {r1}
mov pc,r1

NotMatch:
mov r0, r5           @壊す命令の再送
ldr r0, [r0, #0xc]
MOV r1, #0x80
LSL r1 ,r1 ,#0x4

LDR r3, =0x08024958  @ バリスタチェックの続きを行う
pop {r4, r5}
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
