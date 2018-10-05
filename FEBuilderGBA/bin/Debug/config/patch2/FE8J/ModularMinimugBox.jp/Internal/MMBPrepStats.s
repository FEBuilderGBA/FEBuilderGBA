
.thumb

.include "../Common Definitions.inc"

MMBPrepStats:

	.global	MMBPrepStats
	.type	MMBPrepStats, %function

	@ Inputs:
	@ r0: Pointer to proc state
	@ r1: Pointer to unit in RAM

	cmp		r1, #0x00
	bne		Unit

	bx		r14

Unit:

	@ We only need to do this once

	mov		r2, r0
	add		r2, #BattleStructFlag
	ldrb	r3, [r2]
	cmp		r3, #0x00
	beq		WriteStruct

	bx		r14

WriteStruct:

	mov		r3, #0x01
	strb	r3, [r2]

	push	{r4, r14}

	@ save unit

	mov		r4, r1

	mov		r0, r1

	ldr		r1, =GetEquippedWeaponSlot
	mov		r14, r1
	.short 0xF800

	mov		r1, r0
	mov		r0, r4

	ldr		r2, =SetupBattleStructUnitWeapon
	mov		r14, r2
	.short 0xF800

	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ None
