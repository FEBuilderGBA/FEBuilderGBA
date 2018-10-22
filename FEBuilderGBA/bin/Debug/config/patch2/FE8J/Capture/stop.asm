@thumb

	push	{r4, r5, r6, r7, lr}
	lsl	r0, r0, #16
	lsr	r6, r0, #16


	ldr	r2, =$0203a4e8
	ldr	r0, [r2, #0xC]
	mov		r1,#0x80
	lsl		r1,r1,#0x17
	tst	r0, r1
	beq	GoBack
;騎馬判定
	ldr	r2, =$0203a568
	ldr	r1, [r2, #4]
	ldr	r1, [r1, #40]
	lsl	r0, r1, #31
	bmi	GoBack
;輸送体判定
	ldr	r1, [r2]
	ldr	r1, [r1, #40]
	ldr	r0, [r2, #4]
	ldr	r0, [r0, #40]
	orr	r1, r0
	lsl	r0, r1, #22
	bmi	GoBack
	
	ldr	r0, =$0203a4e8
	ldrb	r0,[r0,#0xB]
	ldr	r1, =$08019108	;GetCharaData
	mov	lr, r1
	@dcw	0xF800
	mov	r4, r0
	
	ldr	r0, =$0203a568
	ldrb	r0,[r0,#0xB]
	ldr	r1, =$08019108	;GetCharaData
	mov	lr, r1
	@dcw	0xF800
	
	mov	r1, r0
	ldr	r0, =$08018030
	mov	lr, r0
	mov	r0, r4
@dcw	0xF800
	lsl	r0, r0, #24
	cmp	r0, #0
	beq	GoBack
;近接判定
	ldr	r0,	=$0203a4d2
	ldrb	r0, [r0]	;距離
	cmp	r0, #1
	bne	GoBack
	
	mov	r0, #0
	pop	{r4, r5, r6, r7}
	pop	{r1}
	bx	r1
GoBack:
	ldr	r4, =$080869f8
	ldr	r4, [r4]
	ldr	r0, =$080869b8
	mov	pc, r0

