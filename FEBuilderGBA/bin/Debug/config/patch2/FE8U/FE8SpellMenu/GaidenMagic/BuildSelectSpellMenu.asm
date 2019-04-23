.thumb

.macro blh to, reg=r6
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@need to put this in BL range

.equ NewMenu_Default, 0x0804EBE4
.equ GetUnitPortraitId, 0x080192B8
.equ NewFace, 0x0800563C
.equ Func1, 0x0801E684
.equ Func2, 0x080832C4
.equ Func3, 0x08006458

push {r4-r6, lr}
sub sp, #0x4
ldr r0, SpellSelectMenuDefs
blh NewMenu_Default
mov r4, r0
ldr r5, =0x03004E50
ldr r1, [r5, #0x0]
ldr r0, [r1, #0x4]
ldrb r0, [r0, #0x4]
cmp r0, #0x51
beq Finish

DrawFace:
mov r0, r1
blh GetUnitPortraitId
mov r1, r0
mov r0, #0x2
str r0, [sp, #0x0]
mov r0, #0x0
mov r2, #0xB0
mov r3, #0xC
blh NewFace
mov r0, #0x0
mov r1, #0x5
blh Func3

Finish:
ldr r1, [r5, #0x0]
mov r0, r4
mov r2, #0xF
mov r3, #0xB
blh Func1
blh Func2
mov r0, #0x17
add sp, #0x4
pop {r4-r6}
pop {r1}
bx r1

.ltorg
.align

SpellSelectMenuDefs:
@POIN MagicMenuDefs