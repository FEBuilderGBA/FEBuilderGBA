
.thumb

.include "../Common Definitions.inc"

MMBRedrawTilemap:

	.global	MMBRedrawTilemap
	.type	MMBRedrawTilemap, %function

	.set MMBWidth, EALiterals + 0
	.set MMBHeight, EALiterals + 4

	@ Inputs:
	@ r0: Pointer to Proc state

	push	{r14}

	ldr		r1, =WindowSideTable
	add		r0, #0x50 @ window position type
	ldrb	r0, [r0]
	lsl		r0, r0, #0x18
	asr		r0, r0, #0x18
	lsl		r0, r0, #0x03
	add		r1, r0, r1
	mov		r0, #0x02
	ldsb	r0, [r1, r0] @ -1 left 1 right
	mov		r2, #0x00
	cmp		r0, #0x00
	blt		SkipRightWindow

	ldr		r0, MMBWidth
	mov		r2, #30 + 1
	sub		r2, r2, r0

SkipRightWindow:
	mov		r0, #0x03
	ldsb	r0, [r1, r0] @ -1 top 1 bottom
	mov		r1, #0x00
	cmp		r0, #0x00
	blt		SkipLowerWindow

	mov		r1, #20
	ldr		r0, MMBHeight
	sub		r1, r1, r0

SkipLowerWindow:

	ldr		r0, =CopyTilemapRect
	mov		r14, r0

	ldr		r0, =WindowBuffer
	lsl		r1, r1, #0x05
	add		r1, r1, r2
	lsl		r1, r1, #0x01
	ldr		r2, =BG0Buffer
	add		r1, r1, r2
	ldr		r2, MMBWidth
	ldr		r3, MMBHeight

	.short 0xF800

	mov		r0,#0x3 @ update BG0 and BG1
	ldr		r1, =EnableBackgroundSyncByMask
	mov		r14, r1
	.short 0xF800

	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBWidth
	@ MMBHeight
