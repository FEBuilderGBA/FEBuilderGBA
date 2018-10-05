
.thumb

.include "../Common Definitions.inc"

MMBAVOGetter:

	.global	MMBAVOGetter
	.type	MMBAVOGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #0x62
	ldsh	r0, [r0, r1]

	bx		r14

.ltorg
