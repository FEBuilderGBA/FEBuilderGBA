@thumb
@org	$0802D7EC	;FE8U
	ldr	r1, [r0]
	ldrh	r1, [r1, #6]
	cmp	r1, #0
	bne	ok
	mov	r1,	#0xAC
ok
	mov	r0, #2
	str	r0, [sp, #0]
	mov	r0, #1
	mov	r2, #176
	mov	r3, r4
	bl	$0800563c	;FE8U
	mov	r0, #0
	mov	r1, #5
	bl	$08006458	;FE8U
	mov	r0, #1
	mov	r1, #5
	bl	$08006458	;FE8U
	mov	r0, #3
	bl	$08001fac	;FE8U
	add	sp, #4
	pop	{r4-r5, pc}