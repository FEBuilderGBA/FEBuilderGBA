@thumb
;aa200
mov	r6, sp

	ldr	r2, [r6, #4]
	lsl	r0, r2, #14
	lsr	r0, r0, #26
	strb	r0, [r4, #20]
	lsl	r0, r2, #8
	lsr	r0, r0, #26
	strb	r0, [r4, #21]
	lsl	r0, r2, #2
	lsr	r0, r0, #26
	strb	r0, [r4, #22]
	lsr	r1, r2, #30
	ldr	r2, [r6, #8]
	lsl r0, r2, #28
	lsr r0, r0, #26
	orr r0, r1
	strb	r0, [r4, #23]
	
	lsl	r0, r2, #22
	lsr	r0, r0, #26
	strb	r0, [r4, #24]
	lsl	r0, r2, #16
	lsr	r0, r0, #26
	strb	r0, [r4, #25]
	lsl	r0, r2, #11
	lsr	r0, r0, #27
	
	ldrb	r2, [r6, #28]
	lsr	r1, r2, #7
	lsl	r1, r1, #5
	orr r0, r1
	strb	r0, [r4, #26]	;‘ÌŠi
	lsl r0, r2 #25
	lsr r0, r0 #25
	strb	r0, [r4, #18]	;;‘Ì—Í
	
	ldr	r0, [r6, #20]
	lsl r0, r0, #12
	lsr	r0, r0, #20
	strh	r0, [r4, #0x3A]
	
	
	mov	r5, r4
	add	r5, #40
	
	ldr	r2, [r6, #20]
	lsr	r2, r2, #29
	ldrb	r0, [r6, #24]	;•Ší1
	strb	r0, [r5, r2]
	
	ldr	r2, [r6, #20]
	lsl	r2, r2, #3
	lsr	r2, r2, #29
	ldrb	r0, [r6, #25]	;•Ší2
	cmp	r0, #0
	beq	end
	strb	r0, [r5, r2]

	ldr	r2, [r6, #20]
	lsl	r2, r2, #6
	lsr	r2, r2, #29
	ldrb	r0, [r6, #26]	;•Ší3
	cmp	r0, #0
	beq	end
	strb	r0, [r5, r2]

	ldr	r2, [r6, #20]
	lsl	r2, r2, #9
	lsr	r2, r2, #29
	ldrb	r0, [r6, #27]	;•Ší4
	cmp	r0, #0
	beq	end
	strb	r0, [r5, r2]
end:
mov	r5, #1
;	ldr	r0, =$080aa254
	ldr	r0, =$080A583C
	mov pc, r0