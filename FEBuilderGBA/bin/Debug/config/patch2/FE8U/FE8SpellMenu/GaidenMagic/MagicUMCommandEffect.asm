.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ ResetIconGraphics, 0x08003584
.equ LoadIconPalettes, 0x080035BC

.thumb
.org 0

@magic effect: spawn a menu with available spells

push {r4, r5, lr}
mov r5, r0
mov r4, r1
blh ResetIconGraphics

ldr r3, =0x0203F082     @marking this as using spell menu
mov r0, #0x1
strb r0, [r3]

mov r0, #0x4
blh LoadIconPalettes
mov r0, r5
mov r1, r4
ldr r3, BuildMagicSelectMenu
mov lr, r3
.short 0xf800

EndFunc:
lsl r0, r0, #0x18
lsr r0, r0, #0x18
pop {r4, r5}
pop {r1}
bx r1

.align
.ltorg

BuildMagicSelectMenu:
@POIN BuildSelectSpellMenu
