.thumb
@r0 = summoned unit id
@Unit Id to Class ID
mov r1,#0x34
mul r0,r1
ldr r1,=0x8858288	@Unit0	{J}
add r1,r0
ldrb r0,[r1,#0x5]

@‰ó‚·ƒR[ƒh‚ğÄ‘—
strb r0, [r2, #0x1] @Load class
strb r4, [r2, #0x2]
ldrb r1, [r2, #0x3]

ldr r0,=0x0807D120+1
bx r0
