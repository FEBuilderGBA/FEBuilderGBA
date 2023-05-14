@Call 2567A
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@r0 temp
@r1 temp
@r2 temp
@r3 temp
@r4 Support Unit Pointer (keep)
@r5 Support Index (keep)
@r6 Support Count (keep)
@r7 [r7] == Self Unit Pointer (keep)

.thumb
push {r6}    @r6をワークメモリとして利用するためどける.

ldr  r6,DenySupport_Table
sub  r6,#0x8

Loop:
add  r6,#0x8

CheckText:
ldrh r0,[r6,#0x06]     @Text
cmp  r0,#0x00
bne  Loop              @テキストが入っているなら、支援メニューを出さないわけではないので
                       @何もしてはいけない

CheckUnit:
ldrb r0,[r6]     @Unit1
cmp  r0,#0x00
beq  NotFound

ldr  r1,[r4,#0x00] @相手のUnit
ldrb r1,[r1,#0x04] @相手のUnitID
cmp  r0,r1
beq  CheckUnit2

cmp  r0,#0xFF
beq  CheckUnit2

ldr  r1,[r7]       @自分のユニットへのポインター
ldr  r1,[r1,#0x00] @
ldrb r1,[r1,#0x04] @自分のUnitID
cmp  r0,r1
bne  Loop

CheckUnit2:
ldrb r0,[r6,#0x01]     @Unit2

cmp  r0,#0xFF
beq  CheckChapter

ldr  r1,[r4,#0x00] @相手のUnit
ldrb r1,[r1,#0x04] @相手のUnitID
cmp  r0,r1
beq  CheckChapter

ldr  r1,[r7]       @自分のユニットへのポインター
ldr  r1,[r1,#0x00] @
ldrb r1,[r1,#0x04] @自分のUnitID
cmp  r0,r1
bne  Loop

CheckChapter:
ldrb r0,[r6,#0x02]  @ChapterID
ldr  r2,=#0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID

cmp  r0,#0xFF
beq  CheckSupportLevel
cmp  r0,r1
bne  Loop

CheckSupportLevel:
ldrb r0,[r6,#0x03]     @Level
cmp  r0,#0x00
beq  CheckFlag

ldr r0, [r7, #0x0]     @gUnitSubject
mov r1 ,r5             @相手のSupportIndex (keep)
blh 0x080281d0         @GetSupportLevelBySupportIndex

ldrb r1,[r6,#0x03]     @制限支援Level
sub  r1,#0x01
cmp  r1,r0             @制限支援Level vs 現在の支援Level
bgt  Loop

CheckFlag:
ldrh r0,[r6,#0x04]     @Flag
cmp  r0,#0x00
beq  Found

blh  0x080860D0   @FE8J CheckFlag
cmp  r0,#0x00
beq  Loop

Found:
pop {r6}
ldr r3, =0x0802568C    @add r5, #0x1 に戻す
mov pc,r3

NotFound:
pop {r6}
mov r0, #0x10
ldsb r0, [r4, r0]
mov r1, #0x11
ldsb r1, [r4, r1]
mov r2, #0xb
ldsb r2, [r4, r2]
ldr r3, =0x08025686    @AddTargetの一つ上に戻す
mov pc,r3

.ltorg
.align
DenySupport_Table:
@POIN DenySupport_Table
