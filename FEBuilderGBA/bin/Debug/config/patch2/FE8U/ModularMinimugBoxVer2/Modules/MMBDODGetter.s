
.thumb

.include "../CommonDefinitions.inc"

MMBDODGetter:

	.global	MMBDODGetter
	.type	MMBDODGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #BattleUnitDodge
	ldsh	r0, [r0, r1]

	bx		lr

.ltorg
