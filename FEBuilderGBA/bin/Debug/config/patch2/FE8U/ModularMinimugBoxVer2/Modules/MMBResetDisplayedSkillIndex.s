
.thumb

.include "../CommonDefinitions.inc"

MMBResetDisplayedSkillIndex:

	.global	MMBResetDisplayedSkillIndex
	.type	MMBResetDisplayedSkillIndex, %function

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	mov		r1, #0
	mov		r2, #DisplayedSkillIndex
	strb	r1, [r0, r2]

	bx		lr

.ltorg

EALiterals:
	@ None
