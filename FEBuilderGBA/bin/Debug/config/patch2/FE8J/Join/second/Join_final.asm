@thumb
	push	{r4, lr}
	
	ldr	r4, =$080350d4
	mov	lr, r4
	mov	r4, r0
	@dcw	$F800
	ldr	r0, =$08024698
	mov	pc, r0