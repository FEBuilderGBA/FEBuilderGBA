
.thumb

.include "../CommonDefinitions.inc"

MMBRenderItemList:

	.global	MMBRenderItemList
	.type	MMBRenderItemList, %function

	.set MMBInventoryTile,	EALiterals + 0

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	@ Ensure that there's a unit

	cmp		r1, #0x00
	bne		UnitPOIN

	bx		lr

UnitPOIN:

	mov		r2, r0
	add		r2, #UnitFlag

	ldrb	r2, [r2]
	cmp		r2, #0x00
	beq		Unit

	@ else exit

	bx		lr

Unit:

	push	{r4-r7, lr}

	@ Save proc

	mov		r7, r0

	@ Get max number of items in inventory
	@ so that we don't render something
	@ for an empty item slot

	mov		r6, #InventoryIconCount
	ldrb	r6, [r7, r6]

	@ Get base tile index to avoid loading
	@ it multiple times

	ldr		r4, =MMBInventoryTile
	ldrh	r4, [r4]

	ldr		r5, =MMBInventoryListTable

	@ Table entry:
	@ +0 item slot index
	@ +1 X coordinate
	@ +2 Y coordinate

IconLoop:

	@ Grab an entry's item slot
	@ -1 signals the end of the table

	mov		r0, #0
	ldsb	r0, [r5, r0]
	cmp		r0, #0
	blt		End

	@ Check if unit doesn't have that
	@ many items

	cmp		r0, r6
	bge		End

	@ Combine with tile

	lsl		r0, #1
	add		r2, r0, r4

	ldr		r0, =MMBRenderIconObj
	mov		lr, r0

	ldrb	r0, [r5, #1]
	ldrb	r1, [r5, #2]

	mov		r3, r7

	bllr

	add		r5, #3
	b		IconLoop

End:

	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBInventoryTile
