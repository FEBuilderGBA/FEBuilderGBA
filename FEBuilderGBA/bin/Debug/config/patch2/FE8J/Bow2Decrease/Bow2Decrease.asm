@thumb
	
	ldr	r0, [r5, #76]
	mov	r1, #130
	and	r0, r1
	bne	decrease
	mov	r0, #30
	ldrh	r0, [r5, r0]
		bl	WEAPON
	cmp	r0, #3
	beq	decrease
	cmp	r0, #2
	beq	non
	ldr	r1, =$0203a4d0
	ldrb	r1, [r1, #2]
	cmp r1, #1
	bne	decrease
non
	ldr	r0, =$0802b770
	mov	pc, r0
decrease
	ldr	r0, =$0802b756
	mov	pc, r0
	

WEAPON
	ldr	r3, =$080172f0
	mov	pc, r3