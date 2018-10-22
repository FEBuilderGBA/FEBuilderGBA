@thumb
;00025130

	
	ldr	r0, =$03004df0
	ldr	r0, [r0]
	ldr	r0, [r0, #0xC]
	mov		r1,#0x80
	lsl		r1,r1,#0x17
	tst	r0, r1
	bne	Test
	
	
Normal:
	ldr	r0, =$08025160
	ldr	r0, [r0]
	ldr	r0, [r0]
	ldrb	r0, [r0, #11]
	lsl	r0, r0, #24
	ldr	r1, =$08025138
	mov	pc, r1
Test:
;騎馬判定
	ldr	r1, [r4, #4]
	ldr	r1, [r1, #40]
	lsl	r0, r1, #31
	bmi	End
;輸送体判定
	ldr	r1, [r4]
	ldr	r1, [r1, #40]
	ldr	r0, [r4, #4]
	ldr	r0, [r0, #40]
	orr	r1, r0
	lsl	r0, r1, #22
	bmi	End
	
	ldr	r0, =$08018030	;救出判定
	mov	lr, r0
	
	ldr	r0, =$03004df0
	ldr	r0, [r0]
	mov	r1, r4
@dcw	0xF800
	lsl	r0, r0, #24
	cmp	r0, #0
	beq	End
	b	Normal
End:
	pop	{r4}
	pop	{r0}
	bx	r0
	
	