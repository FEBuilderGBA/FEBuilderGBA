
.thumb

.include "../CommonDefinitions.inc"

MMBRenderIconObj:

	.global	MMBRenderIconObj
	.type	MMBRenderIconObj, %function

	.set MMBHeight,	EALiterals + 0

	@ Inputs:
	@ r0: X coordinate
	@ r1: Y coordinate
	@ r2: Tile index
	@ r3: pointer to proc state

	push	{r4, lr}

	@ Check if we need to offset the
	@ Y coordinate for the bottom box

	add		r3, #WindowPosType
	ldrb	r3, [r3]
	lsl		r3, r3, #3

	ldr		r4, =WindowSideTable
	add		r3, r3, r4
	mov		r4, #3
	ldsb	r3, [r3, r4]

	@ Negative for top, positive for bottom

	cmp		r3, #0
	blt		Continue

	ldr		r3, MMBHeight
	mov		r4, #20
	sub		r4, r4, r3

	lsl		r4, r4, #3
	add		r1, r1, r4

Continue:

	mov		r3, r2

	ldr		r2, =SpriteData16x16

	ldr		r4, =PushToSecondaryOAM
	mov		lr, r4

	bllr

	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBHeight
