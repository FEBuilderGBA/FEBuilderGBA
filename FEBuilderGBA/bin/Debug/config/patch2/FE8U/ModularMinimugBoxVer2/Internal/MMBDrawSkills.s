
.thumb

.include "../CommonDefinitions.inc"

MMBDrawSkills:

	.global	MMBDrawSkills
	.type	MMBDrawSkills, %function

	.set MMBSkillTileIndex,				EALiterals + 0
	.set MMBSkillObjectPaletteIndex,	EALiterals + 2
	.set MMBSkillSheet,					EALiterals + 3

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r7, lr}

	mov		r4, r0
	mov		r5, r1

	ldr		r0, =ItemIconPalette
	ldr		r1, =MMBSkillObjectPaletteIndex
	ldrb	r1, [r1]
	lsl		r1, r1, #0x05
	mov		r2, #0x20
	ldr		r3, =CopyToPaletteBuffer
	mov		lr, r3
	bllr

	@ Fetch skills

	mov		r0, r5
	ldr		r1, =Skill_Getter
	mov		lr, r1
	bllr

	@ loop counter

	mov		r6, #0x00

	@ pointer to skills

	mov		r7, r0

Loop:

	@ get skill

	ldrb	r0, [r7, r6]

	@ check if we're done

	cmp		r0, #0x00
	beq		End

	@ Icon rework

	ldr		r1, =MMBSkillSheet
	ldrb	r1, [r1]
	lsl		r1, #8
	orr		r0, r1

	@ get tile index

	ldr		r1, =MMBSkillTileIndex
	ldrh	r1, [r1]
	lsl		r2, r6, #0x01
	add		r1, r1, r2

	@ draw

	ldr		r2, =RegisterIconOBJ
	mov		lr, r2
	bllr

	add		r6, r6, #0x01
	b		Loop

End:

	@ Store count

	add		r4, #SkillIconCount
	strb	r6, [r4]

	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBInventoryTileIndex
	@ MMBSkillObjectPaletteIndex
	@ MMBSkillSheet
