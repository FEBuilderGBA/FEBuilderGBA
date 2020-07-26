
.thumb

.include "../CommonDefinitions.inc"

MMBCheckCursor:

	.global	MMBCheckCursor
	.type	MMBCheckCursor, %function

	@ Inputs:
	@ r0: Pointer to UI1 Proc State

	push	{r4-r7, lr}

	mov		r4, r0

	@ Increment time held on unit

	add		r0, #HoverFramecount
	ldrh	r1, [r0]
	add		r1, #0x01
	strh	r1, [r0]

	@ save this for a moment

	push	{r1}

	@ Grab unit

	ldr		r0, =MMBGetUnitAtCursor
	mov		lr, r0
	bllr

	@ save x, y

	mov		r6, r1
	mov		r7, r2

	ldr		r1, =GetDeploymentSlot
	mov		lr, r1
	bllr

	@ save unit

	mov		r5, r0

	@ Do the dynamic stuff

	mov		r0, r4
	mov		r1, r5

	ldr		r2, =MMBBuildDynamics
	mov		lr, r2
	bllr

	@ Check if we need to redraw

	pop		{r0}
	mov		r1, #0x3F
	and		r1, r0
	cmp		r1, #0x00
	bne		NoRedraw

	mov		r0, r4
	ldr		r1, =MMBRedrawTilemap
	mov		lr, r1
	bllr

NoRedraw:

	@ Move last coords around proc state
	@ and save the new ones

	mov		r0, r4
	add		r0, #CursorX
	mov		r1, r4
	add		r1, #CursorXOld
	push	{r0, r1}
	ldrb	r2, [r0]
	strb	r2, [r1]

	strb	r6, [r0]

	add		r0, #0x01
	add		r1, #0x01

	ldrb	r2, [r0]
	strb	r2, [r1]

	strb	r7, [r0]

	@ compare new and old coords

	pop		{r0, r1}

	ldrh	r0, [r0]
	ldrh	r1, [r1]

	cmp		r0, r1
	beq		End

	@ see if we've moved off of a unit

	cmp		r5, #0x00
	beq		KillOnCycle

	@ check for GENS

	ldr		r0, =ProcGENS
	ldr		r1, =FindProc
	mov		lr, r1
	bllr

	cmp		r0, #0x00
	bne		KillOnCycle

	@ Next, check if we need to move the box

	ldr		r0, =WindowPosCheck
	mov		lr, r0
	bllr

	mov		r2, r0
	mov		r0, r4

	add		r0, #WindowPosType
	mov		r3, #0x00
	ldsb	r3, [r0, r3]
	cmp		r2, r3
	beq		SamePosition

	@ just to be sure that sides are different

	ldr		r1, =WindowSideTable

	lsl		r0, r2, #0x03
	add		r5, r0, r1

	lsl		r0, r3, #0x03
	add		r6, r0, r1

	mov		r0, #0x02
	ldsb	r1, [r5, r0]
	ldsb	r0, [r6, r0]

	cmp		r1, r0
	bne		KillOnCycle

	mov		r0, #0x03
	ldsb	r1, [r5, r0]
	ldsb	r0, [r6, r0]

	cmp		r1, r0
	bne		KillOnCycle

SamePosition:

	@ Update info with new unit

	mov		r0, r4
	mov		r1, #0x01
	ldr		r2, =GotoProcLabel
	mov		lr, r2
	bllr
	b		End

.ltorg

KillOnCycle:

	@ murder this thing

	mov		r0, r4
	add		r4, #RetractFlag
	mov		r1, #0x01
	strb	r1, [r4]
	ldr		r1, =ClearProcOnCycle
	mov		lr, r1
	bllr

End:
	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg




