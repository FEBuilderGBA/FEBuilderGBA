@thumb
	ldr	r1, =$0203E184
	ldr	r1, [r1, #4]
	ldr	r0, [r1, #4]
	ldrb	r0, [r0, #4]
	add	r1, #0x4A
	ldrb	r1, [r1]
	lsl	r1, r1, #8
	add	r0, r0, r1
	@align 4
	ldr	r1, [adr]
	strh	r0, [r1]
	ldr	r1, =$0203E184
	ldr	r1, [r1]
	ldr	r0, [r1, #4]
	ldrb	r0, [r0, #4]
	add	r1, #0x4A
	ldrb	r1, [r1]
	lsl	r1, r1, #8
	add	r0, r0, r1
	@align 4
	ldr	r1, [adr]
	strh	r0, [r1, #2]
	ldr	r1, [adr+4]
	mov	pc, r1
@ltorg
adr