@thumb
	push	{r4, lr}
	mov	r4, r0
	add	r0, #74
	ldrh	r0, [r0, #0]
		ldr	r1, =$080173b4
		mov	lr, r1
		@dcw	$F800
	ldrb	r1, [r1, #7]
	cmp	r1, #7
	bgt	none
	add	r1, #40
	ldrb	r1, [r1, r4]
	cmp	r1, #250
	bls	none
	mov	r0, #0
none
	mov	r1, r0
	
	ldr	r0, [r4, #4]
	ldrb	r0, [r0, #17]	;クラス体格
	lsl	r0, r0, #24
	asr	r0, r0, #24
	ldr	r2, [r4]
	ldrb	r2, [r2, #19]	;ユニット体格
	lsl	r2, r2, #24
	asr	r2, r2, #24
	add	r0, r0, r2	;r0に合計体格
	
	ldrb	r2, [r4, #21]	;技
	lsr	r2, r2, #2
	add	r0, r0, r2
	
	sub	r1, r1, r0
	cmp	r1, #0
	bge	jump
	mov	r1, #0
jump
	mov	r0, #22
	ldsb	r0, [r4, r0]
	sub	r0, r0, r1
	mov	r1, r4
	add	r1, #94
	strh	r0, [r1, #0]

;マイナスチェック
	lsl	r0, r0, #16
	cmp	r0, #0
	bge	end
	mov	r0, #0
	strh	r0, [r1, #0]
end
	pop	{r4}
	pop	{r0}
	bx	r0