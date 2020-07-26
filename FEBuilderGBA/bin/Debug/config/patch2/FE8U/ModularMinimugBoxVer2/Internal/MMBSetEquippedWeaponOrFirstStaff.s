
.thumb

.include "../CommonDefinitions.inc"

MMBSetEquippedWeaponOrFirstStaff:

	.global	MMBSetEquippedWeaponOrFirstStaff
	.type	MMBSetEquippedWeaponOrFirstStaff, %function

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r7, lr}

	mov		r4, r0
	mov		r5, r1

	@ Get equipped weapon, -1 for none

	mov		r0, r1
	ldr		r1, =GetEquippedWeaponSlot
	mov		lr, r1
	bllr

	@ If no equipped item, check for staff

	cmp		r0, #0
	bge		End

	mov		r7, #UnitInventory
	mov		r6, #0

StaffLoop:

	mov		r0, r5
	ldrh	r1, [r5, r7]

	ldr		r2, =CanUnitUseStaff
	mov		lr, r2

	bllr

	cmp		r0, #0
	beq		Next

	mov		r0, r6

End:

	mov		r1, #EquippedWeaponIndex
	strb	r0, [r4, r1]

	pop		{r4-r7}
	pop		{r0}
	bx		r0

Next:

	add		r6, #1
	add		r7, #2

	cmp		r6, #5
	bne		StaffLoop

	mov		r0, #0xFF
	b		End

.ltorg

EALiterals:
	@ None
