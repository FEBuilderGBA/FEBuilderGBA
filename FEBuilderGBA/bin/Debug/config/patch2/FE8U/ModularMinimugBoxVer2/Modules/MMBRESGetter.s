
.thumb

.include "../CommonDefinitions.inc"

MMBRESGetter:

	.global	MMBRESGetter
	.type	MMBRESGetter, %function

	push	{lr}

	ldr		r3, =GetRes
	mov		lr, r3
	bllr

	pop		{r1}
	bx		r1

.ltorg
