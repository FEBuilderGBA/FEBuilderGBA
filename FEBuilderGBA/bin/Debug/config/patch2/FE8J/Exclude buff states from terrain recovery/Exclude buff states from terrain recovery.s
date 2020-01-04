@Call 259BA @FE8U
@Call 2596A @FE8J

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


mov r1, #0xf
and r0 ,r1

cmp r0, #0x0 @POW
beq Exit
cmp r0, #0x5 @POW
BLT RECOVER
cmp r0, #0x8 @Avoid
BLE Exit

RECOVER:
mov r0, #0x10
ldsb r0, [r5, r0]
mov r1, #0x11
ldsb r1, [r5, r1]
mov r2, #0xb
ldsb r2, [r5, r2]
mov r3, #0x1
neg r3 ,r3
@blh 0x0804f8bc   @AddTarget FE8U
blh 0x08050630   @AddTarget FE8J

Exit:
@ldr r3, =0x080259CE+1   @FE8U
ldr r3, =0x0802597E+1   @FE8J
bx r3
