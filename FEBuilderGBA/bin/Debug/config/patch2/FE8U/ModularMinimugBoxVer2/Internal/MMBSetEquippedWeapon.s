
.thumb

.include "../CommonDefinitions.inc"

MMBSetEquippedWeapon:

	.global	MMBSetEquippedWeapon
	.type	MMBSetEquippedWeapon, %function

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4, lr}

	mov		r4, r0

	@ Get equipped weapon, -1 for none

	mov		r0, r1
	ldr		r1, =GetEquippedWeaponSlot
	mov		lr, r1
	bllr

	mov		r1, #EquippedWeaponIndex
	strb	r0, [r4, r1]

	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ None
