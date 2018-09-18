@thumb
	ldr	r5, =$02003BFC
	ldr	r0, =$08018ec4
	mov	lr, r0
	ldr	r0, [r5, #12]
	@dcw	$F800
	
	ldr	r0, [r5, #12]
	ldrb	r0, [r0, #26]
	
	ldr	r1, [r5, #12]
	mov	r3, #26
	ldsb	r3, [r1, r3]	;体格読み込み
	str	r0, [sp, #0]
	ldr	r0, [r1, #4]
	ldrb	r0, [r0, #25]	;体格上限読み込み
	lsl	r0, r0, #24
	asr	r0, r0, #24
	str	r0, [sp, #4]
	mov	r0, #7
	mov	r1, #5	;x軸
	ldr	r2, =$08089354
	mov	lr, r2
	mov	r2, #3	;y軸
	@dcw	$F800
;外指揮
;	ldr	r5, =$02003BFC
	ldr	r2, [r5, #12]
	ldr	r2, [r2]
	mov	r1, #0x25
	ldrb	r2, [r2, r1]
	cmp	r2, #0
	bne	jump
	mov	r2, #255
jump
	ldr	r0, =$020040C6
	ldr	r1, =$08004a9c
	mov	lr, r1
	mov	r1, #2
	@dcw	$F800
	
	@dcw	$B014
	pop	{r4-r7, pc}