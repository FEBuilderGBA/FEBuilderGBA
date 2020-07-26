
.thumb

.include "../CommonDefinitions.inc"

MMBDrawMinimug:

	.global	MMBDrawMinimug
	.type	MMBDrawMinimug, %function

	.set MMBMinimugXCoordinate,		EALiterals +  0
	.set MMBMinimugYCoordinate,		EALiterals +  4
	.set MMBMinimugTileIndexStart,	EALiterals +  8
	.set MMBMinimugPaletteIndex,	EALiterals + 12

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4, lr}

	add		sp, #-0x04

	mov		r4, r1

	@ Get unit's portrait index

	mov		r0, r1
	ldr		r2, =GetPortraitIndex
	mov		lr, r2
	bllr

	@ Check for the "increase portrait by 1"
	@ bit

	mov		r2, r0
	mov		r1, r4

	ldr		r0, [r1, #UnitState]
	mov		r1, #0x80
	lsl		r1, r1, #0x10
	and		r0, r1
	cmp		r0, #0x00
	beq		NotIncreased

	add		r2, #0x01

NotIncreased:

	@ Get position

	ldr		r0, =WindowBuffer
	ldr		r1, MMBMinimugXCoordinate
	ldr		r3, MMBMinimugYCoordinate

	lsl		r1, r1, #0x01
	lsl		r3, r3, #0x06

	add		r1, r1, r3
	add		r1, r0, r1

	@ draw

	mov		r0, #0x00
	str		r0, [sp] @ flipping flag

	ldr		r0, =CreateMinimug
	mov		lr, r0

	mov		r0, r2
	ldr		r2, MMBMinimugTileIndexStart
	ldr		r3, MMBMinimugPaletteIndex
	bllr

	add		sp, #0x04

	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBMinimugXCoordinate
	@ MMBMinimugYCoordinate
	@ MMBMinimugTileIndexStart
	@ MMBMinimugPaletteIndex

