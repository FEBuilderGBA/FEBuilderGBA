
.thumb

.include "../CommonDefinitions.inc"

MMBDrawEquippedWeaponNameIfPlayerCentered:

	.global	MMBDrawEquippedWeaponNameIfPlayerCentered
	.type	MMBDrawEquippedWeaponNameIfPlayerCentered, %function

	.set MMBTextAltColor,		EALiterals + 0
	.set MMBItemNamePosition,	EALiterals + 2
	.set MMBAltTextWidth,		EALiterals + 4

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r6, lr}

	mov		r4, r0

	ldrb	r0, [r1, #UnitDeploymentNumber]
	mov		r2, #0xC0
	and		r0, r2
	cmp		r0, #0x00
	bne		End

	@ Otherwise player unit, draw name

	mov		r0, #EquippedWeaponIndex
	ldsb	r0, [r4, r0]

	cmp		r0, #0
	blt		End

	lsl		r0, #1
	add		r0, #UnitInventory

	ldrh	r0, [r1, r0]
	mov		r1, #0xFF
	and		r0, r1

	ldr		r1, =GetROMItemStructPtr
	mov		lr, r1
	bllr

	ldrh	r0, [r0]
	ldr		r1, =TextBufferWriter
	mov		lr, r1
	bllr

	mov		r5, r0

	@ Get padding

	mov		r1, r0

	ldr		r0, =MMBAltTextWidth @ multiplied by 8 in EA
	ldrb	r0, [r0]
	ldr		r2, =GetStringTextCenteredPos
	mov		lr, r2
	bllr

	mov		r6, r0

	add		r4, #AltTextStructStart

	mov		r0, r4
	ldr		r1, =TextClear
	mov		lr, r1
	bllr

	mov		r0, r4
	mov		r1, r6
	ldr		r2, =MMBTextAltColor
	ldrh	r2, [r2]
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

	pop		{r4-r6}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBTextAltColor
	@ MMBItemNamePosition
	@ MMBAltTextWidth
