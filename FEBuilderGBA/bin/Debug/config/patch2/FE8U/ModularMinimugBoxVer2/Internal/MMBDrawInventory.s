
.thumb

.include "../CommonDefinitions.inc"

MMBDrawInventory:

	.global	MMBDrawInventory
	.type	MMBDrawInventory, %function

	.set MMBInventoryTileIndex,				EALiterals + 0
	.set MMBInventoryObjectPaletteIndex,	EALiterals + 2
	.set MMBItemSheet,						EALiterals + 3

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r6, lr}

	mov		r4, r0
	mov		r5, r1

	ldr		r0, =ItemIconPalette
	ldr		r1, =MMBInventoryObjectPaletteIndex
	ldrb	r1, [r1]
	lsl		r1, r1, #0x05
	mov		r2, #0x20
	ldr		r3, =CopyToPaletteBuffer
	mov		lr, r3
	bllr

	@ loop counter

	mov		r6, #0x00

Loop:

	@ check if we're done

	cmp		r6, #0x05
	beq		End

	@ Get item

	mov		r0, r5
	lsl		r1, r6, #0x01
	add		r1, #UnitInventory
	ldrb	r0, [r0, r1]
	cmp		r0, #0x00
	beq		End
	ldr		r1, =GetROMItemStructPtr
	mov		lr, r1
	bllr

	@ get icon

	ldrb	r0, [r0, #ItemDataIconID]

	@ This is to comply with the icon rework
	@ if it is installed.

	ldr		r1, =MMBItemSheet
	ldrb	r1, [r1]
	lsl		r1, #8
	orr		r0, r1

	@ get tile index

	ldr		r1, =MMBInventoryTileIndex
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

	add		r4, #InventoryIconCount
	strb	r6, [r4]

	pop		{r4-r6}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBInventoryTileIndex
	@ MMBInventoryObjectPaletteIndex
	@ MMBItemSheet
