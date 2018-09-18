@thumb
	mov	r1, #26	;â¡éZ
	ldsb	r1, [r4, r1]
	ldrb	r2, [r5, #25]	;è„å¿
	mov	r0, #25
	ldsb	r0, [r5, r0]	;è„å¿
	cmp	r1, r0
	ble	jump
	strb	r2, [r4, #26]	;â¡éZ
jump