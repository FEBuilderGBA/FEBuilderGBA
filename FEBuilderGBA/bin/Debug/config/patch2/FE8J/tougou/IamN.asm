@thumb
	lsl	r0, r0, #2
	add	r0, r0, r4
	ldrh	r0, [r0, #2]
	push	{r4, r5}
	mov	r4, r0	;r4に持ち出しアイテム
	mov	r5, r1	;r5に最終ストア箇所
	
	ldr	r2, =$085775cc
	ldr	r2, [r2]
	ldrh	r2, [r2, #4]
	lsl	r2, r2, #22
	bpl	okee
	
ldr	r1, =$08017358 ;アイテムの使用上限をロード
mov	lr, r1
@dcw	$F800
	cmp	r0, #0xFF
	beq	okee
	mov	r3, r0	;r3に上限
	ldr	r2, [r7, #44]
	add	r2, #30
loopstart
	lsl	r1, r4, #24
	lsr	r1, r1, #24
	ldrb	r0, [r2]
	cmp	r0, #0
	bne	item
okee
	strh	r4, [r5]
	b	end
item
	cmp	r0, r1
	bne	loop
	ldrh	r0, [r2]
	lsr	r0, r0, #8
	lsr	r1, r4, #8
	add	r0, r0, r1
	cmp	r0, r3
	ble	tarinai
	ldrh	r0, [r2]
	lsr	r0, r0, #8
	sub	r0, r3, r0	;r0に余り
	lsl	r0, r0, #8
	sub	r4, r4, r0	;アイテム減算完了
	
	lsl	r0, r3, #8
	ldrh	r1, [r2]
	lsl	r1, r1, #24
	lsr	r1, r1, #24	;IDのみ
	orr	r1, r0
	strh	r1, [r2]	;アイテム統合完了
loop
	add	r2, #2
	b	loopstart
tarinai
	ldrh	r1, [r2]
	lsl	r0, r0, #8
	lsl	r1, r1, #24
	lsr	r1, r1, #24
	orr	r1, r0
	strh	r1, [r2]
end
	pop	{r4, r5}
	ldr	r0, =$080a05cc
	mov	pc, r0
@ltorg
adr: