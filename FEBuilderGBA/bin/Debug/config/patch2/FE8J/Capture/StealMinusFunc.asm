@thumb
	push	{r4, r5, r6, lr}
	mov	r5, r0
	mov	r0, #11
	ldsb	r0, [r5, r0]
	mov	r1, #192
	and	r0, r1
	cmp	r0, #128
	bne	End
	
;騎馬判定
	ldr	r1, [r5, #4]
	ldr	r1, [r1, #40]
	lsl	r0, r1, #31
	bmi	End
;輸送体判定
	ldr	r1, [r5]
	ldr	r1, [r1, #40]
	ldr	r0, [r5, #4]
	ldr	r0, [r0, #40]
	orr	r1, r0
	lsl	r0, r1, #22
	bmi	End
	
	ldr	r0, =$08018030	;救出判定
	mov	lr, r0
	
	ldr	r0, =$03004df0
	ldr	r0, [r0]
	mov	r1, r5
@dcw	0xF800
	lsl	r0, r0, #24
	cmp	r0, #0
	beq	End
	
	ldr	r0, =$08050630
	mov	lr, r0
	mov	r0, #16
	ldsb	r0, [r5, r0]
	mov	r1, #17
	ldsb	r1, [r5, r1]
	mov	r2, #11
	ldsb	r2, [r5, r2]
	mov	r3, #0
	@dcw	0xF800
End:
	pop	{r4, r5, r6}
	pop	{r0}
	bx	r0