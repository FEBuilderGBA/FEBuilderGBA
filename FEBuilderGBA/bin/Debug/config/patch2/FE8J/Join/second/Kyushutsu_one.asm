@thumb
@org	$080228a4
	push	{lr}
	ldr	r0, [$080229b4+4]
	ldr	r0, [r0]
	bl	$08025364
	mov	r0, #1
	ldr	r1, =$0203F101
	strb	r0, [r1]
	ldr	r0, =$085c5958
	b	$080229a8