
.thumb

.include "../CommonDefinitions.inc"

MMBDrawNameCentered:

	.global	MMBDrawNameCentered
	.type	MMBDrawNameCentered, %function

	.set MMBTextWidth,			EALiterals +  0
	.set MMBTextColor,			EALiterals +  4
	.set MMBNameXCoordinate,	EALiterals +  8
	.set MMBNameYCoordinate,	EALiterals + 12

	@ Inputs:
	@ r0: Pointer to MMB Proc State
	@ r1: Pointer to unit in RAM

	push	{r4-r7, lr}

	add		r0, #NameTextStructStart
	mov		r4, r0
	mov		r5, r1

	@ First, write the unit's name to
	@ the text decompression buffer

	ldr		r0, [r1]
	ldrh	r0, [r0]

	ldr		r1, =TextBufferWriter
	mov		lr, r1
	bllr

	@ save resulting width for later

	mov		r6, r0
	mov		r1, r0

	ldr		r0, MMBTextWidth @ multiplied by 8 in EA
	ldr		r2, =GetStringTextCenteredPos
	mov		lr, r2
	bllr

	@ save resulting padding distance

	mov		r7, r0

	@ clear an area in VRAM for text

	mov		r0, r4
	ldr		r1, =TextClear
	mov		lr, r1
	bllr

	@ we write the text info to the proc state

	mov		r0, r4
	mov		r1, r7
	ldr		r2, MMBTextColor

	ldr		r3, =TextSetParameters
	mov		lr, r3
	bllr

	@ Write name

	mov		r0, r4
	mov		r1, r6

	ldr		r2, =TextAppendString
	mov		lr, r2
	bllr

	@ write tilemap

	ldr		r1, =WindowBuffer
	ldr		r2, MMBNameXCoordinate
	ldr		r3, MMBNameYCoordinate
	lsl		r3, r3, #0x06
	lsl		r2, r2, #0x01
	add		r2, r2, r3
	add		r1, r1, r2
	mov		r0, r4
	ldr		r2, =TextDraw
	mov		lr, r2
	bllr

	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBTextWidth
	@ MMBTextColor
	@ MMBNameXCoordinate
	@ MMBNameYCoordinate





