
.thumb

.include "../Common Definitions.inc"

MMBDrawHPBarOrBadStatus:

	.global	MMBDrawHPBarOrBadStatus
	.type	MMBDrawHPBarOrBadStatus, %function

	.set MMBHPBarX,				EALiterals +  0
	.set MMBHPBarY,				EALiterals +  4
	.set MMBHPBarTileWidth,		EALiterals +  8
	.set MMBHPBarBase,			EALiterals + 12

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r6, r14}

	mov		r4, r1


	@ Check for status
	mov		r0, r4
	add		r0, #0x30
	ldrb	r0, [r0]
	mov		r1, #0x0F
	and		r0, r1

	cmp		r0, #0x00
	beq		DrawHPBar

	@ Check if egg, if so don't
	@ display status

	cmp		r0, #0x0A
	beq		DrawHPBar

	@ Otherwise we draw the status

	@ Get buffer position to draw to

	ldr		r0, MMBHPBarX
	ldr		r1, MMBHPBarY
	ldr		r2, =WindowBuffer

	lsl		r0, r0, #0x01
	lsl		r1, r1, #0x06

	add		r0, r1, r0
	add		r0, r0, r2

	mov		r1, r5 @ Character

	@ Draw status image and tilemap

	ldr		r2, =DrawStatus
	mov		r14, r2
	.short 0xF800

	mov		r0, #0x01
	ldr		r1, =EnableBackgroundSyncByMask
	mov		r14, r1
	.short 0xF800

	b		End


DrawHPBar:
	@ Get buffer position to draw to

	ldr		r0, MMBHPBarX
	ldr		r1, MMBHPBarY
	ldr		r2, =WindowBuffer

	lsl		r0, r0, #0x01
	lsl		r1, r1, #0x06

	add		r0, r1, r0
	add		r5, r0, r2

	@ Next get pixel width

	@ Fetch current HP

	mov		r0, r4
	ldr		r1, =GetCurrentHP
	mov		r14, r1
	.short 0xF800

	@ multiply by total number of levels

	ldr		r2, MMBHPBarTileWidth @ (8 * (tiles-2)) + 10
	sub		r2, r2, #0x02
	mov		r1, #0x08
	mul		r2, r2, r1
	add		r2, r2, #0x0A
	mul		r0, r0, r2
	mov		r6, r0

	@ Fetch max HP

	mov		r0, r4
	ldr		r1, =GetMaxHP
	mov		r14, r1
	.short 0xF800

	@ we're getting (curr * width) / max
	@ which gives us the number of filled levels

	mov		r1, r0
	mov		r0, r6

	swi		#0x06 @ div

	mov		r1, r0
	mov		r0, r5
	ldr		r2, MMBHPBarTileWidth

	ldr		r3, MMBHPBarBase

	ldr		r4, =MMBDrawBar
	mov		r14, r4
	.short 0xF800

End:

	pop		{r4-r6}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBHPBarX
	@ MMBHPBarY
	@ MMBHPBarTileWidth
	@ MMBHPBarBase
