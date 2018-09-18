@thumb
	
	mov	r0, #20
	ldsb	r0, [r2, r0]
	@dcw	$4664
	add	r4, #116
	mov	r1, #0
	ldsb	r1, [r4, r1]
	add	r0, r0, r1
	ldr	r5, [r2, #4]
	mov	r1, #20
	ldsb	r1, [r5, r1]
	mov	r3, r5
	cmp	r0, r1
	ble	next
	ldrb	r0, [r3, #20]
	ldrb	r1, [r2, #20]
	sub	r0, r0, r1
	strb	r0, [r4, #0]
next
	mov	r0, #26
	ldsb	r0, [r2, r0]
	@dcw	$4664
	add	r4, #122
	mov	r1, #0
	ldsb	r1, [r4, r1]
	add	r0, r0, r1
	ldr	r5, [r2, #4]
	mov	r1, #25
	ldsb	r1, [r5, r1]	;è„å¿
	mov	r3, r5
	cmp	r0, r1
	ble	end
	ldrb	r0, [r3, #25]	;è„å¿
	ldrb	r1, [r2, #26]
	sub	r0, r0, r1
	strb	r0, [r4, #0]
end
	ldr	r0, =$0802bef6
	mov	pc, r0