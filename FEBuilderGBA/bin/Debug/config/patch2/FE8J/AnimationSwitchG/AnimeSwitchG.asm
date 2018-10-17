@thumb
@org	$0802c9a8
	ldr	r1, [r0, #12]
	lsl	r0, r1, #17
	bpl	tugi
	mov	r0, #0
	b	end
tugi
	lsl	r0, r1, #16
	bmi	san
off:
	mov	r0, #1
	b	end
san
	mov	r0, #3
end
	ldr	r1, [adr4]	;$02024CC0
	ldrh	r1, [r1, #4]
	lsl	r1, r1, #22
	bpl	nonpress
	cmp	r0, #1
	beq	jump
	mov	r0, #1
	bx	lr
jump
	mov	r0, #3
nonpress
	bx	lr
	
	
	ldr	r0, [adr1+4]	;+4を付けなくてもいいはずだがズレるので付ける
	ldrb	r0, [r0, #0xF]
	lsr	r0, r0, #6
	beq	non
	ldr	r1, [adr4]	;$02024CC0
	ldrh	r1, [r1, #4]
	lsl	r1, r1, #31
	bpl	non
	b	off		;Aおしっぱでジャンプ
non
	ldr	r0, [adr1+4]	;$0202bcec
	add	r0, #66
	ldrb	r0, [r0]
	lsl	r0, r0, #29
	lsr	r0, r0, #30
	cmp	r0, #2
	bne	end		;個別以外は終了。オフ=1, 背景=3
;;個別の処理
	ldr	r2, [adr2]	;$0203a4e8
	mov	r0, #11
	ldsb	r0, [r2, r0]
	lsl	r0, r0, #24
	bmi	next
	ldr	r0, [adr3+4]	;$0203a568
	mov	r1, #0x0B
	ldsb	r0, [r0, r1]
	lsl	r0, r0, #24
	bmi	two
	ldr	r0, [adr2]	;$0203a4e8
	b	three
next
	ldr	r2, [adr3]	;$0203a568
	mov	r0, #11
	ldsb	r0, [r2, r0]
	lsl	r0, r0, #24
	bpl	two
	mov	r0, #1
	b	end
two
	mov	r0, r2
three
	b	$0802c9a8
adr1	@dcd	$0202bcec
adr2	@dcd	$0203a4e8
adr3	@dcd	$0203a568
adr4	@dcd	$02024CC0
