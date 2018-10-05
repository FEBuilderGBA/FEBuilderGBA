
.thumb

.include "../Common Definitions.inc"

MMBCHRGetter:

	.global	MMBCHRGetter
	.type	MMBCHRGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #0x47
	ldrb	r0, [r0, r1]
	cmp		r0, #0x10
	bge		HasCharge

	mov		r0, #0xFF
	bx		r14

HasCharge:
	mov		r1, #0x0F
	and		r0, r1

	bx		r14

.ltorg
