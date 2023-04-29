@Call 0x0808ECA0 FE8J
@Call 0x0808C9A4 FE8U
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
.org 0x00

@r7 tile id
@r9 BattleMapState@gGameState
push {r4}

ldr  r4,Table
sub  r4,#0x8        @     ループ処理が面倒なので、最初に0x8バイト差し引きます.

Loop:
add  r4,#0x8        @     次のデータへ

CheckMAP:
ldr  r0,[r4,#0x00]  @     Check Term
ldr  r1,=0xFFFFFFFF
cmp  r0,r1
beq  load_default   @     見つからないのでディフォルト動作

ldrb r0,[r4,#0x00]  @     B0:MAPID=MAPID(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckXY

@ldr  r2,=0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)
ldr  r2,=0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckXY:
mov r2, r9
ldrb r1, [r2, #0x14] @    BattleMapState@gGameState.cursorMapPos.X
ldrb r0,[r4,#0x01]   @    B1:MAPX=X
cmp  r1, #0xff
beq  CheckTileID
cmp  r1, r0
bne  Loop

ldrb r1, [r2, #0x16] @     BattleMapState@gGameState.cursorMapPos.Y
ldrb r0,[r4,#0x02]   @     B2:MAPY=Y
cmp  r1, r0
bne  Loop

CheckTileID:
ldrb r1,[r4,#0x03]  @     B3:TILE=TILEID(0xFF=ANY)
cmp  r1,#0xff
beq  CheckFlag
mov  r0, r7         @     r7=tileid
cmp  r1, r0
bne  Loop

CheckFlag:
ldrh r0,[r4,#0x04]  @     W4:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

@blh  0x080860D0     @FE8J CheckFlag  Flag=r0  Result=r0:bool
blh  0x08083DA8     @FE8U CheckFlag  Flag=r0  Result=r0:bool
cmp  r0,#0x00
beq  Loop           @     条件不一致なので、次のループへ continue;

@@b    Found          @発見!

Found:
ldrh r0,[r4,#0x06]  @     W6:TEXT=Text
@blh  0x08009fa8     @FE8J GetStringFromIndex TextID=r0 Result=r0:buffer
blh  0x0800a240     @FE8U GetStringFromIndex TextID=r0 Result=r0:buffer
b    Exit

load_default:
mov r0 ,r7
@blh 0x08019f18      @FE8J GetTerrainNameString
blh 0x0801a240      @FE8U GetTerrainNameString
b   Exit

Exit:
mov r5, r0          @DrawText Buffer
pop {r4}

@ldr r3, =0x0808ECA8|1 @FE8J
ldr r3, =0x0808C9AC|1 @FE8U
bx  r3

.ltorg
.align
Table:
