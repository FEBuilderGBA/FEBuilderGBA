@thumb

	ldr	r2, [r4, #4]
	mov	r0, #80
	ldrb	r0, [r2, r0]	;クラス初期値
	add	r1, #50
	ldrb	r5, [r1]	;ユニット初期値
	add	r0, r0, r5
	strb	r0, [r4, #26]
	mov	r1, #0
	mov	r3, r4
	add	r3, #40
	ldr	r2, =$08017b98
	mov	pc, r2