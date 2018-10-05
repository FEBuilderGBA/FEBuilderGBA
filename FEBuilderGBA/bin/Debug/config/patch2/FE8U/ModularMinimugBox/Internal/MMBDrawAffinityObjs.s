
.thumb

.include "../Common Definitions.inc"

MMBDrawAffinityObjs:

	.global	MMBDrawAffinityObjs
	.type	MMBDrawAffinityObjs, %function

	.set MMBHeight,			EALiterals + 0

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	@ first check for unit

	cmp		r1, #0x00
	bne		UnitPOIN

	bx r14

UnitPOIN:
	mov		r2, r0
	add		r2, #UnitFlag

	ldrb	r2, [r2]
	cmp		r2, #0x00
	beq		Unit

	@ else exit

	bx		r14

Unit:

	push	{r4, r14}

	mov		r4, r0

	@ check if affinity needs to be drawn

	add		r0, #AffinityFlag
	ldrb	r0, [r0]
	cmp		r0, #0x00
	beq		End

	@ check for lower window

	mov		r0, r4
	add		r0, #0x50
	ldrb	r0, [r0]
	lsl		r0, r0, #0x03
	ldr		r1, =WindowSideTable
	add		r0, r1, r0
	mov		r1, #0x03
	ldsb	r0, [r0, r1] @ -1 top 1 bottom

	mov		r1, #0x00

	cmp		r0, #0x00
	blt		SkipBottom

	ldr		r0, MMBHeight
	mov		r1, #20
	sub		r1, r1, r0

	lsl		r1, r1, #0x03

SkipBottom:

	@ draw

	ldr		r3, =PushToSecondaryOAM
	mov		r14, r3

	mov		r3, r4
	add		r3, #AffinityTile
	ldrh	r3, [r3]

	add		r4, #AffinityX
	ldrb	r0, [r4]
	ldrb	r2, [r4, #0x01]
	add		r1, r1, r2
	ldr		r2, =0x08590F4C @ sprite data for a 16x16 sprite
	.short 0xF800

End:
	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBHeight
