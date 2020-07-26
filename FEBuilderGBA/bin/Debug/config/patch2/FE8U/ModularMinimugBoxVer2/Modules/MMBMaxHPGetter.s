
.thumb

.include "../CommonDefinitions.inc"

MMBMaxHPGetter:

	.global	MMBMaxGetter
	.type	MMBMaxGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #UnitMaxHP
	ldrb	r0, [r0, r1]

	bx		lr

.ltorg
