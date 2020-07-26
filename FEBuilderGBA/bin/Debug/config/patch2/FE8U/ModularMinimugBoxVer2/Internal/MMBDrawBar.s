
.thumb

.include "../CommonDefinitions.inc"

MMBDrawBar:

	.global	MMBDrawBar
	.type	MMBDrawBar, %function

	@ Inputs:
	@ r0: Buffer position to draw to
	@ r1: Pixel width to fill
	@ r2: Bar size in tiles
	@ r3: Base tile

	push	{r4-r6, lr}

	mov		r4, r0
	mov		r5, r1
	mov		r6, r2

	@ Check if the first tile is full

	mov		r0, r5
	cmp		r0, #0x05
	ble		FirstPartial

	@ was full

	mov		r0, #0x05

FirstPartial:

	@ Store first tile

	add		r1, r0, r3
	strh	r1, [r4]

	@ take off how many pixels written
	@ dec tile count

	sub		r5, r5, r0
	sub		r6, r6, #0x01

	@ add to base to get mid tiles

	add		r3, r3, #0x06

	@ Loop through middle tiles

Loop:
	sub		r6, r6, #0x01 @ dec tile count
	add		r4, r4, #0x02 @ next tile in buffer
	cmp		r6, #0x00 @ check if at the cap
	beq		EndLoop
	mov		r0, r5 @ pixel width remaining
	cmp		r0, #0x08
	blt		NotFull

	@ Full tile

	mov		r0, #0x08

NotFull:
	sub		r5, r5, r0
	add		r0, r0, r3
	strh	r0, [r4]
	b		Loop

EndLoop:

	@ Should be 5 or less

	add		r3, r3, #0x09 @ Right cap tiles
	add		r0, r3, r5
	strh	r0, [r4]

	pop		{r4-r6}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:

