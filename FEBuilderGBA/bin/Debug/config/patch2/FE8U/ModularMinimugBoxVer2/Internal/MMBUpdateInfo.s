
.thumb

.include "../CommonDefinitions.inc"

MMBUpdateInfo:

	.global	MMBUpdateInfo
	.type	MMBUpdateInfo, %function

	@ Inputs:
	@ r0: Pointer to Proc state

	push	{r4, lr}

	mov		r4, r0

	ldr		r0, =MMBGetUnitAtCursor
	mov		lr, r0
	bllr

	ldr		r1, =GetDeploymentSlot
	mov		lr, r1
	bllr

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
	mov		lr, r2
	bllr

	b		End

	.ltorg

Unit:

	@ Otherwise rebuild stuff on box

	mov		r0, r4
	ldr		r2, =MMBBuildWindow
	mov		lr, r2
	bllr

	mov		r0, r4
	ldr		r1, =MMBRedrawTilemap
	mov		lr, r1
	bllr

End:
	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg
