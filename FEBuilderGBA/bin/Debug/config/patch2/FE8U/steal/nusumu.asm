@thumb
;@org	$08025BA0
	push	{r4, r5, r6, lr}
	mov	r5, r0
	ldrb	r0, [r5, #11]
	lsl	r0, r0, #24
	bpl	end
	ldrb	r0, [r5, #22]
	ldr	r1, =$03004E50
	ldr	r1, [r1]
	ldrb	r1, [r1, #22]
	sub	r1, #1	;相手より1速ければ
	cmp	r1, r0
	blt	end
	ldr	r0, [r5]
	ldr	r1, [r5, #4]
	ldr	r0, [r0, #40]
	ldr	r1, [r1, #40]
	orr	r0, r1
	lsl	r0, r0, #22	;輸送隊
	bmi	end
	lsl	r0, r0,	#6	;敵将
	bmi	end
	mov	r6, #0
	mov	r4, r5
	add	r4, #30
loop
	ldrh	r0, [r4, #0]
	cmp	r0, #0
	beq	end
	bl	$08017054
	cmp	r0, #0
	beq	jump
	mov	r0, #16
	ldsb	r0, [r5, r0]
	mov	r1, #17
	ldsb	r1, [r5, r1]
	mov	r2, #11
	ldsb	r2, [r5, r2]
	mov	r3, #0
	bl	$0804f8bc
	b	end
jump
	add	r4, #2
	add	r6, #1
	cmp	r6, #4
	ble	loop
end
	pop	{pc, r4, r5, r6}