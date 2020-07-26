
.thumb

.include "../CommonDefinitions.inc"

MMBASGetter:

	.global	MMBASGetter
	.type	MMBASGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #BattleUnitAttackSpeed
	ldsh	r0, [r0, r1]

	bx		lr

.ltorg
