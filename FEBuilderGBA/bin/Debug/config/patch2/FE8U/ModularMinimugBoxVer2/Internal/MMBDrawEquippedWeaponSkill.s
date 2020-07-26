
.thumb

.include "../CommonDefinitions.inc"

MMBDrawEquippedWeaponSkill:

	.global	MMBDrawEquippedWeaponSkill
	.type	MMBDrawEquippedWeaponSkill, %function

	.set MMBEquippedItemSkillTileIndex,			EALiterals + 0
	.set MMBSkillObjectPaletteIndex,			EALiterals + 2
	.set MMBSkillSheet,							EALiterals + 3

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4, r5, lr}

	mov		r4, r0
	mov		r5, r1

	@ Draw skill icon palette

	ldr		r0, =ItemIconPalette
	ldr		r1, =MMBSkillObjectPaletteIndex
	ldrb	r1, [r1]
	lsl		r1, r1, #0x05
	mov		r2, #0x20
	ldr		r3, =CopyToPaletteBuffer
	mov		lr, r3
	bllr

	@ Get equipped item's skill ID

	mov		r0, r5
	ldr		r1, =GetEquippedWeapon
	mov		lr, r1
	bllr

	mov		r1, #0xFF
	and		r0, r1
	cmp		r0, #0
	beq		End

	ldr		r1, =GetROMItemStructPtr
	mov		lr, r1
	bllr

	mov		r1, #ItemDataItemSkill
	ldrb	r0, [r0, r1]

	cmp		r0, #0
	beq		End

	@ Draw the icon

	@ Icon rework

	ldr		r1, =MMBSkillSheet
	ldrb	r1, [r1]
	lsl		r1, #8
	orr		r0, r1

	ldr		r1, =MMBEquippedItemSkillTileIndex
	ldrh	r1, [r1]

	ldr		r2, =RegisterIconOBJ
	mov		lr, r2
	bllr

	@ Flag that equipped item has a skill

	mov		r0, #1

End:

	mov		r1, #WeaponSkillFlag
	strb	r0, [r4, r1]

	pop		{r4, r5}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBEquippedItemSkillTileIndex
	@ MMBSkillObjectPaletteIndex
	@ MMBSkillSheet
