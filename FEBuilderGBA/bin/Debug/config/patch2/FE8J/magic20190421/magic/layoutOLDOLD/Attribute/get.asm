@thumb
		ldr	r0, =$080890bc
		mov	lr, r0
	ldr	r0, [mojimoji]
	@dcw	$F800	;=文字描写
	ldr	r5, =$02003EB4	;=$02003f84
	ldr	r4, =$02003bfc
		ldr	r0, =$08028650
		mov	lr, r0
	ldr	r0, [r4, #12]
	@dcw	$F800
	mov	r1, r0		;;アイコン指定
	mov	r2, #160	
	lsl	r2, r2, #7
		ldr	r0, =$08003608
		mov	lr, r0
	mov	r0, r5
	add	r0, #0		;アイコンの横位置修正
	@dcw	$F800
;属性の文字
	mov	r5, r4
	add	r5, #0x80
		ldr	r0, =$080286a4
		mov	lr, r0
	ldr	r0, [r4, #12]
	ldr	r0, [r0, #0]
	ldrb	r0, [r0, #9]
	@dcw	$F800
	mov	r3, r0
		ldr	r0, =$080043b8
		mov	lr, r0
	mov	r0, r5
	mov	r1, #0x2C;#0x28 標準	;#0x2E	文字の横位置右いっぱい
	mov	r2, #2
	@dcw	$F800
	
	mov	r10, r5
	
;指揮
	ldr	r2, [r4, #12]
	ldr	r2, [r2]
	mov	r1, #0x25
	ldrb	r2, [r2, r1]
	cmp	r2, #0
	bne	jumpShiki
	mov	r2, #255
jumpShiki:
	ldr	r1, =$08004a9c
	mov	lr, r1
	ldr	r0, =$02003EC6
	mov	r1, #2
	@dcw	$F800
	
	pop	{r4, r5, r6, r7}
	pop	{r0}
	bx	r0
@ltorg
mojimoji