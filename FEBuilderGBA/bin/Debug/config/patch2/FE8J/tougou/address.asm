@thumb

	push	{lr}
@dcw	$B081
	ldr	r1, [r0, #44]
	ldr	r2, [r0, #48]
	mov	r3, pc
	add	r3, #0x8
	ldr	r0, [r0, #52]
	str	r0, [sp]
	ldr	r0, =$0808b70a
	mov	pc, r0
@incbin address.bin