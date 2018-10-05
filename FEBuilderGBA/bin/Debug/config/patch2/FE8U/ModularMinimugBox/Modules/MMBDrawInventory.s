
.thumb

.include "../Common Definitions.inc"

MMBDrawInventory:

	.global	MMBDrawInventory
	.type	MMBDrawInventory, %function

	.set MMBInventoryTileIndex,	EALiterals + 0

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r6, r14}

	mov		r4, r0
	mov		r5, r1

	@ Draw the item icon palette to oam palette 4

	ldr		r0, =0x085996F4
	mov		r1, #0x14
	lsl		r1, r1, #0x05
	mov		r2, #0x20
	ldr		r3, =CopyToPaletteBuffer
	mov		r14, r3
	.short 0xF800

	@ loop counter

	mov		r6, #0x00

Loop:

	@ check if we're done

	cmp		r6, #0x05
	beq		End

	@ Get item

	mov		r0, r5
	lsl		r1, r6, #0x01
	add		r1, #0x1E
	ldrb	r0, [r0, r1]
	cmp		r0, #0x00
	beq		End
	ldr		r1, =GetROMItemStructPtr
	mov		r14, r1
	.short 0xF800

	@ get icon

	ldrb	r0, [r0, #0x1D]

	@ get tile index

	ldr		r1, MMBInventoryTileIndex
	lsl		r2, r6, #0x01
	add		r1, r1, r2

	@ draw

	ldr		r2, =RegisterIconOBJ
	mov		r14, r2
	.short 0xF800

	add		r6, r6, #0x01
	b		Loop

End:

	@ Store count

	add		r4, #OAMCount
	strb	r6, [r4]

	pop		{r4-r6}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBInventoryTileIndex
