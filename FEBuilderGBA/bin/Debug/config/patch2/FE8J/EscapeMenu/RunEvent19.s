.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}
mov r2 ,r0
ldr r1, =0x0203A954 @(ActionData@gActionData._u00 )	{J}
mov r0, #0x1   @Wait
strb r0, [r1, #0x11]   @ActionData@gActionData.unitActionType

mov r0, #0x3
blh 0x080860a8   @フラグを立てる関数 r0=立てるフラグ:FLAG	{J}

mov r0, #0x17
pop {r1}
bx r1
