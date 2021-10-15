.thumb

push {r3}
@ldr r3, =0x0203A90C	@{J}
ldr r3, =0x0203A910	@{U}
mov r0, #0x1f
and r0 ,r2
ldr r1, [r3, r1]
lsr r1 ,r0
pop {r3}
@ldr r0, =0x080B396A|1	@{J}
ldr r0, =0x080AED4A|1	@{U}
bx r0
