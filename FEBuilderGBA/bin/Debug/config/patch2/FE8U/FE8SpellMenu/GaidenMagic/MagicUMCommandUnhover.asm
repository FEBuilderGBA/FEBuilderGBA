.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ HideMoveRangeGraphics, 0x0801DACC

push {r3, lr}
ldr r3, =0x0203F082     @marking this as not using spell menu
mov r0, #0x0
strb r0, [r3]
blh HideMoveRangeGraphics
mov r0, #0x0
pop {r3}
pop {r1}
bx r1