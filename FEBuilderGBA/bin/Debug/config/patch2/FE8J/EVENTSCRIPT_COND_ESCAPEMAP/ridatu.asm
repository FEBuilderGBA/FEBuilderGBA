@thumb

;400D0000
	push	{lr}
	ldr	r0, =$03004df0
	ldr	r0, [r0]
	ldrb	r1, [r0, #0xB]
	lsr	r1, r1, #6
	bne	end	;自軍以外なら終了
	
	ldrb	r1, [r0, #0xC]
	lsl	r1, r1, #27
	bpl	jump		;担いでないならジャンプ
	ldrb	r0, [r0, #0x1B]
	ldr	r1, =$08019108
	mov	lr, r1
	@dcw	$F800
	ldr	r1, [r0]
	ldrb	r1, [r1, #4]
	ldr	r2, =$030004B0
	ldr	r2, [r2, #4]
	cmp	r1, r2
	beq	end
	
	ldrb	r1, [r0, #0xC]
	mov	r2, #0x8
	orr	r1, r2
	strb	r1, [r0, #0xC]
jump
	ldr	r0, =$03004df0
	ldr	r0, [r0]
	mov	r1, #0
	strb	r1, [r0, #0x13]
	ldrb	r1, [r0, #0xC]
	mov	r2, #0x8
	orr	r1, r2
	strb	r1, [r0, #0xC]

;	ldr	r1, =$0203A954
;	mov	r0, #0xFF
;	strb	r0, [r1, #14]
;	mov	r0, #0x10
;	strb	r0, [r1, #15]
end
	pop	{pc}
	