
.thumb

.include "../CommonDefinitions.inc"

MMBExtendWindow:

	.global	MMBExtendWindow
	.type	MMBExtendWindow, %function

	@ Inputs:
	@ r0: Pointer to Proc state

	push	{r4, lr}

	mov		r4, r0

	@ Clear the last scrolled frame

	ldr		r1, =MMBClearScrolledWindow
	mov		lr, r1
	bllr

	mov		r2, r1

	ldr		r1, =MMBExtendTable

	mov		r0, r4
	ldr		r3, =MMBSetWindowPosition
	mov		lr, r3
	bllr

	@ get framecount, increment, and check for last frame

	ldr		r0, [r4, #Framecount]
	add		r0, #0x01
	str		r0, [r4, #Framecount]

	cmp		r0, #0x04
	bne		End

	@ If last frame, clear OnCycle
	@ and display dynamic stuff

	mov		r1, r4
	add		r1, #UnitFlag
	mov		r0, #0x00
	strb	r0, [r1] @ clear flag for unit?
	str		r0, [r4, #Framecount] @ clear framecount

	@ clear OnCycle

	mov		r0, r4
	ldr		r1, =ClearProcOnCycle
	mov		lr, r1
	bllr

	@ fetch unit

	ldr		r0, =MMBGetUnitAtCursor
	mov		lr, r0
	bllr

	ldr		r1, =GetDeploymentSlot
	mov		lr, r1
	bllr

	mov		r1, r0
	mov		r0, r4

	ldr		r2, =MMBBuildDynamics
	mov		lr, r2
	bllr

End:
	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:

