.thumb
.org 0x0

@Hook 274D8   @FE8J
@
@複数の離脱ポイントマークのタイルマーカーを表示します。
@タイルマーカーの表示ルーチンはもともとのルーチンを利用します。
@そのため、結構変な構造になっています。
@もともとのルーチンを呼び出すには r4,r5をpushしないといけないので、
@呼び出す時に、pushを同時に行います。 (blh_lr2)
@
.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.macro blh_lr2 to
  mov r3, pc
  add r3, #0x8+1

  push {r3,r2,r1}
  ldr r3, =\to
  mov lr, r3
  .short 0xf800
.endm


@r4	gChapterData
@r5

push {r6,r7}

mov r0, #0xe
ldrb r6, [r4, r0]      @ gChapterData->MapID

ldr r7, Table
sub r7, #0x8
loop:
add r7, #0x8

check_mapid:
ldrb r0,[r7, #0x00]
cmp r0,#0xff
beq break_loop
cmp r0,r6
bne loop

check_flag:
ldrh r0,[r7, #0x04]
cmp  r0,#0x00
beq  show_tile_marker

blh  0x080860d0        @ CheckFlag
cmp  r0,#0x00
beq  loop

show_tile_marker:
ldrb r4,[r7, #0x02]    @ Table->Y
ldrb r5,[r7, #0x01]    @ Table->X

blh_lr2 0x080274F0  @FE8J 表示を行う
b loop

break_loop:

@ もともとのルーチンを実行することで元に戻る.

mov r0,r6              @ r0=mapid
blh 0x08034520         @ GetMapSetting
mov r1, r0
add r1, #0x8f
ldrb r5, [r1, #0x0]    @ X

mov r1, r0
add r1, #0x90
ldrb r4, [r1, #0x0]    @ Y

pop {r6,r7}

ldr  r3, =0x080274F0  @FE8J 元々あるデータの表示を行う
mov  pc, r3


.ltorg
.align
Table:
