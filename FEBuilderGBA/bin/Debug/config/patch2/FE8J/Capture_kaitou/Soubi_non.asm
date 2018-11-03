@thumb
;00016328


	ldr	r0, [r4, #12]	;捕獲
	mov		r1,#0x80
	lsl		r1, r1,#0x17
	tst	r0, r1
	beq	Normal
	
	mov		r0,#0xFF
	and		r0,r5
	mov		r1,#0x24
	mul		r0,r1
	ldr		r1,=$080172d4	;ItemTable
	ldr		r1, [r1]
	add		r0,r0,r1
	ldr		r1,[r0,#0x8]		;weapon abilities
	mov		r2,#0x1
	tst		r1,r2
	beq		NextWeapon
	ldrb	r1,[r0,#0x19]		;weapon range
	lsr		r1, r1, #4				;min
	cmp		r1,#0x1
	bne		NextWeapon
Normal:
	mov	r1, #255
	and	r1, r5
	lsl	r0, r1, #3
	add	r0, r0, r1
	ldr	r1, =$08016330
	mov	pc, r1
	
NextWeapon:
	mov	r0, 0
	pop	{r4, r5}
	pop	{r1}
	bx	r1