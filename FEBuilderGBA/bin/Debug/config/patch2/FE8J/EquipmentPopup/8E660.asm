@thumb
;@org	$0808e660

	ldr	r2, =$2120
	strh	r2, [r1, #0]
	ldr	r3, =$2121
	strh	r3, [r1, #2]
	mov	r2, #0
	strh	r2, [r1, #4]
	strh	r2, [r1, #6]
	add	r3, #29
	strh	r3, [r1, #8]
	strh	r2, [r1, #10]
	strh	r2, [r1, #12]
	bx	lr