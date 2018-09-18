.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@漫符を表示するときに、効果音がなるのでそれをやめさせる.

.thumb
PUSH {r4,r5,r6,lr}
MOV r6, r9
MOV r5, r8
PUSH {r5,r6}
SUB SP, #0x4
MOV r4 ,r0
ldr r3, =0x8078A82+1       @FE8U
mov pc,r3
