@thumb

	push	{r4, r5, lr}
	mov	r4, r0	
	ldr	r5, =$02033f38
	ldr	r0, [r5, #0]
	ldrb	r0, [r0, #11]
	lsl	r0, r0, #24
	asr	r0, r0, #24
	mov	r1, #11
	ldsb	r1, [r4, r1]	;;所属IDロード
eor r1, r0
eor r0, r1
eor r1, r0
ldr	r3, =$08024d54	;所属一致チェック
mov	lr, r3
@dcw	$F800
	lsl	r0, r0, #24
	cmp	r0, #0
	beq	end
	ldr	r2, [r5, #0]
	ldr	r0, [r2, #4]
	ldrb	r0, [r0, #4]
	cmp	r0, #81
	beq	end
	ldr	r0, [r4, #4]
	ldrb	r0, [r0, #4]
	cmp	r0, #81
	beq	end

	mov	r0, r4
	add	r0, #48
	ldrb	r1, [r0, #0]
	mov	r0, #15
	and	r0, r1
	cmp	r0, #4
	beq	end		;バサーク不可
	cmp	r0, #2
	beq	end		;スリープ不可
	ldr	r1, [r2, #4]	;騎馬
	ldr	r1, [r1, #40]
	lsl	r0, r1, #31
	bmi	end
	
	ldr	r0, [r2, #12]
	mov	r1, #48
	and	r0, r1
	cmp	r0, #0
	bne	end		;救出中なら不可
	
	ldr	r0, [r4, #12]
	mov	r1, #48
	and	r0, r1
	cmp	r0, #0
	bne	end		;救出中なら不可
eor r4, r2
eor r2, r4
eor r4, r2
	mov	r5, r2	;違い
	mov	r0, r2
	mov	r1, r4
ldr	r3, =$08018030
mov	lr, r3
@dcw	$F800
	lsl	r0, r0, #24
	cmp	r0, #0
	beq	end

ldr	r3, =$08050630
mov	lr, r3
	mov	r0, #16
	ldsb	r0, [r5, r0]	;違い
	mov	r1, #17
	ldsb	r1, [r5, r1]	;違い
	mov	r2, #11
	ldsb	r2, [r5, r2]	;違い
	mov	r3, #0
@dcw	$F800
end
	pop	{r4, r5}
	pop	{r0}
	bx	r0