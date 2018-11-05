@thumb
	push {lr}
	mov r2, r0
	lsl r0, r1, #24
	lsr	r0, r0, #24
	cmp r0, #0x89
	beq meti
	b scroll
meti:
	ldr	r0, [r2, #12]
	mov	r1, #128
	lsl	r1, r1, #6
	and	r0, r1
	cmp	r0, #0
	bne	aruyo
	b	naiyo
scroll:
	ldr	r1, =$080174E4
	mov lr, r1
	@dcw $F800
	cmp r0, #0x19
	beq	one
	cmp r0, #0x1A
	beq	two
	cmp r0, #0x1B
	beq	three
	cmp r0, #0x1C
	beq	four
	cmp r0, #0x1D
	beq	five
	cmp r0, #0x30
	beq	six
	cmp r0, #0x31
	beq	seven
	cmp r0, #0x32
	beq	eight
	cmp r0, #0x2F	;0x8A
	beq	nine
	cmp r0, #55	;0xC1
	beq	ten
	b	aruyo
one:
	mov	r1, #0x1
	b merge
two:
	mov	r1, #0x2
	b merge
three:
	mov	r1, #0x4
	b merge
four:
	mov	r1, #0x8
	b merge
five:
	mov	r1, #0x10
	b merge
six:
	mov	r1, #0x20
	b merge
seven:
	mov	r1, #0x40
	b merge
eight:
	mov	r1, #0x80
	b merge
nine:
	mov	r1, #0x80
	lsl r1, r1, #1
	b merge
ten:
	mov	r1, #0x80
	lsl r1, r1, #2
merge:
	ldrh	r0, [r2, #0x3A]
	ldr	r2, [r2]
	ldrh	r2, [r2, #0x26]
	orr	r0, r2
	and	r0, r1
	beq naiyo
aruyo:
	mov r0, #1
	b end
naiyo:
	mov r0, #0
end:
	pop {pc}