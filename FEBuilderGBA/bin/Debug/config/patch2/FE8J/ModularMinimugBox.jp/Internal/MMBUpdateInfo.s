
.thumb

.include "../Common Definitions.inc"

MMBUpdateInfo:

	.global	MMBUpdateInfo
	.type	MMBUpdateInfo, %function

	@ Inputs:
	@ r0: Pointer to Proc state

	push	{r4, r14}

	mov		r4, r0

	ldr		r0, =MMBGetUnitAtCursor
	mov		r14, r0
	.short 0xF800

	ldr		r1, =GetDeploymentSlot
	mov		r14, r1
	.short 0xF800

	@ Move unit pointer into r1
	@ to pass into BuildUI1Window

	mov		r1, r0

	cmp		r1, #0x00
	bne		Unit

	@ If no unit at space we're moving to

	mov		r0, r4

	@ Jump to retract box

	mov		r1, #0x03
	ldr		r2, =GotoProcLabel
	mov		r14, r2
	.short 0xF800

	b		End

	.ltorg

Unit:

	@ Otherwise rebuild stuff on box

	mov		r0, r4
	ldr		r2, =MMBBuildWindow
	mov		r14, r2
	.short 0xF800

	mov		r0, r4
	ldr		r1, =MMBRedrawTilemap
	mov		r14, r1
	.short 0xF800

End:
	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg
