@The source code is from FE8J.
@thumb
@org	$08015518
	push	{lr}
	ldr	r0, [$080155a8+4]	;0202bcec
	ldrb	r0, [r0, #0xF]
	cmp	r0, #0
	bne	end
	ldr	r1, [$08015560+4]
	mov	r0, #9
	strb	r0, [r1, #22]
	mov	r0, #3
	bl	$080aa5e4
end
	pop	{pc}

@org	$08015534
	ldr	r0, [$080155a8]	;0202bcec
	
@org	$08015560
@dcd	$0203a954


;オンヘルプ
;0xAAF730:24 00
;オフヘルプ
;0xAAF738:24 00