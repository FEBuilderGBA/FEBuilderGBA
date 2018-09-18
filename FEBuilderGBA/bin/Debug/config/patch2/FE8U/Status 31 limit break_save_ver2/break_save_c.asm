@thumb
;000aa8a6
	mov	r3, r12
	ldr	r1, [r3, #36]
	lsl	r1, r1, #15
	lsr	r1, r1, #15
	ldrb	r0, [r7, #20]
	lsl	r0, r0, #17
	orr	r1, r0
	ldrb	r0, [r7, #21]
	lsl	r0, r0, #23
	orr	r1, r0
	ldrb	r2, [r7, #22]
	lsl	r0, r2, #29
	orr	r1, r0
	str	r1, [r3, #36]
	lsr	r1, r2, #3
	ldrb	r0, [r7, #23]
	lsl	r0, r0, #3
	orr	r1, r0
	ldrb	r0, [r7, #24]
	lsl	r0, r0, #9
	orr	r1, r0
	str	r1, [r3, #40]
	ldrb	r0, [r7, #25]	;幸運
	ldrb	r2, [r7, #26]	;体格
	lsr	r1, r2, #5
	lsl	r1, r1, #7
	orr	r0, r1
	strb	r0, [r3, #24]
	
	
	mov	r2, #0
	mov	r4, #0
	mov r6, #0
	add	r7, #40
	str	r2, [r3, #20]
loop
	ldrb	r0, [r7, r2]
	cmp	r0, #0
	beq	test
	cmp	r4, #1
	beq	one
	cmp	r4, #2
	beq	two
	cmp	r4, #3
	beq	three
zero
	strb	r0, [r3, #20]
	lsl r6, r2, #13
	add	r4, #1
	b	test
one
	strb	r0, [r3, #21]
	lsl	r1, r2, #10
	orr r6, r1
	add	r4, #1
	b	test
two
	strb	r0, [r3, #22]
	lsl	r1, r2, #7
	orr r6, r1
	add	r4, #1
	b	test

three
	strb	r0, [r3, #23]
	lsl	r1, r2, #4
	orr r6, r1
	b	end
test
	add	r2, #1
	cmp	r2, #8
	bne	loop
end
	sub	r7, #40
	
	ldrh	r0, [r7, #0x3A]	;
	strb	r0, [r3, #25]	;
	lsr r0, r0, #8
	orr r6, r0
	strh	r6, [r3, #18]	;
	
	
	mov	r6, #7
	ldrb	r2, [r7, #26]
	mov	r4, #31
;000aa936