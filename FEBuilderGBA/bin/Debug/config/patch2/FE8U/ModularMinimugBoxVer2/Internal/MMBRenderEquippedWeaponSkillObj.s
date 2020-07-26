
.thumb

.include "../CommonDefinitions.inc"

MMBRenderEquippedWeaponSkillObj:

	.global	MMBRenderEquippedWeaponSkillObj
	.type	MMBRenderEquippedWeaponSkillObj, %function

	.set MMBEquippedItemSkillTileIndex,	EALiterals + 0
	.set MMBEquippedItemSkillX,			EALiterals + 2
	.set MMBEquippedItemSkillY,			EALiterals + 3

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	@ Short out if nothing to render

	mov		r1, r0
	add		r1, #WeaponSkillFlag
	ldrb	r1, [r1]
	cmp		r1, #0
	bne		Start

	bx		lr

Start:

	push	{r4, lr}

	mov		r4, r0

	ldr		r3, =MMBEquippedItemSkillTileIndex
	ldrh	r2, [r3]
	ldrb	r0, [r3, #2]
	ldrb	r1, [r3, #3]

	ldr		r3, =MMBRenderIconObj
	mov		lr, r3

	mov		r3, r4

	bllr

	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBEquippedItemSkillTileIndex
	@ MMBEquippedItemSkillX
	@ MMBEquippedItemSkillY
