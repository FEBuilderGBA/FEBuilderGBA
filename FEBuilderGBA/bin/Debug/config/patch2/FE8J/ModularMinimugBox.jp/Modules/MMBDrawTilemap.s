
.thumb

.include "../Common Definitions.inc"

MMBDrawTilemap:

	.global	MMBDrawTilemap
	.type	MMBDrawTilemap, %function

	.set MMBTilemap,				EALiterals + 0
	.set MMBTilemapPaletteIndex,	EALiterals + 4

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4, r14}

	mov		r4, r1

	@ Draw the tilemap

	ldr		r0, =WindowBufferBG1
	ldr		r1, MMBTilemap
	mov		r2, #0xC0 @ 0x3000
	lsl		r2, r2, #0x06
	ldr		r3, =DrawTilemap
	mov		r14, r3
	.short 0xF800

	@ fetch palette based on allegiance

	mov		r1, r4
	mov		r0, #0x0B @ allegiance byte
	ldsb	r0, [r1, r0]
	mov		r1, #0xC0
	and		r0, r1
	ldr		r1, MMBTilemapPaletteIndex
	ldr		r2, =GetPaletteByAllegiance
	mov		r14, r2
	.short 0xF800

	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBTilemap
	@ MMBTilemapPaletteIndex


