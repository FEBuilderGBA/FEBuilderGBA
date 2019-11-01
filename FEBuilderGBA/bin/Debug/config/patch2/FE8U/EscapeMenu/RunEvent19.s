.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}
mov r2 ,r0
ldr r1, =0x0203A958 @(ActionData@gActionData._u00 )	{U}
mov r0, #0x1   @Wait
strb r0, [r1, #0x11]   @ActionData@gActionData.unitActionType

mov r0, #0x3
blh 0x08083d80   @フラグを立てる関数 r0=立てるフラグ:FLAG	{U}

mov r0, #0x17
pop {r1}
bx r1
