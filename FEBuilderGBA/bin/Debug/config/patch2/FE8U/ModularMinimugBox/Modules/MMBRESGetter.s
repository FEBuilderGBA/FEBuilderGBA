
.thumb

.include "../Common Definitions.inc"

MMBRESGetter:

	.global	MMBRESGetter
	.type	MMBRESGetter, %function

	push	{r14}

	ldr		r3, =GetRes
	mov		r14, r3
	.short 0xF800

	pop		{r1}
	bx		r1

.ltorg
