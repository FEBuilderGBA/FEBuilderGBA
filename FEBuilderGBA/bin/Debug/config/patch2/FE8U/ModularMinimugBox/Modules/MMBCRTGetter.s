
.thumb

.include "../Common Definitions.inc"

MMBCRTGetter:

	.global	MMBCRTGetter
	.type	MMBCRTGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #0x66
	ldsh	r0, [r0, r1]

	bx		r14

.ltorg
