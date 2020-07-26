
.thumb

.include "../CommonDefinitions.inc"

MMBHITGetter:

	.global	MMBHITGetter
	.type	MMBHITGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #BattleUnitHit
	ldsh	r0, [r0, r1]

	bx		lr

.ltorg
