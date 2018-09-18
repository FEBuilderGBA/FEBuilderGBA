@thumb
@org	$080895b6
	ldr	r1, [r5, #12]
	ldr	r0, [r1, #4]
	mov	r2, #17
	ldsb	r2, [r0, r2]
	ldr	r0, [r1, #0]
	ldrb	r0, [r0, #19]
	lsl	r0, r0, #24
	asr	r0, r0, #24
	add	r2, r2, r0
	ldr	r0, =$02003e06
	mov	r1, #2
	bl	$08004a9c
	b	$080895e4