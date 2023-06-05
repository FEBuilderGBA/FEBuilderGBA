.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r3, r4,r5}
ldr r0, [r5]
mov	r4,r0

ldr  r5,Table
sub  r5,#0x8        @     ループ処理が面倒なので、最初に0x8バイト差し引きます.
Loop:
add  r5,#0x8        @     次のデータへ

ldrh r0,[r5,#0x00]  @     W0:TextID
cmp  r0,#0x0        @     データのポインタがない場合、終端とみなす.
beq  Original   @データがないので、ディフォルトの説明文をロードして終了!

ldrb r0,[r5,#0x02]  @     B2:UNITID

ldr  r1,[r4,#0x00]  @     RAMUNIT->ROMUNIT
ldrb r1,[r1,#0x4]   @     ROMUNIT->ID
cmp  r0,r1
bne  Loop

CheckClass:
ldrb r0,[r5,#0x03]  @     B3:CLASSID
cmp  r0,#0x00       @     ANY?
beq  CheckMAP
cmp  r0,#0xFF       @     ANY?
beq  CheckMAP

ldr  r1,[r4,#0x04]  @     RAMUNIT->ROMCLASS
ldrb r1,[r1,#0x4]   @     CLASSUNIT->ID
cmp  r0,r1
bne  Loop

CheckMAP:
ldrb r0,[r5,#0x04]  @     B4:MAPID=MAPID(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckEdition

@ldr  r2,=0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)	@{J}
ldr  r2,=0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)	@{U}
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckEdition:
ldrb r0,[r5,#0x05]  @     B5:EDITION=編(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckFlag

@ldr  r2,=0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)	@{J}
ldr  r2,=0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)	@{U}
ldrb r1,[r2,#0x1B]  @     ChapterData->Edition
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckFlag:
ldrh r0,[r5,#0x06]  @     W6:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

@blh  0x080860d0     @FE8U CheckFlag  Flag=r0  Result=r0:bool	@{J}
blh  0x08083DA8     @FE8U CheckFlag  Flag=r0  Result=r0:bool	@{U}
cmp  r0,#0x00
beq  Loop           @     条件不一致なので、次のループへ continue;

@@b    Found          @発見!

Found:
ldrh r0,[r5,#0x00]  @     W0:TEXTを採用
ldr	r3,=#0x800A240
mov	lr,r3
.short	0xF800
b	Exit

Original:
mov	r1,r4
ldr	r0,[r1]
ldrh	r0,[r0]
ldr	r3,=#0x800A240
mov	lr,r3
.short	0xF800

Exit:

pop {r3, r4, r5}
ldr	r2,=#0x807BEB7
bx	r2
.align
.ltorg
Table:
@POIN ChangeUnitName_Table
