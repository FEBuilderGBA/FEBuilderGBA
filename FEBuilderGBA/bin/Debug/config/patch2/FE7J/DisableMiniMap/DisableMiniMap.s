@r0 keep minimap procs
@r1 keep parent procs
@r2 work
@r3 work

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
.org 0x00

push {r4,r5,r6}
mov  r4,r0          @     保存 MiniMap Procs
mov  r5,r1          @     保存 Parent Procs
ldr  r6,Table
sub  r6,#0x4        @     ループ処理が面倒なので、最初に0x4バイト差し引きます.

Loop:
add  r6,#0x4        @     次のデータへ

CheckMAP:
ldr  r0,[r6,#0x00]  @     Check Term
ldr  r1,=0xFFFFFFFF
cmp  r0,r1
beq  load_default   @     見つからないのでディフォルト動作

ldrb r0,[r6,#0x00]  @     B0:MAPID=MAPID(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckFlag

ldr  r2,=#0x202BBF4 @FE7J Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckFlag:
ldrh r0,[r6,#0x02]  @     W2:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

blh  0x0807a0c8     @FE7J CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq  Loop           @     条件不一致なので、次のループへ continue;

@@b    Found          @発見!

Found:
b    Exit

load_default:
mov  r0,r4           @     MiniMap Procs
mov  r1,r5           @     ParentProcs
blh  0x08004370      @New6C FE7J

Exit:
pop {r4,r5,r6}
	
pop {r0}               @     フックした関数が確保しているlrを復帰して呼び出し元へ戻る
bx r0

.ltorg
.align
Table:
