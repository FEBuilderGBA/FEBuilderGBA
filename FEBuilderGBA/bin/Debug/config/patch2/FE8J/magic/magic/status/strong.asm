@thumb
	push	{r4, lr}
	ldr	r4, =$080168D0
	mov	lr, r4
	mov	r4, r0
	@dcw	$F800
	lsl	r0, r0, #16
	lsr	r0, r0, #16
	ldr	r2, =$080161C8
	mov	lr, r2
	@dcw	$F800
	mov	r1, r0
	mov	r0, #20
	ldsb	r0, [r4, r0]
	add	r0, r0, r1
	pop	{r4}
	pop	{r1}
	bx	r1
	