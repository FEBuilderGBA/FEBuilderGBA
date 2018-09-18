@thumb
	ldr	r5, =$02003BFC
	ldr	r0, =$08018ecc
	mov	lr, r0
	ldr	r0, [r5, #12]
	@dcw	$F800
	str	r0, [sp]
	
	ldr	r1, [r5, #12]
	mov	r3, #26
	ldsb	r3, [r1, r3]	;ëÃäiì«Ç›çûÇ›
	
	ldr	r0, [r1, #4]
	ldrb	r0, [r0, #25]	;ëÃäiè„å¿ì«Ç›çûÇ›
	lsl	r0, r0, #24
	asr	r0, r0, #24
	str	r0, [sp, #4]
	mov	r0, #7
	mov	r1, #5	;xé≤
	ldr	r2, =$08089354
	mov	lr, r2
	mov	r2, #3	;yé≤
	@dcw	$F800
	
	@dcw	$B014
	pop	{r4-r7, pc}