@thumb
;支援チェック
	push	{lr}
	mov	r0, r6
	mov	r1, r7
ldr	r2, =$8028738
mov	lr, r2
@dcw	$F800
	lsl	r1, r0, #24
	mov	r0, #0
	cmp	r1, #0
	beq	end
	mov	r0, #1
end
	pop	{pc}
	
	