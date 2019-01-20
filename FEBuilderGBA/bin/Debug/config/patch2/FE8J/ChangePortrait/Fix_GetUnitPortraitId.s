.thumb
@r0 ram unit

.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.org 0x0
LDR r1, [r0, #0x0]
LDRH r1, [r1, #0x6]
CMP r1, #0x0
BEQ NullExit

blh 0x08018fcc   @GetUnitPortraitId
CMP r0, #0x0
BEQ NullExit

mov r1, r0       @顔画像を r1に代入
ldr r3,=0x0802D72E+1
bx  r3

NullExit:
ldr r3,=0x0802D72C+1
bx  r3
