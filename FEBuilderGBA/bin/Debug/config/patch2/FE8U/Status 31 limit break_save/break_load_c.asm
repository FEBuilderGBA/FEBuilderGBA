@thumb
;@org $080aab5e
@org $080A6146

	mov	r7, sp
	mov	r5, sp
	add	r5, #26
	ldrb	r0, [r5, #10]
	lsl	r0, r0, #27
	lsr	r0, r0, #27
	strb	r0, [r6, #8]
	ldrb	r0, [r7, #16]
	strb	r0, [r6, #9]
	ldr	r0, [sp, #4]
	str	r0, [r6, #12]
	ldrh	r0, [r7, #36]
	lsl	r0, r0, #21
	lsr	r0, r0, #26
	strb	r0, [r6, #16]
	ldr	r2, [sp, #36]
	lsl	r0, r2, #15
	lsr	r0, r0, #26
	strb	r0, [r6, #17]
	ldrb	r0, [r7, #14]
	strb	r0, [r6, #18]
	ldrb	r0, [r7, #15]
	strb	r0, [r6, #19]
	lsl	r0, r2, #9
	lsr	r0, r0, #26
	strb	r0, [r6, #20]
	lsl	r0, r2, #3
	lsr	r0, r0, #26
	strb	r0, [r6, #21]
	lsr	r1, r2, #29
	ldr	r2, [sp, #40]
	lsl	r0, r2, #29
	lsr	r0, r0, #26
	orr	r0, r1
	strb	r0, [r6, #22]
	lsl	r0, r2, #23
	lsr	r0, r0, #26
	strb	r0, [r6, #23]
	lsl	r0, r2, #17
	lsr	r0, r0, #26
	strb	r0, [r6, #24]
	
	ldrb	r1, [r7, #24]	;幸運+体格
	lsl	r0, r1, #26
	lsr	r0, r0, #26
	strb	r0, [r6, #25]	;幸運
	lsr	r1, r1, #7
	lsl	r1, r1, #5
	lsl	r0, r2, #12
	lsr	r0, r0, #27
	orr	r0, r1
	strb	r0, [r6, #26]	;体格
	
	ldrb	r0, [r5, #16]
	lsl	r0, r0, #25
	mov	r4, r6
	add	r4, #48
	lsr	r0, r0, #29
	ldrb	r1, [r4, #0]
	mov	r3, #16
	neg	r3, r3
	and	r3, r1
	orr	r3, r0
	strb	r3, [r4, #0]
	lsl	r0, r3, #28
	lsr	r2, r0, #28
	ldrb	r1, [r7, #2]
	mov	r0, #128
	mov	r8, r0
	and	r0, r1
	cmp	r0, #0
	@dcw $d001
	mov	r0, #8
	orr	r2, r0
	ldrh	r0, [r5, #16]
	lsl	r0, r0, #22
	lsr	r0, r0, #29
	lsl	r0, r0, #4
	mov	r3, #15
	and	r2, r3
	orr	r2, r0
	strb	r2, [r4, #0]
	ldrb	r1, [r5, #17]
	lsl	r0, r1, #27
	lsr	r0, r0, #29
	lsr	r1, r1, #5
	lsl	r1, r1, #4
	and	r0, r3
	orr	r0, r1
	mov	r2, #49
	strb	r0, [r6, r2]
	ldrb	r0, [r7, #3]
	strb	r0, [r6, #27]
	ldrb	r0, [r5, #18]
	lsl	r0, r0, #28
	lsr	r0, r0, #28
	strb	r0, [r6, #29]
	ldrb	r2, [r5, #22]
	mov	r0, #127
	and	r0, r2
	strb	r0, [r6, #28]
	ldrh	r5, [r7, #8]
@align 4
	ldr	r1, [$080A636C] ;[$080aad84]
	mov	r0, r1
	and	r0, r5
	strh	r0, [r6, #30]
	ldrh	r4, [r7, #10]
	mov	r0, r1
	and	r0, r4
	strh	r0, [r6, #32]
	ldrh	r3, [r7, #12]
	and	r1, r3
	strh	r1, [r6, #34]
	ldr	r0, [sp, #44]
	lsl	r0, r0, #14
	lsr	r0, r0, #18
	strh	r0, [r6, #36]
	ldrh	r0, [r7, #46]
	lsr	r0, r0, #2
	strh	r0, [r6, #38]
	mov	r1, r8
	and	r1, r2
	lsl	r1, r1, #24
	lsr	r1, r1, #31
	mov	r2, #192
	lsl	r2, r2, #8
	mov	r0, r2
	and	r0, r5
	lsr	r0, r0, #13
	orr	r0, r1
	mov	r1, r2
	and	r1, r4
	lsr	r1, r1, #11
	orr	r1, r0
	and	r2, r3
	lsr	r2, r2, #9
	orr	r2, r1
	mov	r0, #57
	strb	r2, [r6, r0]
	mov	r1, r6
	add	r1, #64
	str	r1, [sp, #56]
	add	r1, #3
	str	r1, [sp, #60]
	add	r1, #1
	str	r1, [sp, #64]
	add	r1, #1
	str	r1, [sp, #68]
	add	r1, #1
	str	r1, [sp, #72]
	mov	r1, sp
	add	r1, #49
	str	r1, [sp, #52]
	mov	r2, #0
	add	r6, #40
	str	r2, [r6, #0]
	str	r2, [r6, #4]
	ldrh	r3, [r7, #18]
;いち
	ldrb	r0, [r7, #20]
	cmp	r0, #0
	beq	end
	lsr	r1, r3, #13
	strb	r0, [r6, r1]
;に
	ldrb	r0, [r7, #21]
	cmp	r0, #0
	beq	end
	lsl r1, r3, #19
	lsr	r1, r1, #29
	strb	r0, [r6, r1]
;さん
	ldrb	r0, [r7, #22]
	cmp	r0, #0
	beq	end
	lsl r1, r3, #22
	lsr	r1, r1, #29
	strb	r0, [r6, r1]
;よん
	ldrb	r0, [r7, #23]
	cmp	r0, #0
	beq	end
	lsl r1, r3, #25
	lsr	r1, r1, #29
	strb	r0, [r6, r1]
end:
	lsl r1, r3, #28
	lsr	r1, r1, #20
	
	ldrb	r0, [r7, #25]
	orr r0, r1
	sub	r6, #40
	strh	r0, [r6, #0x3A]	;
	
	
	mov	r1, #66
	add	r1, r1, r6
	mov	r10, r1
	mov	r0, sp
	add	r0, #33
	mov	r12, r0
	add	r0, #1
	mov	r8, r0
	add	r0, #1
	mov	r9, r0
	mov	r3, sp
	add	r3, #26
	mov	r4, r6
	add	r4, #50
	b	$080A62E8 ;$080aad00