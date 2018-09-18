@thumb
@org	$0808EAB4
	strb	r4, [r0, #1]
	mov	r0, #5
	lsl	r0, r0, #8
	add	r0, #8
	mov	r1, #0x3E
	strh	r0, [r7, r1]
	ldr	r0, [sp]
	bl	$080168d0
	bl	$080172C0
	ldrh	r0, [r1]