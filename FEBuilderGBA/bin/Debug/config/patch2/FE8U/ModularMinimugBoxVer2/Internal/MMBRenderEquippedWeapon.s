
.thumb

.include "../CommonDefinitions.inc"

MMBRenderEquippedWeapon:

	.global	MMBRenderEquippedWeapon
	.type	MMBRenderEquippedWeapon, %function

	.set MMBInventoryTile,	EALiterals + 0
	.set MMBInventoryX,		EALiterals + 2
	.set MMBInventoryY,		EALiterals + 3

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4, lr}

	mov		r4, r0

	@ Get equipped weapon slot

	mov		r1, #EquippedWeaponIndex
	ldsb	r0, [r0, r1]

	@ -1 means no item

	cmp		r0, #0
	blt		End

	@ Otherwise r0 contains the slot,
	@ use to get the proper icon tile

	ldr		r3, =MMBInventoryTile

	ldrh	r2, [r3]
	lsl		r0, #1
	add		r2, r0

	ldrb	r0, [r3, #2]
	ldrb	r1, [r3, #3]

	ldr		r3, =MMBRenderIconObj
	mov		lr, r3

	mov		r3, r4

	bllr

End:

	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBInventoryTile
	@ MMBInventoryX
	@ MMBInventoryY
