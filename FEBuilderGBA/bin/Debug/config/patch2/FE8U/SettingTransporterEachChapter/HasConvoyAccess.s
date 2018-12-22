@Call  31644         @FE8U
@r0 MapID

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

ldr r1, Table
ldrb r1,[r1,r0]

cmp  r1,#0x01
bge  ReturnTrue


ReturnFalse:        @輸送隊は使えない    0
mov r0, #0x0
ldr r3, =0x0803165A|1
bx  r3

ReturnTrue:
mov r0, #0x1
ldr r3, =0x0803165A|1
bx  r3

.ltorg
.align
Table:
