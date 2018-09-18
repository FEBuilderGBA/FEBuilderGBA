@thumb

	ldr	r0, [r4, #4]
	ldr	r1, [r2, #40]
	ldr	r0, [r0, #40]
	orr	r1, r0
	mov	r0, #128
	lsl	r0, r0, #8
	and	r1, r0
	cmp	r1, #0
	beq	start
	ldr	r0, =$080278e8
	mov	pc,	r0
start
	mov	r1, #11
	ldsb	r1, [r4, r1]
	mov	r0, #0x40
	and	r1, r0
	ldr	r2, [r4, #0]
	cmp	r1, #0
	bne	end
	ldrb	r0, [r4, #13]
	lsl	r0, r0, #27
	bpl	end
	mov	r1, #16
	ldsb	r1, [r4, r1]	;座標
	lsl	r1, r1, #4
	ldr	r2, =$0202bcac
	mov	r5, #12
	ldsh	r0, [r2, r5]
	sub	r3, r1, r0
	mov	r0, #17
	ldsb	r0, [r4, r0]	;座標
	lsl	r0, r0, #4
	mov	r4, #14
	ldsh	r1, [r2, r4]
	sub	r2, r0, r1

	mov	r1, r3
	add	r1, #16
	mov	r0, #128
	lsl	r0, r0, #1
	cmp	r1, r0
	bhi	end
	mov	r0, r2
	add	r0, #16
	cmp	r0, #176
	bhi	end

	ldr	r5, =$0209
	add	r0, r3, r5
	ldr	r1, =$01ff
	and	r0, r1
	ldr	r3, =$0107
	add	r1, r2, r3
	mov	r2, #255
	and	r1, r2
	
ldr	r3, =$08002b08
mov	lr, r3
	ldr	r2, =$085b8cdc
;	ldr	r3, =$804	;下三角アイコン指定
	ldr	r3, =$877	;右下アイコン指定
@dcw	$F800
end
	ldr	r0, =$08027994
	mov	pc, r0
	
	