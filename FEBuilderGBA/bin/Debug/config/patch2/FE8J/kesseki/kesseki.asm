;; r0   result
;; r4   data
@thumb
	push	{r4}
	ldr	r4, [r4, #0]
	ldrb	r4, [r4, #4]
	ldr	r3, =$0x0202BCEC
	ldr	r2,	=$0x09000000	;;change your address
	
loop
	ldrb	r0, [r2, #0x0]
	cmp	r0, 0x0	;;リスト末尾 0x00
	beq	out
unitcheck
	cmp	r4, r0
	bne	next
scenariocheck
	ldrb	r0, [r2, #0x2]
	cmp	r0, #0xFF
	beq	chaptercheck
	ldrb	r1, [r3, #0x1B]
	cmp	r0, r1
	bne	next
chaptercheck
	ldrb	r0, [r2, #0x3]
	cmp	r0, #0xFF
	beq	kesseki				;;全章出撃不可
	ldrb	r1, [r3, #0xE]	;;章読み込み
	cmp	r1, r0
	bne	next
kesseki
	mov	r0, #0x1
	b	end
next
	add	r2, #4
	b	loop
out
	mov	r0, #0x0
end
	pop	{r4}

	ldr	r1,=$08097AE0		;;元に戻す
	mov pc,r1
