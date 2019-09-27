@Call 0x08096B10 FE8U
@r0
@r1
@r2
@r3
@r4 free
@r5 procs

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


.thumb
ldr  r4,Table
sub  r4,#0x4        @     ループ処理が面倒なので、最初に0x4バイト差し引きます.

Loop:
add  r4,#0x4        @     次のデータへ
ldrb r0,[r4,#0x00]  @     MapID
cmp  r0,#0xFF       @     終端
beq  LoadDefault    @データがないので、ディフォルトの表示

CheckMapID:

ldr  r3,=#0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)
ldrb r1,[r3,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckFlag:
ldrh r0,[r4,#0x02]  @     W2:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

blh  0x08083da8     @FE8U CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq  Loop           @     条件不一致なので、次のループへ continue;

Found:              @探索したデータにマッチした。
                    @r4 Table(current)

ldrb r0,[r4,#0x01]   @     指定値
b Exit

@設定がない場合は、ディフォルト設定を利用する
LoadDefault:
mov r0, #0x1 @通常マップ
@b Exit

Exit:

@r0には、種類を代入します.

@r4 には、MapIDを代入する.
ldr  r1,=#0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)
ldrb r4,[r1,#0xE]   @     ChapterData->MAPID

@r5 はデータを格納する位値に移動させます.
mov r1, r5
add r1, #0x30

ldr r3,=0x8096b74+1 @FE8U 戻るアドレス
bx  r3

.ltorg
.align
Table:
