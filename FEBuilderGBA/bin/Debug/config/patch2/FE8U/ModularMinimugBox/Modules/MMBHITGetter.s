
.thumb

.include "../Common Definitions.inc"

MMBHITGetter:

	.global	MMBHITGetter
	.type	MMBHITGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #0x60
	ldsh	r0, [r0, r1]

	bx		r14

.ltorg
