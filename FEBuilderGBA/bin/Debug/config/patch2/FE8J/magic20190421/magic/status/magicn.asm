@thumb
	push	{r4, lr}
	ldr	r4, =$080168D0	;装備チェック
	mov	lr, r4
	mov	r4, r0
	@dcw	$F800
	lsl	r0, r0, #16
	lsr	r0, r0, #16
	
	mov	r1, r0	
	cmp	r1, #0
	beq	non
	mov	r0, #255
	and	r0, r1
	lsl	r1, r0, #3
	add	r1, r1, r0
	lsl	r1, r1, #2
	ldr	r0, =$080172bc	;アイテム先頭アドレス
	ldr	r0, [r0]		;アイテム先頭アドレスロード
	add	r1, r1, r0
	ldr	r0, [r1, #12]
	cmp	r0, #0
	bne	jump
non
	mov	r0, #0
	b	end
jump
	ldrb	r0, [r0, #8]	;ボデリン補正
	lsl	r0, r0, #24
	asr	r0, r0, #24
end
	mov	r1, r0
	mov	r0, #26
	ldsb	r0, [r4, r0]
	add	r0, r0, r1
	pop	{r4}
	pop	{r1}
	bx	r1
	