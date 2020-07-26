
.thumb

.include "../CommonDefinitions.inc"

MMBCurrentHPGetter:

	.global	MMBCurrentHPGetter
	.type	MMBCurrentHPGetter, %function

	ldr		r0, =BattleBufAttacker
	mov		r1, #UnitCurrentHP
	ldrb	r0, [r0, r1]

	bx		lr

.ltorg
