.thumb
@.org	0801b560	0048 8746 XXXXXXXX

	mov	r0, #128
	and	r5, r0
	cmp	r5, #0
	bne	end		@Œ³ˆ—
	mov	r2, #16
	ldsb	r2, [r4, r2]	@À•W
	cmp	r2, #0
	blt	end
	mov	r2, #17
	ldsb	r2, [r4, r2]	@À•W
	cmp	r2, #0
	blt	end
	ldr	r2, [r4]	@‘¶İ
	cmp	r2, #0
	beq	end
	ldr	r0, =0x0801b568
	mov	pc, r0
end:
	ldr	r0, =0x0801b600
	mov	pc, r0
	