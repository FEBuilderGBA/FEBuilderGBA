@thumb
;Dumped from Fire Emblem Destiny
	push	{r4, r5, r6, lr}
	ldr	r4, =$0203F101	;同じです適当な空き領域と思われる
	ldrb	r2, [r4]
	cmp	r2, #2
	beq	two
	cmp	r2, #1
	bne	end
;1おそらく普通の救出(shelter)
ldr	r1, =$080320b0	;=$08032164
mov	lr, r1
@dcw $F800
	b	end
;Pair-UP
two
	mov	r6, r0
	ldr	r4, =$0203A954	;=$0203A958
	ldrb	r0, [r4, #0xD]
ldr	r1, =$08019108	;=$08019430
mov	lr, r1
@dcw $F800
	mov	r5, r0
	ldrb	r0, [r4, #0xC]
ldr	r1, =$08019108	;=$08019430
mov	lr, r1
@dcw $F800
	mov	r4, r0
ldr	r1, =$08037b04	;=$08037A6C
mov	lr, r1
@dcw $F800
	mov	r2, #0x10
	ldsb	r0, [r5, r2]
	mov	r3, #0x11
	ldsb	r1, [r5, r3]
	ldsb	r2, [r4, r2]
	ldsb	r3, [r4, r3]
push	{r4}
ldr	r4, =$0801d838	;=$0801DBD4
mov	lr, r4
@dcw	$F800
pop	{r4}
	mov	r1, r0
	mov	r0, r4
	mov	r2, #0
	mov	r3, r6
push	{r4}
ldr	r4, =$0801d8e0	;=$0801DC7C
mov	lr, r4
@dcw $F800
pop	{r4}
	mov	r0, r5
	mov	r1, r4
ldr	r3, =$08018060	;=$0801834C
mov	lr	r3
@dcw $F800
	mov	r0, r4
ldr	r3, =$080280a0	;=$0802810C(000280a6:C0 46)
mov	lr	r3
@dcw $F800
	ldr	r0, =$03004DF0	;=$03004E50
	ldr	r1, =$03004E10	;=$03004E70
	str	r1, [r0]
	mov	r0, #0xFF
	strb	r0, [r1, #0xC]
	ldr	r2, =$0202BCC2	;カーソルの座標;0202BCC6
	ldrb	r0, [r2]
	strb	r0, [r1, #0x11]	;座標？
	ldrb	r0, [r2, #2]
	strb	r0, [r1, #0x10]	;座標？
	ldr	r4, =$02024E68	;そのままで多分良い
	ldr	r6, =$08A132D0	;=$089A2C48
	mov	r5, #0x3F
loop
	ldr	r0, [r4]
	cmp	r0, r6
	bne	jump1
	ldr	r0, [r4, #0x1C]
	cmp	r0, #0
	beq	jump1
	mov	r0, r4
ldr	r1, =$08002fbc	;$0800306C
mov	lr, r1
@dcw $F800
jump1
	sub	r5, #1
	add	r4, #0x6C
	cmp	r5, #0
	bge	loop
	mov	r0, #0
end
	pop	{r4-r6}
	pop	{r1}
	ldr	r1, =$0803208e
	mov	pc, r1
	