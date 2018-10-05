
.thumb

.include "../Common Definitions.inc"

MMBDrawEquippedWeapon:

	.global	MMBDrawEquippedWeapon
	.type	MMBDrawEquippedWeapon, %function

	.set MMBInventoryTileIndex,	EALiterals + 0

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r5, r14}

	mov		r4, r0
	mov		r5, r1

	@ Check if unit has an equipped weapon

	mov		r0, r1
	ldr		r1, =GetEquippedWeapon
	mov		r14, r1
	.short 0xF800

	@ if not, end

	cmp		r0, #0x00
	beq		End

	mov		r1, #0xFF
	and		r0, r1

	ldr		r1, =GetROMItemStructPtr
	mov		r14, r1
	.short 0xF800

	@ get icon

	ldrb	r0, [r0, #0x1D]

	@ get tile index to draw to

	add		r4, #OAMCount
	ldrb	r2, [r4]
	add		r3, r2, #0x01
	strb	r3, [r4] @ increment icon count
	lsl		r2, r2, #0x01
	ldr		r1, MMBInventoryTileIndex
	add		r1, r1, r2

	ldr		r2, =RegisterIconOBJ
	mov		r14, r2
	.short 0xF800

	@ Draw the item icon palette to oam palette 4

	ldr		r0, =0x085996F4
	mov		r1, #0x14
	lsl		r1, r1, #0x05
	mov		r2, #0x20
	ldr		r3, =CopyToPaletteBuffer
	mov		r14, r3
	.short 0xF800

End:

	pop		{r4-r5}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBInventoryTileIndex
