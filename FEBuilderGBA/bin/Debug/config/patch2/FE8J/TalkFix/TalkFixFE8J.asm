@thumb

	ldr	r0, [r0]
	ldrb	r1, [r0, #4]
	mov	r0, r5
	ldr r5,[sp]
	mov r2,#0x17
	strb r2,[r4,#0x11
	ldr r4, [r5,#4]
	sub r4, #0x30
	str r4, [r5,#4]
	ldr r4,=0x202bce9
	mov r5,#80
	strb r5,[r4]
	ldr r4,=0x8086288
	mov lr,r4
	@dcw 0xf800
	ldr	r0, =$08032313
	bx	r0

;the original version was made by Circleseverywhere