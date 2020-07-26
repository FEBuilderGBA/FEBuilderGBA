
.thumb

.include "../CommonDefinitions.inc"

MMBAVOGetter:

	.global	MMBAVOGetter
	.type	MMBAVOGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #BattleUnitAvoid
	ldsh	r0, [r0, r1]

	bx		lr

.ltorg
