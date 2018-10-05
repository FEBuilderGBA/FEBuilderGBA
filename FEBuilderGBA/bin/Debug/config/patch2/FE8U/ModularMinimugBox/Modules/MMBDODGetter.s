
.thumb

.include "../Common Definitions.inc"

MMBDODGetter:

	.global	MMBDODGetter
	.type	MMBDODGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #0x68
	ldsh	r0, [r0, r1]

	bx		r14

.ltorg
