@thumb
@org	$080A0F30

	push	{r4, r5, r6, r7, lr}
	mov	r5, r0
	bl	$080a0b44
	ldr	r7, =$08803D90
	sub	r0, #1
	mov	r1, #52
	mul	r0, r1
	ldr	r7, [r7, r0]
	ldrb	r7, [r7, #21]	;支援可能性人数をロード
	mov	r6, #5
	sub	r6, r6, r7
	bge normal
	mov	r6, #0
normal:
	mov	r4, #0
loop:
	mov	r0, r5
	mov	r1, r4
	bl	$080a0ad4

	cmp	r0, #0
	beq	jump
	add	r6, #1
jump:
	add	r4, #1
	cmp	r4, r7
	blt	loop		;潰すと常に5
	mov	r0, r6
	pop	{pc, r4, r5, r6, r7}