
.thumb

.include "../Common Definitions.inc"

MMBDrawSkillAlternatingObjs:

	.global	MMBDrawSkillAlternatingObjs
	.type	MMBDrawSkillAlternatingObjs, %function

	.set MMBHeight,			EALiterals + 0
	.set MMBSkillTile,		EALiterals + 4
	.set MMBSkillsX,		EALiterals + 6
	.set MMBSkillsY,		EALiterals + 7

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

	@ check if skill needs to be drawn

	add		r0, #OAMCount2
	ldrb	r0, [r0]
	cmp		r0, #0x00
	beq		End

	@ check we need to change icon

	mov		r1, r4
	add		r1, #HoverFramecount
	ldrb	r1, [r1]
	mov		r2, #0x3F
	and		r1, r2
	cmp		r1, #0x00
	bne		NoChange

	@ else change drawn index

	sub		r0, r0, #0x01
	mov		r1, r4
	add		r1, #DisplayedIndex
	ldrb	r2, [r1]
	cmp		r0, r2
	beq		Max

	@ otherwise inc

	add		r2, r2, #0x01
	strb	r2, [r1]
	b		NoChange

Max:

	mov		r0, #0x00
	strb	r0, [r1]

NoChange:

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

	ldr		r0, =MMBSkillTile
	ldrh	r3, [r0]

	ldrb	r2, [r0, #0x03]
	ldrb	r0, [r0, #0x02]
	add		r1, r1, r2
	add		r4, #DisplayedIndex
	ldrb	r4, [r4]
	lsl		r4, r4, #0x01
	add		r3, r3, r4
	ldr		r2, =0x08590F4C @ sprite data for a 16x16 sprite
	.short 0xF800

End:
	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBHeight
	@ MMBSkillTile
	@ MMBSkillsX
	@ MMBSkillsY
