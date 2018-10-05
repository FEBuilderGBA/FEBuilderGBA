
.thumb

.include "../Common Definitions.inc"

MMBDrawSkills:

	.global	MMBDrawSkills
	.type	MMBDrawSkills, %function

	.set MMBInventoryTileIndex,	EALiterals + 0

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r7, r14}

	mov		r4, r0
	mov		r5, r1

	@ Draw the item icon palette to oam palette 4

	ldr		r0, =0x085996F4
	mov		r1, #0x14
	lsl		r1, r1, #0x05
	mov		r2, #0x20
	ldr		r3, =CopyToPaletteBuffer
	mov		r14, r3
	.short 0xF800

	@ fetch skills

	mov		r0, r5
	ldr		r1, =Skill_Getter
	mov		r14, r1
	.short 0xF800

	@ loop counter

	mov		r6, #0x00

	@ pointer to skills

	mov		r7, r0

Loop:

	@ get skill

	ldrb	r0, [r7, r6]

	@ check if we're done

	cmp		r0, #0x00
	beq		End

	mov		r1, #0x01
	lsl		r1, r1, #0x08 @ 0x100
	orr		r0, r0, r1

	@ get tile index

	ldr		r1, MMBInventoryTileIndex
	lsl		r2, r6, #0x01
	add		r1, r1, r2

	@ draw

	ldr		r2, =RegisterIconOBJ
	mov		r14, r2
	.short 0xF800

	add		r6, r6, #0x01
	b		Loop

End:

	@ Store count

	mov		r0, r4
	add		r0, #OAMCount2
	strb	r6, [r0]

	add		r4, #DisplayedIndex
	mov		r0, #0x00
	strb	r0, [r4]

	pop		{r4-r7}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBInventoryTileIndex
