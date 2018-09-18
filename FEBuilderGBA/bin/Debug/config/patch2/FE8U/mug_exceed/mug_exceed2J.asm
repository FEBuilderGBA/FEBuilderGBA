@thumb
	push	{r4, r5, r6, r7, lr}
	mov	r7, r12
	mov	r6, r10
	mov	r5, r9
	mov	r4, r8
	push	{r4, r5, r6, r7}
	mov	r6, r0
	mov	r7, r1
	mov	r8, r2	;退避

	ldrb	r5, [r2, #20]
	sub	r5, #1
	ldrb	r4, [r2, #21]
	ldr	r1, =$085A0838 ; =$085c8d58
	
	ldr	r2, =$080d74a0 ; =$080dc0dc //CallARM_FillTileRect 
	mov	lr, r2
	lsl	r2, r7, #16
	lsr	r2, r2, #16
	@dcw	$F800
	lsl	r4, r4, #5
	add	r4, r4, r5
	lsl	r4, r4, #1
	add	r2, r4, r6
	mov	r0, r7
	add	r0, #28
	strh	r0, [r2, #0]
	add	r0, #1
	strh	r0, [r2, #2]
	add	r0, #1
	strh	r0, [r2, #4]
	add	r0, #1
	strh	r0, [r2, #6]
	mov	r1, r2
	add	r1, #64
	add	r0, #29
	strh	r0, [r1, #0]
	add	r1, #2
	add	r0, #1
	strh	r0, [r1, #0]
	add	r1, #2
	add	r0, #1
	strh	r0, [r1, #0]
	add	r1, #2
	add	r0, #1
	strh	r0, [r1, #0]
;整理

mov	r5, r7
mov	r7,	r6
mov	r6, r8

	ldrb	r3, [r6, #26]
	cmp	r3, #1
	beq	jump
end:
	pop	{r1, r2, r3, r4}
	mov	r8, r1
	mov	r9, r2
	mov	r10, r3
	mov	r12, r4
	pop	{r4, r5, r6, r7}
	pop	{r1}
	bx	r1
	nop
	nop
	b	end
@ltorg
jump: