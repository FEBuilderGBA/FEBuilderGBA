.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push { lr }

ldr r0, =0x0203a818
mov r1, #0x0
mov r2, #53 * 3
blh 0x080d6968   @memset	{J}

pop { r0 }
bx r0

.align
.ltorg
