@thumb
@org	$0801c5bc

	@dcw	$D018
	ldr	r1, [$0801c66c+4]
	mov	r2, #20
	ldsh	r0, [r1, r2]
	mov	r3, #22
	ldsh	r1, [r1, r3]
	ldr	r2, [$0801c5e8]
	mov	lr, r2
	@dcw	$F800