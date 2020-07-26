
.thumb

.include "../CommonDefinitions.inc"

MMBInit:

	.global	MMBInit
	.type	MMBInit, %function

	.set MMBTextStruct1Width, EALiterals + 0
	.set MMBTextStruct2Width, EALiterals + 4

	@ Inputs:
	@ r0: Pointer to Proc state

	push	{r4, lr}

	mov		r4, r0

	@ Clear the window side type

	mov		r1, #0xFF
	mov		r0, r4
	add		r0, #WindowSideType
	strb	r1, [r0]

	@ Allocate text space

	mov		r0, r4
	add		r0, #NameTextStructStart
	ldr		r1, MMBTextStruct1Width
	ldr		r3, =TextAllocate
	mov		lr, r3
	bllr

	mov		r0, r4
	add		r0, #AltTextStructStart
	ldr		r1, MMBTextStruct2Width
	ldr		r3, =TextAllocate
	mov		lr, r3
	bllr

	@ Clear framecount

	mov		r1, #0x00
	str		r1, [r4, #Framecount]

	@ Clear retract flag?

	mov		r0, r4
	add		r0, #RetractFlag
	strb	r1, [r0]

	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBTextStruct1Width
	@ MMBTextStruct2Width
