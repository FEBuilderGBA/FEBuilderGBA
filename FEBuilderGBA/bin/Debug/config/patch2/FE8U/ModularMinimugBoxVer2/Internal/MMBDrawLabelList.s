
.thumb

.include "../CommonDefinitions.inc"

MMBDrawLabelList:

	.global	MMBDrawLabelList
	.type	MMBDrawLabelList, %function

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4, lr}

	@ loop through all available labels

	ldr		r4, =MMBLabelTable

	@ Table entry:
	@ +0 Graphics pointer
	@ +4 Tilemap entry
	@ +6 position short, upper 4 bits are tile count

EntryLoop:
	@ Check for table end first

	ldr		r0, [r4]
	cmp		r0, #0x00
	bne		Continue
	b		End

Continue:

	@ r0 is otherwise graphics ptr

	@ Grab tile and strip palette

	ldrh	r1, [r4, #0x04]
	lsl		r1, r1, #0x16
	lsr		r1, r1, #0x11
	mov		r2, #0x06
	lsl		r2, r2, #0x18 @ 0x06000000
	add		r1, r1, r2

	@ Grab count * 0x20

	ldrb	r2, [r4, #0x07]
	lsr		r2, r2, #0x04
	lsl		r2, r2, #0x05

	ldr		r3, =RegisterTileGraphics
	mov		lr, r3
	bllr

	@ Next write the tilemap

	ldrh	r0, [r4, #0x06] @ position

	lsr		r2, r0, #0x0C @ count
	lsl		r0, r0, #0x14
	lsr		r0, r0, #0x14 @ shift off count

	ldr		r1, =WindowBuffer
	add		r0, r1, r0

	ldrh	r1, [r4, #0x04] @ tilemap entry

TilemapLoop:
	strh	r1, [r0]
	add		r1, r1, #0x01
	add		r0, r0, #0x02
	sub		r2, r2, #0x01
	cmp		r2, #0x00
	bne		TilemapLoop

	@ advance an entry and loop

	add		r4, r4, #0x08
	b		EntryLoop

.ltorg

End:
	pop		{r4}
	pop		{r0}
	bx		r0
