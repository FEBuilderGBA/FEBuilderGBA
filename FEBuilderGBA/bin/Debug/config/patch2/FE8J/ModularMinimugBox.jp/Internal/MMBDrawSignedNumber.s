
.thumb

.include "../Common Definitions.inc"

MMBDrawSignedNumber:

	.global	MMBDrawSignedNumber
	.type	MMBDrawSignedNumber, %function

	@ Inputs:
	@ r0: X Coordinate
	@ r1: Y Coordinate
	@ r2: Number

	push	{r4-r7, r14}

	mov		r4, r0
	mov		r5, r1

	@ Flag for negative numbers

	mov		r7, #0x00

	cmp		r2, #0x00
	bge		Positive

	@ Set flag for negative number, negate number

	mov		r7, #0x01
	neg		r2, r2

Positive:

	@ Write number to buffer

	mov		r0, r2
	ldr		r1, =WriteNumberBuffer
	mov		r14, r1
	.short 0xF800

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
	mov		r14, r3
	.short 0xF800

	@ Decrememnt stuff

	sub		r4, #0x07 @ Width
	sub		r6, #0x01

	b		NumberLoop

EndLoop:

	@ Check if we need to display a -

	cmp		r7, #0x00
	beq		NoSign

	@ Draw a -

	mov		r0, r4
	mov		r1, r5
	mov		r2, #0x0A
	ldr		r3, =MMBDrawNumberOAM
	mov		r14, r3
	.short 0xF800

NoSign:

	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg
