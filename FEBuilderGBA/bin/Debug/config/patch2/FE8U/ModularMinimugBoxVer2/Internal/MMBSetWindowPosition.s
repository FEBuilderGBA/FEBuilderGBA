
.thumb

.include "../CommonDefinitions.inc"

MMBSetWindowPosition:

	.global	MMBSetWindowPosition
	.type	MMBSetWindowPosition, %function

	.set MMBWidth, EALiterals + 0
	.set MMBHeight, EALiterals + 4

	@ Inputs:
	@ r0: Pointer to Proc state
	@ r1: Pointer to extend/retract table
	@ r2: Y coordinate of box

	push	{r4-r7, lr}

	mov		r4, r0

	lsl		r5, r2, #0x06 @ Y coord * 0x40
	lsl		r6, r2, #0x05 @ Y coord * 0x20

	@ Grab framecount to see how far to scroll

	ldr		r0, [r4, #Framecount]

	@ Fetch extension amount

	add		r0, r0, r1
	ldrb	r7, [r0]

	@ Check where the window's at

	mov		r0, r4
	add		r0, #WindowPosType
	ldrb	r0, [r0]
	lsl		r0, r0, #0x03
	ldr		r1, =WindowSideTable
	add		r0, r0, r1
	ldrb	r0, [r0, #0x02]

	lsl		r0, r0, #0x18
	asr		r0, r0, #0x18

	cmp		r0, #0x00
	bge		RightWindow

	@ Window on the left side

	ldr		r3, MMBWidth
	sub		r3, r3, r7
	lsl		r3, r3, #0x01

	ldr		r0, =WindowBuffer
	add		r0, r0, r3 @ source

	ldr		r1, =BG0Buffer
	add		r1, r5, r1 @ dest

	mov		r2, r7 @ visible width

	ldr		r3, MMBHeight

	ldr		r4, =CopyTilemapRect
	mov		lr, r4
	bllr

	@ Again for BG1

	ldr		r3, MMBWidth
	sub		r3, r3, r7
	lsl		r3, r3, #0x01

	ldr		r0, =WindowBufferBG1
	add		r0, r0, r3 @ source

	ldr		r1, =BG1Buffer
	add		r1, r5, r1 @ dest

	mov		r2, r7 @ visible width

	ldr		r3, MMBHeight

	ldr		r4, =CopyTilemapRect
	mov		lr, r4
	bllr

	b		End

.ltorg

RightWindow:

	ldr		r0, =WindowBuffer

	mov		r4, r6
	add		r4, #30 @ screen width

	sub		r4, r4, r7
	lsl		r4, r4, #0x01

	ldr		r1, =BG0Buffer
	add		r1, r4, r1

	mov		r2, r7 @ visible width

	ldr		r3, MMBHeight

	ldr		r5, =CopyTilemapRect
	mov		r5, lr
	bllr

	@ Again for BG1

	ldr		r0, =WindowBufferBG1

	ldr		r1, =BG1Buffer
	add		r1, r4, r1

	mov		r2, r7 @ visible width

	ldr		r3, MMBHeight

	ldr		r5, =CopyTilemapRect
	mov		lr, r5
	bllr

End:
	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBWidth
	@ MMBHeight
