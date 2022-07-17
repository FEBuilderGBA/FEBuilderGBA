.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

mov r0, #0xEE
blh 0x080860A8	@SetFlag	@{J}
@blh 0x08083D80	@SetFlag	@{U}

mov r6, #0x0
mov r0, #0x19
strb r0, [r5, #0x11] @ActionData@gActionData.unitActionType
ldr r4, =0x03004DF0	 @操作キャラのワークメモリへのポインタ	{J}

ldr r3, =0x080BA654|1	@{J}
bx r3
