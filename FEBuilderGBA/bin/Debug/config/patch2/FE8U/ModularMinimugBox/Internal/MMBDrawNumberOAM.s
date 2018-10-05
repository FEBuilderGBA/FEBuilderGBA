
.thumb

.include "../Common Definitions.inc"

MMBDrawNumberOAM:

	.global	MMBDrawNumberOAM
	.type	MMBDrawNumberOAM, %function

	@ Inputs:
	@ r0: X coordinate
	@ r1: Y coordinate
	@ r2: Number

	push	{r14}

	@ We'll need all scratch registers,
	@ so we set lr first

	ldr		r3, =PushToSecondaryOAM
	mov		r14, r3

	@ add number to base
	@ 0-9 in r2 is the number
	@ 0x0A in r2 is a dash

	ldr		r3, =0x82E0 @ Number base tile

	add		r3, r2, r3

	@ OAM data for a single 8x8 sprite

	ldr		r2, =0x08590F44

	.short 0xF800

	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ None
