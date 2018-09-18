@thumb
	ldr	r2, =$0202BE48
	mov	r3, #0
loop
	ldr	r0, [r2]
	cmp	r0, #0
	beq	non
	ldrb	r0, [r0, #4]
	ldr	r1, =$030004B0
	ldr	r1, [r1, #4]	;ユニット指定
	cmp	r0, r1
	beq	next
	ldr	r1, [r2, #12]
	mov	r0, #4	;戦死
	and	r0, r1
	bne	next
	lsl	r0, r1, #15	;離脱
	bmi	next
	mov	r0, #0x20	;被救出
	and	r0, r1
	bne	next
	mov	r0, #8
	and	r1, r0
	beq	shutugeki
next
	add	r2, #0x48
	add	r3, #1
	cmp	r3, #51
	blt	loop
non
	mov	r0, #1	;410Cでジャンプ400Cで素通り
	b	end
shutugeki
	mov	r0, #0	;400Cでジャンプ410Cで素通り
end
	ldr	r1, =$030004B0
	str	r0, [r1, #48]
	mov	r0, #0
	bx	lr