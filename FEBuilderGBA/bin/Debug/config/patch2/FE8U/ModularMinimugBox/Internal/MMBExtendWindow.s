
.thumb

.include "../Common Definitions.inc"

MMBExtendWindow:

	.global	MMBExtendWindow
	.type	MMBExtendWindow, %function

	@ Inputs:
	@ r0: Pointer to Proc state

	push	{r4, r14}

	mov		r4, r0

	@ Clear the last scrolled frame

	ldr		r1, =MMBClearScrolledWindow
	mov		r14, r1
	.short 0xF800

	mov		r2, r1

	ldr		r1, =MMBExtendTable

	mov		r0, r4
	ldr		r3, =MMBSetWindowPosition
	mov		r14, r3
	.short 0xF800

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
	mov		r14, r1
	.short 0xF800

	@ fetch unit

	ldr		r0, =MMBGetUnitAtCursor
	mov		r14, r0
	.short 0xF800

	ldr		r1, =GetDeploymentSlot
	mov		r14, r1
	.short 0xF800

	mov		r1, r0
	mov		r0, r4

	ldr		r2, =MMBBuildDynamics
	mov		r14, r2
	.short 0xF800

End:
	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:

