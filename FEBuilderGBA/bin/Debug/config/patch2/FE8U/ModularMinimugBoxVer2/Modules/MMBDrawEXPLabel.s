
.thumb

.include "../CommonDefinitions.inc"

MMBDrawEXPLabel:

	.global	MMBDrawEXPLabel
	.type	MMBDrawEXPLabel, %function

	.set MMBEXPLabelX,				EALiterals +  0
	.set MMBEXPLabelY,				EALiterals +  4
	.set EXPGraphic,				EALiterals +  8
	.set MMBEXPVRAMTile,			EALiterals + 12
	.set MMBLabelPaletteIndex,		EALiterals + 16

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{lr}

	@ Queue graphics to be written

	ldr		r0, EXPGraphic

	@ calculate VRAM position

	mov		r1, #0x06
	lsl		r1, r1, #0x18 @ 0x06000000
	ldr		r2, MMBEXPVRAMTile
	lsl		r2, r2, #0x05 @ tile # * 0x20 bytes per tile
	add		r1, r1, r2
	mov		r2, #0x40

	ldr		r3, =RegisterTileGraphics
	mov		lr, r3

	bllr

	@ Get tilemap position

	ldr		r0, MMBEXPLabelX
	ldr		r1, MMBEXPLabelY

	ldr		r2, =WindowBuffer

	lsl		r0, r0, #0x01
	lsl		r1, r1, #0x06

	add		r0, r1, r0
	add		r0, r0, r2


	ldr		r2, MMBLabelPaletteIndex
	lsl		r2, r2, #0x0C
	ldr		r1, MMBEXPVRAMTile
	add		r1, r1, r2
	strh	r1, [r0]
	add		r1, #0x01
	strh	r1, [r0, #0x02]

	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBEXPLabelX
	@ MMBEXPLabelY
	@ EXPGraphic
	@ MMBEXPVRAMTile
	@ MMBLabelPaletteIndex
