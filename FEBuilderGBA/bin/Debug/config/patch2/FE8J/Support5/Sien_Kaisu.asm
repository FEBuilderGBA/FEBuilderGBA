@thumb
;	0002821a > 00 20
;	@org	$080282CC > 00 48 87 46 XX XX XX XX
	ldr	r4, =$0802830C
	ldr	r4, [r4]
	mov	r0, #50
	add	r0, r0, r6
	ldrb	r7, [r5, r0]	;対象の支援値ロード
	cmp	r7, #80
	ble	checkC
	cmp	r7, #160
	bgt	checkA
	b	judge
checkC
	mov	r0, r5
	bl	youkaizo
	cmp	r0, #5		;最大支援人数
	bge	non
	mov	r0, r5
	mov	r1, r6
ldr	r3, =$08028188
mov	lr, r3
@dcw	$F800
	bl	youkaizo
	cmp	r0, #5		;最大支援人数
	bge	non
	b	judge
checkA
	mov	r0, r5
	bl AloopC
	cmp	r0, #0
	bne	non
	mov	r0, r5
	mov	r1, r6
ldr	r3, =$08028188
mov	lr, r3
@dcw	$F800
	bl AloopC
	cmp	r0, #0
	bne	non
judge
	mov	r0, r5
	mov	r1, r6
ldr	r3, =$080281d0
mov	lr, r3
@dcw	$F800
	ldr	r4, =$085C3e88
	lsl	r0, r0, #2
	ldr	r0, [r0, r4]
	cmp	r7, #241
	beq	non
	cmp	r7, r0
	bne	non
	mov	r0, #1
	b	end
non
	mov	r0, #0
end
	pop	{pc, r4, r5, r6, r7}
	
	
AloopC
	push	{lr, r4}
	mov	r4, r0
	mov	r2, #0
loop
	mov	r0, r4
	mov	r1, r2
ldr	r3, =$080281d0
mov	lr, r3
@dcw	$F800
	cmp	r0, #3
	beq	got
	add r2, #1
	cmp	r2, #7
	bne	loop
	mov	r0, #0
	b	return
got
	mov	r0, #1
return
	pop	{pc, r4}
	
	
	
	
youkaizo
	push	{r4, r5, r6, r7, lr}
	mov	r7, r0
ldr	r3, =$0802815c
mov	lr, r3
@dcw	$F800
	mov	r5, r0	;;r5最大支援数
	mov	r4, #0
	mov	r6, #0
	cmp	r6, r5
	bge	jump
lonlon
	mov	r0, r7
	mov	r1, r4
ldr	r3, =$080281d0
mov	lr, r3
@dcw	$F800
	cmp	r0, #0
	beq	nonke
	add	r6, #1
nonke
	add	r4, #1
	cmp	r4, r5
	blt	lonlon
jump
	mov	r0, r6
	pop	{pc, r4-r7}