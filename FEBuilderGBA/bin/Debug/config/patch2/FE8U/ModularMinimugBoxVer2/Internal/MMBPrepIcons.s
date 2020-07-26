
.thumb

.include "../CommonDefinitions.inc"

MMBPrepIcons:

	.global	MMBPrepIcons
	.type	MMBPrepIcons, %function

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{lr}

	add		r0, r0, #InventoryIconCount
	mov		r1, #0x00
	strb	r1, [r0]

	ldr		r0, =ClearIconRegistry
	mov		lr, r0
	bllr

	pop		{r0}
	bx		r0

.ltorg
