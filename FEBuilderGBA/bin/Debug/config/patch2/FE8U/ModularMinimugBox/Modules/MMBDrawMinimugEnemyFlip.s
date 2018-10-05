
.thumb

.include "../Common Definitions.inc"

MMBDrawMinimugEnemyFlip:

	.global	MMBDrawMinimugEnemyFlip
	.type	MMBDrawMinimugEnemyFlip, %function

	.set MMBMinimugXCoordinate,		EALiterals +  0
	.set MMBMinimugYCoordinate,		EALiterals +  4
	.set MMBMinimugTileIndexStart,	EALiterals +  8
	.set MMBMinimugPaletteIndex,	EALiterals + 12

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r5, r14}

	add		sp, #-0x04

	mov		r4, r0
	mov		r5, r1

	@ Get unit's portrait index

	mov		r0, r1
	ldr		r2, =GetPortraitIndex
	mov		r14, r2
	.short 0xF800

	mov		r2, r0

	@ check if enemy

	mov		r1, r5
	mov		r0, #0x0B @ allegiance byte
	ldsb	r0, [r1, r0]
	mov		r1, #0x80 @ enemy
	and		r0, r1
	mov		r1, #0x00
	cmp		r0, #0x00
	beq		Ally

	mov		r1, #0x01

Ally:
	str		r1, [sp] @ flipping flag

	@ Check for the "increase portrait by 1"
	@ bit

	mov		r1, r5

	ldr		r0, [r1, #0x0C]
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

	ldr		r0, =CreateMinimug
	mov		r14, r0

	mov		r0, r2
	ldr		r2, MMBMinimugTileIndexStart
	ldr		r3, MMBMinimugPaletteIndex
	.short 0xF800

	add		sp, #0x04

	pop		{r4-r5}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBMinimugXCoordinate
	@ MMBMinimugYCoordinate
	@ MMBMinimugTileIndexStart
	@ MMBMinimugPaletteIndex

