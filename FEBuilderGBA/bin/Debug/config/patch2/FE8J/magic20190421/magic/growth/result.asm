@thumb

	mov	r0, r5
	add	r0, #121
	ldrb	r0, [r0, #0]
	ldrb	r1, [r4, #25]
	add	r0, r0, r1
	strb	r0, [r4, #25]
	
	mov	r0, r5
	add	r0, #122
	ldrb	r0, [r0, #0]
	ldrb	r1, [r4, #26]
	add	r0, r0, r1
	strb	r0, [r4, #26]
	
	ldr	r0, =$0802c1bc
	mov	pc, r0