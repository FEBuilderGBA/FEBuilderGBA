
.thumb

.include "../CommonDefinitions.inc"

MMBDrawUnsignedNumber:

	.global	MMBDrawUnsignedNumber
	.type	MMBDrawUnsignedNumber, %function

	@ Inputs:
	@ r0: X Coordinate
	@ r1: Y Coordinate
	@ r2: Number

	push	{r4-r6, lr}

	mov		r4, r0
	mov		r5, r1

	@ Write number to buffer

	mov		r0, r2
	ldr		r1, =WriteNumberBuffer
	mov		lr, r1
	bllr

	ldr		r6, =NumberBuffer + 7 @ Last digit in buffer

	@ Draw numbers until we hit a space

NumberLoop:

	ldrb	r2, [r6]
	cmp		r2, #0x20
	beq		EndLoop

	@ Draw

	sub		r2, #0x30
	mov		r0, r4
	mov		r1, r5
	ldr		r3, =MMBDrawNumberOAM
	mov		lr, r3
	bllr

	@ Decrememnt stuff

	sub		r4, #0x07 @ Width
	sub		r6, #0x01

	b		NumberLoop

EndLoop:

	pop		{r4-r6}
	pop		{r0}
	bx		r0

.ltorg
