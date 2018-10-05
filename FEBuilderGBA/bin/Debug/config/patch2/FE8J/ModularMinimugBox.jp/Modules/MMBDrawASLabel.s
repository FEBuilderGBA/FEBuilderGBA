
.thumb

.include "../Common Definitions.inc"

MMBDrawASLabel:

	.global	MMBDrawASLabel
	.type	MMBDrawASLabel, %function

	.set MMBASLabelX,			EALiterals +  0
	.set MMBASLabelY,			EALiterals +  4
	.set MMBASLabelGraphic,		EALiterals +  8
	.set MMBASVRAMTile,			EALiterals + 12
	.set MMBLabelPaletteIndex,	EALiterals + 16

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r14}

	@ Queue graphics to be written

	ldr		r0, MMBASLabelGraphic

	@ calculate VRAM position

	mov		r1, #0x06
	lsl		r1, r1, #0x18 @ 0x06000000
	ldr		r2, MMBASVRAMTile
	lsl		r2, r2, #0x05 @ tile # * 0x20 bytes per tile
	add		r1, r1, r2
	mov		r2, #0x40

	ldr		r3, =RegisterTileGraphics
	mov		r14, r3

	.short 0xF800

	@ Get tilemap position

	ldr		r0, MMBASLabelX
	ldr		r1, MMBASLabelY

	ldr		r2, =WindowBuffer

	lsl		r0, r0, #0x01
	lsl		r1, r1, #0x06

	add		r0, r1, r0
	add		r0, r0, r2


	ldr		r2, MMBLabelPaletteIndex
	lsl		r2, r2, #0x0C
	ldr		r1, MMBASVRAMTile
	add		r1, r1, r2
	strh	r1, [r0]
	add		r1, #0x01
	strh	r1, [r0, #0x02]

	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBASLabelX
	@ MMBASLabelY
	@ MMBASLabelGraphic
	@ MMBASVRAMTile
	@ MMBLabelPaletteIndex
