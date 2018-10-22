@thumb
	
	push	{lr}
	ldr	r4, =$0203F101	;同じです適当な空き領域と思われる
	mov	r0, #2
	strb	r0, [r4]
	ldr	r0, =$03004df0
	ldr	r0, [r0]
	ldr	r1, [next]
	mov	lr, r1
	@dcw	$F800
	ldr	r0, =$080507b0
	mov	lr, r0
	ldr	r0, [next+4]
	@dcw	$F800
	mov	r0, #7
	pop	{pc}
@ltorg
next
	
	
	