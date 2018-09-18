;危険コマンド。カウンタ初期化時に0ではなく0x80をストア
@thumb
	push	{lr}
	ldr	r0, =$080226e0
	ldr	r0, [r0]
	mov	r1, #0
	str	r1, [r0, #0]
	mov	r1, #0x80
	ldr	r0, =$080226e4
	ldr	r0, [r0]
	add	r0, #62
	strb	r1, [r0]
	ldr	r0, =$080226ce
	mov	pc, r0
	