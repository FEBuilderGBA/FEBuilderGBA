@thumb
;	@org $0803f4f0	00 48 87 46 XX XX XX XX[アドレス]
	ldr	r4, =$0203AA00
	add	r4, #135
	mov	r0, #0
	strb	r0, [r4]
	ldrb	r0, [r1, #11]
	strb	r0, [r7, #0]
	mov	r0, r1
	add	r0, #66
	ldr	r4, =$0803f4f8
	mov	pc, r4