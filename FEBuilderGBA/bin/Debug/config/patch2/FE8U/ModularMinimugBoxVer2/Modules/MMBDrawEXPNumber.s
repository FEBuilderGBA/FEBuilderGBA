
.thumb

.include "../CommonDefinitions.inc"

MMBDrawEXPNumber:

	.global	MMBDrawEXPNumber
	.type	MMBDrawEXPNumber, %function

	.set MMBEXPX,	EALiterals + 0
	.set MMBEXPY,	EALiterals + 4
	.set MMBHeight,				EALiterals + 8

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit

	push	{r4-r7, lr}

	mov		r4, r0
	mov		r5, r1

	@ Check if we're on a unit to avoid
	@ bad number drawing

	mov		r0, r4
	add		r0, #UnitFlag

	ldrb	r0, [r0]
	cmp		r0, #0x00
	bne		End

	mov		r0, r5
	cmp		r0, #0x00
	beq		End

	@ Get positions for numbers

	ldr		r6, MMBEXPX
	ldr		r7, MMBEXPY

	@ check for lower window

	mov		r0, r4
	add		r0, #WindowPosType
	ldrb	r0, [r0]
	lsl		r0, r0, #0x03
	ldr		r1, =WindowSideTable
	add		r0, r1, r0
	mov		r1, #0x03
	ldsb	r0, [r0, r1] @ -1 top 1 bottom
	cmp		r0, #0x00
	blt		SkipBottom

	ldr		r0, MMBHeight
	mov		r1, #20
	sub		r1, r1, r0

	lsl		r1, r1, #0x03
	add		r7, r7, r1

SkipBottom:

	mov		r0, r5
	ldrb	r2, [r0, #UnitExperience]

	mov		r0, r6
	mov		r1, r7

	ldr		r3, =MMBDrawUnsignedNumber
	mov		lr, r3

	bllr

End:

	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBEXPX
	@ MMBEXPY
	@ MMBHeight
