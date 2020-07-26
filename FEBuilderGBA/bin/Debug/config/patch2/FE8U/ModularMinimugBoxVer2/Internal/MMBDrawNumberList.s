
.thumb

.include "../CommonDefinitions.inc"

MMBDrawNumberList:

	.global	MMBDrawNumberList
	.type	MMBDrawNumberList, %function

	.set MMBHeight,	EALiterals + 0

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	@ first check for unit and also
	@ battle struct status

	cmp		r1, #0x00
	bne		UnitPOIN

	bx lr

UnitPOIN:
	mov		r2, r0
	add		r2, #UnitFlag

	ldrb	r2, [r2]
	cmp		r2, #0x00
	beq		Unit

	@ else exit

	bx		lr

Unit:
	mov		r2, r0
	add		r2, #BattleStructFlag
	ldrb	r2, [r2]
	cmp		r2, #0x00
	bne		Start

	bx		lr

Start:

	push	{r4-r6, lr}

	mov		r6, r1

	@ We'll use r5 to keep track of window position
	@ for the purpose of vertically offsetting the 
	@ numbers.

	mov		r5, #0x00

	add		r0, #WindowPosType
	ldrb	r0, [r0]
	lsl		r0, r0, #0x03
	ldr		r1, =WindowSideTable
	add		r0, r1, r0
	mov		r1, #0x03
	ldsb	r0, [r0, r1] @ -1 top 1 bottom
	cmp		r0, #0x00
	blt		SkipBottom

	ldr		r0, MMBHeight
	mov		r5, #20
	sub		r5, r5, r0
	lsl		r5, r5, #0x03

SkipBottom:

	@ loop through all available numbers

	ldr		r4, =MMBNumberTable

	@ Table entry:
	@ +0 Number getter pointer
	@ +4 X coordinate
	@ +6 Y coordinate

	@ Getter inputs:
	@ r0: pointer to unit

	@ Getter outputs:
	@ r0: number

NumberLoop:
	ldr		r1, [r4]

	@ check for table end

	cmp		r1, #0x00
	bne		Continue
	b		End

Continue:

	mov		r0, r6 @ unit

	@ Run the getter

	mov		lr, r1
	bllr

	@ draw the number

	mov		r2, r0

	ldrh	r0, [r4, #0x04]
	ldrh	r1, [r4, #0x06]

	add		r1, r1, r5 @ add vertical offset

	@ Let's assume that the upper bit will only
	@ be set if the number's negative

	ldr		r3, =MMBDrawUnsignedNumber
	mov		lr, r3

	bllr

	@ inc and loop

	add		r4, #0x08
	b		NumberLoop

End:

	pop		{r4-r6}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBHeight


