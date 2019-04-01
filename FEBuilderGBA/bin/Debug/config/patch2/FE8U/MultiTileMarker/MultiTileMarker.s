.thumb
.org 0x0

@Hook 27534   @FE8U
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
beq  check_enenmy_class

blh  0x08083DA8        @ CheckFlag
cmp  r0,#0x00
beq  loop

check_enenmy_class:
ldrb r0,[r7, #0x03]
cmp  r0,#0x00
beq  show_tile_marker
bl   CheckEnemyClass
cmp  r0,#0x00
beq  loop

show_tile_marker:
ldrb r4,[r7, #0x02]    @ Table->Y
ldrb r5,[r7, #0x01]    @ Table->X

blh_lr2 0x0802754C  @FE8U 表示を行う
b loop

break_loop:

@ もともとのルーチンを実行することで元に戻る.

mov r0,r6              @ r0=mapid
blh 0x08034618         @ GetMapSetting
mov r1, r0
add r1, #0x8f
ldrb r5, [r1, #0x0]    @ X

mov r1, r0
add r1, #0x90
ldrb r4, [r1, #0x0]    @ Y

pop {r6,r7}

ldr  r3, =0x0802754C  @FE8U 元々あるデータの表示を行う
mov  pc, r3


@ 特定の敵クラスがいるかどうか
CheckEnemyClass:
push {lr,r4}

mov  r4,r0           @SearchClass

ldr  r0,=0x0202CFBC  @FE8U 敵
ldr r1,=#0x32 * 0x48 @Enemy
add r1,r0

sub r0,#0x48        @Because it is troublesome, first subtract
CheckEnemyClass_loop:
cmp r0,r1
bgt CheckEnemyClass_break

add r0,#0x48

ldr r2, [r0]          @unitram->unit
cmp r2, #0x00
beq CheckEnemyClass_loop  @Check Empty

check_enemy_class_id:
ldr r2, [r0, #0x4]
cmp r2, #0x00
beq CheckEnemyClass_loop

ldrb r2, [r2, #0x4]
cmp r2,r4
bne CheckEnemyClass_loop

found_enenmy_id:
mov  r0,#0x01
b    CheckEnemyClass_Exit

CheckEnemyClass_break:
mov  r0,#0x00
@b    CheckEnemyClass_Exit


CheckEnemyClass_Exit:
pop {r4}
pop {r1}
bx  r1


.ltorg
.align
Table:
