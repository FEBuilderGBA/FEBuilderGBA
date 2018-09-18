@thumb
;@org	0008e75c

	push	{r4, r5, r6, r7, lr}
	mov	r7, r8
	push	{r7}
	mov	r4, r0
	mov	r5, r1
	add	r0, #68
	mov	r1, #0
	ldsh	r6, [r0, r1]
	
	mov	r0, #63
	and	r0, r6
	cmp	r0, #0
	bne	check
	mov	r0, #64
	and	r0, r6
	cmp	r0, #0
;	beq	start
;	ldr	r0, [r4, #64]
;	ldr	r1, =$0808e688
;	mov	lr, r1
;	mov	r1, r5
;	@dcw	$F800
;	b	dont
	
	
start:
	ldrb	r0, [r5, #8]	;レベル
	bl	numbers
	ldr	r1, =$02028e44
	ldrb	r0, [r1, #6]
	sub	r0, #48
	mov	r2, r4
	add	r2, #81
	strb	r0, [r2, #0]
	ldrb	r0, [r1, #7]
	sub	r0, #48
	mov	r1, r4
	add	r1, #82
	strb	r0, [r1, #0]



	ldrb	r0, [r5, #9]	;経験値
	bl	numbers
	ldr	r1, =$02028e44
	ldrb	r0, [r1, #6]
	sub	r0, #48
	mov	r2, r4
	add	r2, #83
	strb	r0, [r2, #0]
	ldrb	r0, [r1, #7]
	sub	r0, #48
	mov	r1, r4
	add	r1, #84
	strb	r0, [r1, #0]
;文字
	ldr	r1, [r4, #64]
	mov	r0, r4
	mov	r2, #0

	ldr	r3, =$2160
	strh	r3, [r1, #0]
	add	r3, #1
	strh	r3, [r1, #2]
	strh	r2, [r1, #4]
	strh	r2, [r1, #6]
	add	r3, #1
	strh	r3, [r1, #8]
	strh	r2, [r1, #10]
	strh	r2, [r1, #12]
dont:
	mov	r0, #1
	ldr	r1, =$08001efc
	mov	lr, r1
	@dcw	$F800
check:
	mov	r0, r4
	add	r0, #85
	ldrb	r0, [r0, #0]
	lsl	r0, r0, #24
	asr	r0, r0, #24
	bne	end
	ldr	r0, =$0808e838
	mov	pc, r0
end:
	ldr	r0, =$0808e8b4
	mov	pc, r0

numbers:
	ldr	r1, =$08003868
	mov	pc, r1
	

	
