@thumb
@org	$08094a5e
	ldr	r0, [r4]
	ldr	r0, [r0]
	ldr	r2, [r0, #4]
	ldrb	r1, [r2, #25]	;上限
	ldrb	r2, [r0, #26]	;能力
	mov	r6, #2
	cmp	r1, r2
	bne	non
	mov	r6, #4
non
	bl	$08018ecc	;
	mov	r2, r0
	mov	r0, r5
	add	r0, #0x18
	mov	r1, r6
	b	jump
	
	@dcd	$0200D6E0
jump
	bl	$08004a9c
	b	$08094ea6