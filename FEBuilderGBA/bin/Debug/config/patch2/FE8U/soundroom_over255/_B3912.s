.thumb

@ldr r0, =0x0203A90C		@{J}
ldr r0, =0x0203A910	@{U}
mov r2, #0x1f
and r2 ,r1
ldr r0, [r0, r3]
lsr r0 ,r2

@ldr r1, =0x080B391C|1	@{J}
ldr r1, =0x080AECFC|1	@{U}
bx r1
