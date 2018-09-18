@thumb
	push	{r4, lr}
	mov	r4, r0
		ldr	r1, =$080168d0
		mov	lr, r1
		@dcw	$F800
	cmp	r0, #0
	beq	end
		ldr	r1, =$08017314
		mov	lr, r1
		@dcw	$F800
	mov	r1, #2
	and	r0, r1
	bne	mahou
buturi
	mov	r0, r4
	pop	{r4, lr}
	ldr	r1, =$08018ec4
	mov	pc, r1
mahou
	mov	r0, r4
	pop	{r4, lr}
	ldr	r1, =$08018ecc
	mov	pc, r1
end
	pop	{r4, pc}