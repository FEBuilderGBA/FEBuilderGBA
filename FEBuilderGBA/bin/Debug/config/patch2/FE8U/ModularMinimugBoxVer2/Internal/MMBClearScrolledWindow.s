
.thumb

.include "../CommonDefinitions.inc"

MMBClearScrolledWindow:

	.global	MMBClearScrolledWindow
	.type	MMBClearScrolledWindow, %function

	.set MMBWidth, EALiterals + 0
	.set MMBHeight, EALiterals + 4

	@ Inputs:
	@ r0: Pointer to Proc state

	@ Outputs:
	@ r0: X coordinate of box
	@ r1: Y coordinate of box

	push	{r4-r7, lr}

	@ Grab window side type

	add		r0, #WindowPosType
	ldrb	r0, [r0]
	lsl		r0, r0, #0x03
	ldr		r1, =WindowSideTable
	add		r1, r0, r1

	mov		r0, #0x03
	ldsb	r0, [r1, r0]

	@ Determine coordinates from type

	mov		r5, #0x00 @ Top window Y coordinate

	cmp		r0, #0x00
	blt		SkipLowerWindow

	ldr		r3, MMBHeight
	mov		r5, #20 @ screen height
	sub		r5, r5, r3 @ Bottom window X coordinate

SkipLowerWindow:

	mov		r0, #0x02
	ldsb	r0, [r1, r0]

	mov r4, #0x00 @ Left window X coordinate

	cmp		r0, #0x00
	blt		SkipRightWindow

	ldr		r3, MMBWidth
	mov		r4, #30
	sub		r4, r4, r3

SkipRightWindow:

	mov		r6, r4
	mov		r7, r5

	@ calculate buffer offset
	@ Tilemap entries are 2 bytes, so mult X by 2
	@ Rows are 0x40 bytes, so mult Y by 0x40

	lsl		r4, r4, #0x02 @ X
	lsl		r5, r5, #0x06 @ Y

	ldr		r0, =BG0Buffer
	add		r0, r0, r5
	add		r0, r0, r4

	@ Clear tilemaps

	ldr		r1, MMBWidth
	ldr		r2, MMBHeight
	mov		r3, #0x00
	ldr		r4, =FillTilemapRect
	mov		lr, r4
	bllr

	ldr		r0, =BG1Buffer
	add		r0, r0, r5
	lsl		r4, r6, #0x02
	add		r0, r0, r4
	ldr		r4, =FillTilemapRect
	mov		lr, r4
	bllr

	@ Set that tilemaps have been edited

	mov		r0, #0x03
	ldr		r1, =EnableBackgroundSyncByMask
	mov		lr, r1
	bllr

	@ Let's return the coords of the window to
	@ make things nice

	mov		r0, r6
	mov		r1, r7

	pop		{r4-r7}
	pop		{r2}
	bx		r2

.ltorg

EALiterals:
	@ MMBWidth
	@ MMBHeight

