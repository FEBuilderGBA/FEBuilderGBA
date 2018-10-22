@thumb
	
	push	{r4, r5, lr}
	mov	r4, #16
	ldsb	r4, [r0, r4]	;座標
	mov	r5, #17
	ldsb	r5, [r0, r5]	;座標
	ldr	r1, =$02033F38
	str	r0, [r1, #0]
	ldr	r0, =$0202e4e0
	ldr	r0, [r0, #0]
	mov	r1, #0
	ldr	r2, =$080194bc
	mov	lr, r2
	@dcw	$F800
	ldr	r2, =$08024f20
	mov	lr, r2
	ldr	r2, [next]
	mov	r0, r4
	mov	r1, r5
	@dcw	$F800
	pop	{r4, r5, pc}
@ltorg
next