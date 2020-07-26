
.thumb

.include "../CommonDefinitions.inc"

MMBCRTGetter:

	.global	MMBCRTGetter
	.type	MMBCRTGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #BattleUnitCrit
	ldsh	r0, [r0, r1]

	bx		lr

.ltorg
