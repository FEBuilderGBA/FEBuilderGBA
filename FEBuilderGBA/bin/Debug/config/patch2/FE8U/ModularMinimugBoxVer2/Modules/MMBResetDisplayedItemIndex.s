
.thumb

.include "../CommonDefinitions.inc"

MMBResetDisplayedItemIndex:

	.global	MMBResetDisplayedItemIndex
	.type	MMBResetDisplayedItemIndex, %function

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	mov		r1, #0
	mov		r2, #DisplayedItemIndex
	strb	r1, [r0, r2]

	bx		lr

.ltorg

EALiterals:
	@ None
