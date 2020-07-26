
.thumb

.include "../CommonDefinitions.inc"

MMBCHRGetter:

	.global	MMBCHRGetter
	.type	MMBCHRGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #UnitUnknown47
	ldrb	r0, [r0, r1]
	cmp		r0, #0x10
	bge		HasCharge

	mov		r0, #0xFF
	bx		lr

HasCharge:
	mov		r1, #0x0F
	and		r0, r1

	bx		lr

.ltorg
