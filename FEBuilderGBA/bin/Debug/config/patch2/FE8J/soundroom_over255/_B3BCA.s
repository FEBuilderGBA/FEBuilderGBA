.thumb

push {r3}
ldr r3, =0x0203A90C	@{J}
@ldr r3, =0x0203A910	@{U}
mov r1, #0x1f
and r1 ,r2
ldr r0, [r3, r0]
lsr r0 ,r1
pop {r3}

ldr r1 , =0x080B3BD4|1	@{J}
@ldr r1 , =0x080AEFB4|1	@{U}
bx r1
