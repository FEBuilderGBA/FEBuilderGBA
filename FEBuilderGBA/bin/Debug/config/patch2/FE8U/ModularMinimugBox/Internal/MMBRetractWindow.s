
.thumb

.include "../Common Definitions.inc"

MMBRetractWindow:

	.global	MMBRetractWindow
	.type	MMBRetractWindow, %function

	@ Inputs:
	@ r0: Pointer to UI1 Proc state

	push	{r4-r7, r14}

	mov		r4, r0

	@ Clear the last scrolled frame

	ldr		r1, =MMBClearScrolledWindow
	mov		r14, r1
	.short 0xF800

	mov		r2, r1

	ldr		r1, =MMBRetractTable

	mov		r0, r4
	ldr		r3, =MMBSetWindowPosition
	mov		r14, r3
	.short 0xF800

	@ get framecount, increment, and check for last frame

	ldr		r0, [r4, #Framecount]
	add		r0, #0x01
	str		r0, [r4, #Framecount]

	cmp		r0, #0x03
	bne		End

	@ If last frame, clear OnCycle

	mov		r1, r4
	add		r1, #RetractFlag
	mov		r0, #0x00
	strb	r0, [r1] @ clear flag for unit?
	str		r0, [r4, #Framecount] @ clear framecount

	add		r1, #0x01
	mov		r0, #0xFF
	strb	r0, [r1]

	@ clear OnCycle

	mov		r0, r4
	ldr		r1, =ClearProcOnCycle
	mov		r14, r1
	.short 0xF800

End:
	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:

