.thumb
.macro blh to, reg=r3
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm


ldrb r0, [r5, #0x1]
ldrb r1, [r5, #0x4]
ldrb r2, [r5, #0x5]
blh 0x0802e350   @AddArrowTrap	@{U}
@blh 0x0802e288   @AddArrowTrap	@{J}

ldr r3, =0x8037900|1	@{U}
@ldr r3, =0x8037998|1	@{J}
bx  r3
