
.thumb

.include "../CommonDefinitions.inc"

MMBDrawEquippedWeapon:

	.global	MMBDrawEquippedWeapon
	.type	MMBDrawEquippedWeapon, %function

	.set MMBInventoryTileIndex,	EALiterals + 0

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r5, lr}

	mov		r4, r0
	mov		r5, r1

	@ Check if unit has an equipped weapon

	mov		r0, r1
	ldr		r1, =GetEquippedWeapon
	mov		lr, r1
	bllr

	@ if not, end

	cmp		r0, #0x00
	beq		End

	mov		r1, #0xFF
	and		r0, r1

	ldr		r1, =GetROMItemStructPtr
	mov		lr, r1
	bllr

	@ get icon

	ldrb	r0, [r0, #ItemDataIconID]

	@ get tile index to draw to

	add		r4, #InventoryIconCount
	ldrb	r2, [r4]
	add		r3, r2, #0x01
	strb	r3, [r4] @ increment icon count
	lsl		r2, r2, #0x01
	ldr		r1, MMBInventoryTileIndex
	add		r1, r1, r2

	ldr		r2, =RegisterIconOBJ
	mov		lr, r2
	bllr

	@ Draw the item icon palette to oam palette 4

	ldr		r0, =ItemIconPalette
	mov		r1, #0x14
	lsl		r1, r1, #0x05
	mov		r2, #0x20
	ldr		r3, =CopyToPaletteBuffer
	mov		lr, r3
	bllr

End:

	pop		{r4-r5}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBInventoryTileIndex
