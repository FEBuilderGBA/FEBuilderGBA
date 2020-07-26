
.thumb

.include "../CommonDefinitions.inc"

MMBPrepStats:

	.global	MMBPrepStats
	.type	MMBPrepStats, %function

	@ Inputs:
	@ r0: Pointer to proc state
	@ r1: Pointer to unit in RAM

	cmp		r1, #0x00
	bne		Unit

	bx		lr

Unit:

	@ We only need to do this once

	mov		r2, r0
	add		r2, #BattleStructFlag
	ldrb	r3, [r2]
	cmp		r3, #0x00
	beq		WriteStruct

	bx		lr

WriteStruct:

	mov		r3, #0x01
	strb	r3, [r2]

	push	{r4, lr}

	@ save unit

	mov		r4, r1

	mov		r0, r1

	ldr		r1, =GetEquippedWeaponSlot
	mov		lr, r1
	bllr

	mov		r1, r0
	mov		r0, r4

	ldr		r2, =SetupBattleStructUnitWeapon
	mov		lr, r2
	bllr

	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ None
