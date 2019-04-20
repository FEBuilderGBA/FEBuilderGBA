@thumb
	ldr	r0, [r7, #0]
	add	r0, #51
	ldrb	r0, [r0, #0]
	add	r0, r10
	ldr	r1, =$0802b8e8
	mov	lr, r1
	@dcw	$F800
	mov	r1, r7
	add	r1, #122
	strb	r0, [r1, #0]
	mov	r0, #0
	ldsb	r0, [r1, r0]
	add	r6, r6, r0

	ldr	r0, [sp, #0]
	str	r0, [sp, #8]
	ldr	r0, [sp, #4]
	str	r0, [sp, #12]
	
	ldr	r0, =$0802ba94
	mov	pc, r0