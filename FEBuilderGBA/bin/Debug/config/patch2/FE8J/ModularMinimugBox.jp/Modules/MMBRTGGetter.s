
.thumb

.include "../Common Definitions.inc"

MMBRTGGetter:

	.global	MMBRTGGetter
	.type	MMBRTGGetter, %function

	push	{r4, r5, r14}

	mov		r5, r0 @ unit
	mov		r4, #0x00 @ sum

	ldr		r1, =GetMaxHP
	mov		r0, r5
	bl		Goto_r1
	add		r4, r4, r0
	ldr		r1, =GetStr
	mov		r0, r5
	bl		Goto_r1
	add		r4, r4, r0
	ldr		r1, =GetSkl
	mov		r0, r5
	bl		Goto_r1
	add		r4, r4, r0
	ldr		r1, =GetSpd
	mov		r0, r5
	bl		Goto_r1
	add		r4, r4, r0
	ldr		r1, =GetLuk
	mov		r0, r5
	bl		Goto_r1
	add		r4, r4, r0
	ldr		r1, =GetDef
	mov		r0, r5
	bl		Goto_r1
	add		r4, r4, r0
	ldr		r1, =GetRes
	mov		r0, r5
	bl		Goto_r1
	add		r0, r4, r0
	pop		{r4, r5}
	pop		{r1}

Goto_r1:
	bx		r1

.ltorg
