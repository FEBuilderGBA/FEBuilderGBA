
.thumb

.include "../Common Definitions.inc"

MMBBuildWindow:

	.global	MMBBuildWindow
	.type	MMBBuildWindow, %function

	.set FillSize, EALiterals + 0

	@ Inputs:
	@ r0: Pointer to Proc State
	@ r1: Pointer to unit in RAM

	push	{r4-r6, r14}

	mov		r4, r0
	mov		r5, r1

	@ Clear buffer via CPUFastSet

	mov		r0, #00 @ fill by word
	push	{r0}
	ldr		r1, =WindowBuffer
	ldr		r2, FillSize
	mov		r0, sp
	swi		#0x0C
	pop		{r0}

	@ Clear hover framecount

	mov		r1, r4
	add		r1, #HoverFramecount
	str		r0, [r1]

	@ Clear flag for battle struct

	mov		r1, r4
	add		r1, #BattleStructFlag
	str		r0, [r1]

	@ Loop through all build routines

	ldr		r6, =MMBBuildRoutines

Loop:
	ldr		r0, [r6]
	cmp		r0, #0x00
	beq		End
	mov		lr, r0
	mov		r0, r4
	mov		r1, r5
	.short 0xF800
	add		r6, r6, #0x04

	b		Loop

End:

	mov		r0, r4
	mov		r1, r5
	ldr		r2, =MMBBuildDynamics
	mov		r14, r2
	.short 0xF800

	pop		{r4-r6}
	pop		{r0}
	bx		r0

	.ltorg

	EALiterals:
		@ FillSize


