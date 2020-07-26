
.thumb

.include "../CommonDefinitions.inc"

MMBRenderEquippedWeaponIfPlayerOrInventoryObj:

	.global	MMBRenderEquippedWeaponIfPlayerOrInventoryObj
	.type	MMBRenderEquippedWeaponIfPlayerOrInventoryObj, %function

	.set MMBInventoryTile,	EALiterals + 0
	.set MMBInventoryX,		EALiterals + 2
	.set MMBInventoryY,		EALiterals + 3

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

	mov		r7, r0

	@ Check if player unit

	ldrb	r0, [r1, #UnitDeploymentNumber]
	mov		r2, #0xC0
	and		r0, r2
	cmp		r0, #0x00
	bne		Inventory

	@ Otherwise render just the equipped item

	mov		r0, #EquippedWeaponIndex
	ldsb	r0, [r7, r0]

	@ -1 for no item, display full inventory

	cmp		r0, #0
	blt		Inventory

	ldr		r3, =MMBInventoryTile

	ldrh	r2, [r3]
	lsl		r0, #1
	add		r2, r0

	ldrb	r0, [r3, #2]
	ldrb	r1, [r3, #3]

	ldr		r3, =MMBRenderIconObj
	mov		lr, r3

	mov		r3, r7

	bllr

	b		End

.ltorg

Inventory:

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
	@ MMBInventoryX
	@ MMBInventoryY
