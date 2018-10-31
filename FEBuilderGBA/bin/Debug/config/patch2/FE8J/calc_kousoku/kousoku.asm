@thumb
@org	$0802aae4
	push	{r4, lr}
	mov	r4, r0
	mov	r1, #74
	ldrh	r0, [r1, r4]
	bl	$080173b4	;r0に武器の重さ
	
	ldrb	r1, [r1, #7]
	cmp	r1, #7
	bgt	none
	add	r1, #40
	ldrb	r1, [r1, r4]
	cmp	r1, #250
	bls	none
	mov	r0, #0
none
	ldrb	r1, [r4, #26]	;体格
	ldrb	r2, [r4, #21]	;技
	lsr	r2, r2, #2
	add	r1, r1, r2
	sub	r0, r0, r1
	bhi	make
	mov	r0, #0
make
	ldrb	r1, [r4, #22]
	sub	r0, r1, r0
	bge	plus
	mov	r0, #0
plus
	add	r4, #94
	strh	r0, [r4]
	pop	{pc, r4}