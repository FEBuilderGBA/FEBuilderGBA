
.thumb

.include "../Common Definitions.inc"

MMBDrawAffinity:

	.global	MMBDrawAffinity
	.type	MMBDrawAffinity, %function

	.set MMBAffinityTile,	EALiterals + 0
	.set MMBAffinityX,		EALiterals + 4
	.set MMBAffinityY,		EALiterals + 5

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4, r14}

	@ save proc state, clear affinity flag

	mov		r4, r0
	add		r4, #AffinityFlag

	mov		r0, #0x00
	strb	r0, [r4]

	@ get the unit's affinity

	mov		r0, r1
	ldr		r1, =GetAffinity
	mov		r14, r1
	.short 0xF800

	@ end if unit doesn't have an affinity

	cmp		r0, #0x00
	blt		End

	@ register icon

	ldr		r1, MMBAffinityTile

	ldr		r2, =RegisterIconOBJ
	mov		r14, r2
	.short 0xF800

	@ set affinity flag

	mov		r0, #0x01
	strb	r0, [r4]

	@ set affinity coordinates

	ldr		r0, =MMBAffinityX
	ldrb	r1, [r0]
	ldrb	r2, [r0, #0x01]
	strb	r1, [r4, #0x01]
	strb	r2, [r4, #0x02]

	@ set affinity tile

	add		r4, #0x03
	ldr		r1, MMBAffinityTile
	mov		r0, #0x05
	lsl		r0, r0, #0x0C
	add		r1, r1, r0
	strh	r1, [r4]

	@ draw palette

	ldr		r0, =0x08599714
	mov		r1, #0x15
	lsl		r1, r1, #0x05
	mov		r2, #0x20
	ldr		r3, =CopyToPaletteBuffer
	mov		r14, r3
	.short 0xF800

End:
	pop		{r4}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBAffinityTile
	@ MMBAffinityX
	@ MMBAffinityY
