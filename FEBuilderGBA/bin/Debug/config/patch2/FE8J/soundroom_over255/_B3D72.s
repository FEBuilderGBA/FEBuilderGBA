.thumb

ldr r3, =0x0203A90C	@{J}
@ldr r3, =0x0203A910	@{U}
mov r1, #0x1f
and r1 ,r2
ldr r0, [r3, r0]
lsr r0 ,r1

ldr r1, =0x080B3D7C|1	@{J}
@ldr r1, =0x080AF15C|1	@{U}
bx r1
