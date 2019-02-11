
.thumb

.include "../Common Definitions.inc"

MMBDrawWeaponNameCenteredWarningColor:

	.global	MMBDrawWeaponNameCenteredWarningColor
	.type	MMBDrawWeaponNameCenteredWarningColor, %function

	.set MMBAltTextWidth	,	EALiterals + 0
	.set MMBTextAltColor,		EALiterals + 2
	.set MMBItemNamePosition,	EALiterals + 4
	.set MMBTextWarningParsent,	EALiterals + 8
	.set MMBTextWarningColor,	EALiterals + 10

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r7, r14}

	mov		r4, r0

	@ Check if unit has an equipped weapon

	mov		r0, r1
	ldr		r1, =GetEquippedWeapon
	mov		r14, r1
	.short 0xF800

	@ if not, end

	cmp		r0, #0x00
	beq		End

	@ save Weapon Count
	mov		r1, r0
	lsr		r1, #0x08
	mov		r5, r1

	mov		r1, #0xFF
	and		r0, r1

	ldr		r1, =GetROMItemStructPtr
	mov		r14, r1
	.short 0xF800

	mov		r6, r0

	@ get color to r5

	ldrb	r0, [r6,#0x8]  @Item->Attribute1 & INF
	mov     r2, #0x8
	and     r0, r2
	cmp     r0, #0x00
	bne     NormalColor

	ldrb	r1, [r6,#0x14]  @Item->MaxCount
	cmp		r1, #0x00
	beq		NormalColor

	mov		r0, r5
	mov		r2, #100
	mul		r0, r2
	swi		0x6     @ div

	@ Switch the color with the rest of the weapon.

	ldr		r1, =MMBTextWarningParsent
	ldrh	r1, [r1]
	cmp		r0,r1
	ble		WarningColor

NormalColor:
	ldr		r2, =MMBTextAltColor
	ldrh	r5, [r2]
	b       ColorJoin

WarningColor:
	ldr		r2, =MMBTextWarningColor
	ldrh	r5, [r2]
@	b       ColorJoin

ColorJoin:

	@ get item name

	mov		r0, r6
	ldrh	r0, [r0]

	ldr		r1, =TextBufferWriter
	mov		r14, r1
	.short 0xF800

	@ save resulting width for later

	mov		r6, r0
	mov		r1, r0

	ldr		r0, =MMBAltTextWidth 
	ldrh	r0, [r0]             @ multiplied by 8 in EA
	ldr		r2, =GetStringTextCenteredPos
	mov		r14, r2
	.short 0xF800

	@ save resulting padding distance

	mov		r7, r0

	@ write item name

	add		r4, #AltTextStructStart
	mov		r0, r4
	ldr		r1, =TextClear
	mov		r14, r1
	.short 0xF800

	@ we write the text info to the proc state

	mov		r0, r4
	mov		r1, r7
	mov		r2, r5  @ Switch the color with the rest of the weapon.

	ldr		r3, =TextSetParameters
	mov		r14, r3
	.short 0xF800

	@ Write name

	mov		r0, r4
	mov		r1, r6

	ldr		r2, =TextAppendString
	mov		r14, r2
	.short 0xF800

	@ write tilemap

	mov		r0, r4
	ldr		r1, =WindowBuffer
	ldr		r2, MMBItemNamePosition
	add		r1, r1, r2

	ldr		r2, =TextDraw
	mov		r14, r2
	.short 0xF800

End:

	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBAltTextWidth
	@ MMBTextAltColor
	@ MMBItemNamePosition
	@ MMBTextWarningColor
	@ MMBTextWarningParsent
