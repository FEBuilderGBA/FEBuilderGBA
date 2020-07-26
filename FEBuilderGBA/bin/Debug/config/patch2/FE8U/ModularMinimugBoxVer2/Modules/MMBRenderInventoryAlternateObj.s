
.thumb

.include "../CommonDefinitions.inc"

MMBRenderInventoryAlternateObj:

	.global	MMBRenderInventoryAlternateObj
	.type	MMBRenderInventoryAlternateObj, %function

	.set MMBInventoryTile,	EALiterals + 0
	.set MMBInventoryX,		EALiterals + 2
	.set MMBInventoryY,		EALiterals + 3

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	@ first check for unit

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

	@ Short out if there are no icons to draw

	mov		r1, #InventoryIconCount
	ldrb	r1, [r0, r1]
	cmp		r1, #0
	bne		Icons

	bx		lr

Icons:

	push	{r4-r7, lr}

	mov		r5, r1
	mov		r7, r0

	@ Check if we need to change the icon index

	add		r0, #HoverFramecount
	ldrb	r1, [r0]

	@ Add 1 here so that the first frame is 1 and not
	@ 0, which would trigger an icon change.

	add		r1, #1
	mov		r0, #0x3F
	and		r1, r0

	mov		r2, #DisplayedItemIndex
	ldrb	r0, [r7, r2]

	ldr		r6, =MMBAlternatingItemListTable

	cmp		r1, #0
	bne		Render

	@ It's been 64 frames, increment index

	add		r0, #1

	@ Check if we've hit the end of the table

	ldrb	r1, [r6, r0]
	cmp		r1, #0xFF
	beq		Reset

	@ Check if unit has enough items to display
	@ that item slot

	cmp		r5, r1
	ble		Reset

	@ Otherwise store new index

	b		SetIndex

Reset:

	mov		r0, #0

SetIndex:

	strb	r0, [r7, r2]

Render:

	ldrb	r0, [r6, r0]

	@ Check if not enough items

	cmp		r5, r0
	ble		End

	ldr		r3, =MMBInventoryTile

	@ Add item index to tile base

	ldrh	r2, [r3]
	lsl		r0, #1
	add		r2, r0

	@ Get coordinates

	ldrb	r0, [r3, #2]
	ldrb	r1, [r3, #3]

	ldr		r3, =MMBRenderIconObj
	mov		lr, r3

	mov		r3, r7

	bllr

End:

	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBInventoryTile
	@ MMBInventoryX
	@ MMBInventoryY
