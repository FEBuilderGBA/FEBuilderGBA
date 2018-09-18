@thumb
	mov	r1, r0
	cmp	r1, #0
	beq	zero
	mov	r0, #255
	and	r0, r1
	lsl	r1, r0, #3
	add	r1, r1, r0
	lsl	r1, r1, #2
	ldr	r0, =$080161e8
	ldr	r0, [r0]
	add	r1, r1, r0
	ldr	r0, [r1, #12]
	cmp	r0, #0
	bne	jump
zero
	mov	r0, #0
	b	end
jump
	ldrb	r0, [r0, #8]
	lsl	r0, r0, #24
	asr	r0, r0, #24
end
	mov	r1, #26
	ldsb	r1, [r4, r1]
	add	r1, r1, r0
	ldr	r0, =$08018758
	mov	pc, r0