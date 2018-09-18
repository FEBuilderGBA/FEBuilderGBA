@thumb
@org	$08057A7A
	cmp	r0, #0
	beq	$08057AB4 ; FE8J= 080589fc
	cmp	r0, #3
	beq	$08057AB4 ; FE8J= 080589fc
	cmp	r0, #1
	bne	$08057AB4 ; FE8J= 080589fc
animeoff
	;mov	r2, r10
	;ldr	r0, [r2, #0]
	mov r5, r9
	ldr	r0, [r5, #0]
	ldrh	r0, [r0, #0x28]
	lsl	r0, r0, #0x10
	lsr	r0, r0, #0x1f
	bne	$08057AB4 ; FE8J= 080589fc