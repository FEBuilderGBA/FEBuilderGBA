@thumb
@org	$0801c95c

	lsl	r1, r1, #30
	lsr	r1, r1, #30
	beq	$0801c978
	cmp	r1, #1
	beq	$0801c978
	mov	r0, #5
	b	$0801c97a