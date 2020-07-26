
.thumb

.include "../CommonDefinitions.inc"

MMBDrawEquippedWeaponNameWithWarningCentered:

	.global	MMBDrawEquippedWeaponNameWithWarningCentered
	.type	MMBDrawEquippedWeaponNameWithWarningCentered, %function

	.set MMBItemNamePosition,			EALiterals + 0
	.set MMBTextAltColor,				EALiterals + 2
	.set MMBTextWarningColor,			EALiterals + 3
	.set MMBItemDurabilityThreshold,	EALiterals + 4
	.set MMBAltTextWidth,				EALiterals + 5

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r7, r14}

	mov		r4, r0
	mov		r5, r1

	@ Get unit's equipped weapon

	mov		r0, #EquippedWeaponIndex
	ldsb	r0, [r4, r0]

	cmp		r0, #0
	blt		End

	lsl		r0, #1
	add		r0, #UnitInventory

	ldrh	r0, [r5, r0]

	@ Save durability

	lsr		r6, r0, #8

	@ Use ID to get item data

	mov		r1, #0xFF
	and		r0, r1

	ldr		r1, =GetROMItemStructPtr
	mov		lr, r1
	bllr

	mov		r7, r0

	@ Write the item name to the text buffer

	ldrh	r0, [r0]
	ldr		r1, =TextBufferWriter
	mov		lr, r1
	bllr

	@ Save pointer for now, write text
	@ info to the proc state

	mov		r5, r0

	mov		r1, r0
	ldr		r0, =MMBAltTextWidth
	ldrb	r0, [r0]
	ldr		r2, =GetStringTextCenteredPos
	mov		lr, r2
	bllr

	@ Save position

	push	{r0}

	add		r4, #AltTextStructStart

	mov		r0, r4
	ldr		r1, =TextClear
	mov		lr, r1
	bllr

	@ Determine which color to use

	ldr		r0, [r7, #ItemDataAttributes]

	@ Unbreakable items and items without
	@ durability get the normal text color.

	mov		r1, #ItemAttributeUnbreakable
	and		r0, r1
	cmp		r0, #0
	bne		NormalColor

	ldrb	r1, [r7, #ItemDataUses]
	cmp		r1, #0
	beq		NormalColor

	@ Check if durability percent is
	@ under the threshold.

	mov		r0, r6
	mov		r2, #100
	mul		r0, r2
	swi		0x06 @ div

	ldr		r1, =MMBItemDurabilityThreshold
	ldrb	r1, [r1]
	cmp		r0, r1
	ble		WarningColor

NormalColor:

	ldr		r2, =MMBTextAltColor
	b		Continue

WarningColor:

	ldr		r2, =MMBTextWarningColor

Continue:

	ldrb	r2, [r2]

	mov		r0, r4
	pop		{r1} @ Position

	ldr		r3, =TextSetParameters
	mov		lr, r3
	bllr

	mov		r0, r4
	mov		r1, r5
	ldr		r2, =TextAppendString
	mov		lr, r2
	bllr

	mov		r0, r4
	ldr		r1, =WindowBuffer
	ldr		r2, =MMBItemNamePosition
	ldrh	r2, [r2]
	add		r1, r2
	ldr		r2, =TextDraw
	mov		lr, r2
	bllr

End:

	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBItemNamePosition
	@ MMBTextAltColor
	@ MMBTextWarningColor
	@ MMBItemDurabilityThreshold
	@ MMBAltTextWidth
