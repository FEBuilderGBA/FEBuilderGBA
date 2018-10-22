@thumb	;000228d8
	push	{lr}
	ldr	r0, =$03004df0
	ldr	r2, [r0, #0]
	ldr	r1, [r2, #12]
	mov	r0, #64
	and	r0, r1
	cmp	r0, #0
	bne	end
	mov	r0, #129
	lsl	r0, r0, #4
	and	r1, r0
	cmp	r1, #0
	bne	end
	mov	r0, r2
	bl	jump
	ldr	r2, =$0802288c
	mov	pc, r2
end
	mov	r0, #3
	pop	{pc}
@ltorg
jump
	
	
	