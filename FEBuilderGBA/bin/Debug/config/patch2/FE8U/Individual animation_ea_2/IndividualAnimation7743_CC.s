.thumb
@Branching from 0x080CD0E0
@r0 ccするclass id
@r1 ok
@r2 ok
@r3 ok.
@r5 この関数を抜ける時には、アニメポインタになります
@r9 procs 
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@ ccするclass id
ldrh r0, [r5, #0x0]
mov  r5, r0     @あとあと利用するのでr5に保存

mov  r0, r9     @procs
add  r0, #0x42
ldrh r0, [r0, #0x0]  @get Unit ID

lsl  r2,r5,#0x8
orr  r2,r0           @Class_ID << 8 | Unit->Unit_ID

@Search the table
ldr  r1,CustomAnimeTable
loop_search_table:
ldrh r0,[r1, #0x0]		 @ read custom animation for class_id<<8 | unitid
cmp  r0,#0x00        @ term data
beq  not_found
cmp  r0,r2           @ check class_id and unit_id
beq  found
add  r1,#0x8         @ next table
b    loop_search_table

found:

@壊すコードを再送 ただし、 r5 だけは検索で見つけたものを利用する
mov r0, r5
ldr r5,[r1, #0x4]    @アニメポインタ 発見した個別アニメのポインタに差し替える.
blh 0x08019444       @GetROMClassStruct
str r0,[sp, #0x8]    @壊すコードの再送 spを触るので、スタック操作は禁止!

b   store_and_exit

not_found:

@壊すコードを再送
mov r0, r5
blh 0x08019444       @GetROMClassStruct
str r0,[sp, #0x8]    @壊すコードの再送 spを触るので、スタック操作は禁止!
ldr r5, [r0, #0x34]   @Anime Pointer  見つからなかったのでディフォルト値

store_and_exit:

@r0 は rom class strcutを保持していること
@[sp,#0x8]に rom class strcutを保存していることが条件

ldr r3,=0x080CD0EB
mov pc,r3

.ltorg
CustomAnimeTable:
