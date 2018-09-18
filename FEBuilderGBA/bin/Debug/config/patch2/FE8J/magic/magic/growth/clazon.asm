@thumb

		ldr	r2, =$0802b90c
		mov	lr, r2
		@dcw	$F800
	ldrb	r1, [r4, #25]
	add	r1, r1, r0
	strb	r1, [r4, #25]
	
	ldr	r0, [r4, #4]
	add	r0, #81
	ldrb	r0, [r0, #0]	;クラス成長率読み
	lsl	r0, r0, #24
	asr	r0, r0, #24
		ldr	r1, =$0802b90c
		mov	lr, r1
	mov	r1, r5
		@dcw	$F800
	ldrb	r1, [r4, #26]
	add	r1, r1, r0
	strb	r1, [r4, #26]
	pop	{r4, r5}
	pop	{r0}
	bx	r0