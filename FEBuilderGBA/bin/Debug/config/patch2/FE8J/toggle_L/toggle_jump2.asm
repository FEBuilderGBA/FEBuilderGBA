@thumb
@org	$080338a8

	@dcw	$D018
	ldr	r1, [$08033944+4]
	mov	r2, #20
	ldsh	r0, [r1, r2]
	mov	r3, #22
	ldsh	r1, [r1, r3]
	ldr	r2, [$080338d4]
	mov	lr, r2
	@dcw	$F800