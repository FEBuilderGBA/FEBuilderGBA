
.thumb

.include "../CommonDefinitions.inc"

MMBGetUnitAtCursor:

	.global	MMBGetUnitAtCursor
	.type	MMBGetUnitAtCursor, %function

	@ Inputs:
	@ None

	@ Outputs:
	@ r0: Unit Allegiance Byte
	@ r1: X coordinate
	@ r2: Y coordinate

	push	{lr}

	@ See Teq's Doc for info about this
	@ Grab cursor coordinates

	ldr		r0, =GameDataStruct

	mov		r2, #0x16 @ Y coordinate
	ldsh	r2, [r0, r2] @ These are signed according to vanilla loading?

	mov		r1, #0x14 @ X coordinate
	ldsh	r1, [r0, r1]

	@ Fetch unit map stuff, calculate map offset, fetch byte

	ldr		r0, =UnitMap
	ldr		r0, [r0] @ Fetch pointer to row table

	lsl		r3, r2, #0x02 @ Multiply Y by 4 to get row offset
	add		r0, r0, r3
	ldr		r0, [r0] @ Pointer to row

	add		r0, r0, r1 @ Add X coordinate
	ldrb	r0, [r0]

	@ Return

	pop		{r3}
	bx		r3

	.ltorg

	.align 2, 0

EALiterals:
	@ None
