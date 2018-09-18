@Call 0x080192B8 FE8U
@r0 ram unit
@r1 
@r2 
@r3 
@r4 use stack. RAM UNIT
@r5 use stack. Table

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
.org 0x00

push {lr}           @     フックする余白のために pushの領域を壊すので再送する.
push {r4,r5}
mov  r4,r0          @     RAM UNIT
ldr  r5,Table
sub  r5,#0x8        @     ループ処理が面倒なので、最初に0x8バイト差し引きます.

Loop:
add  r5,#0x8        @     次のデータへ

ldrh r0,[r5,#0x00]  @     W0:Portrait
cmp  r0,#0x0        @     データのポインタがない場合、終端とみなす.
beq  load_defualt   @データがないので、ディフォルトの顔画像をロードして終了!

ldrb r0,[r5,#0x02]  @     B2:UNITID
cmp  r0,#0x00       @     データのポインタがない場合、終端とみなす.
beq  load_defualt   @データがないので、ディフォルトの顔画像をロードして終了!

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

ldr  r2,=#0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckEdition:
ldrb r0,[r5,#0x05]  @     B5:EDITION=編(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckFlag

ldr  r2,=#0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0x1B]  @     ChapterData->Edition
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckFlag:
ldrh r0,[r5,#0x06]  @     W6:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

blh  0x08083DA8     @FE8U CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq  Loop           @     条件不一致なので、次のループへ continue;

@@b    Found          @発見!

Found:
ldrh r0,[r5,#0x00]  @     W0:Portraitを採用
b    Exit

load_defualt:
ldr  r1,[r4,#0x00]  @     RAMUNIT->ROMUNIT
ldrh r0,[r1,#0x6]   @     ROMUNIT->ID

Exit:

@FE8Uの場合、リオンの顔のハードコーディングがあるため、素直に戻せない!
cmp  r0,#0x00
bne  TrueExit

ldr  r1, [r4, #0x4] @    RAMUNIT->Class
ldrh r0, [r1, #0x8] @    クラスカード参照
TrueExit:

pop {r5,r4}         @     この関数内で利用したスタックの解放(lrは解放したらダメよ)
pop {pc}            @FE8U FE8Uの場合、関数を丸ごとのっとってしまおう.


.ltorg
.align
Table:
