
.thumb

.include "../Common Definitions.inc"

MMBPrepIcons:

	.global	MMBPrepIcons
	.type	MMBPrepIcons, %function

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r14}

	add		r0, r0, #OAMCount
	mov		r1, #0x00
	strb	r1, [r0]

	ldr		r0, =ClearIconRegistry
	mov		r14, r0
	.short 0xF800

	pop		{r0}
	bx		r0

.ltorg
