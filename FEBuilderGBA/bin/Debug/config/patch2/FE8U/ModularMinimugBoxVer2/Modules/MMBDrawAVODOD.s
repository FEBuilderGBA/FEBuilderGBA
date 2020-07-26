
.thumb

.include "../CommonDefinitions.inc"

MMBDrawAVODOD:

	.global	MMBDrawAVODOD
	.type	MMBDrawAVODOD, %function

	.set MMBAVODODBGPos,	EALiterals +  0
	.set MMBAVODODX,		EALiterals +  4
	.set MMBAVODODY,		EALiterals +  6
	.set MMBHeight,			EALiterals +  8
	.set MMBAVODODVRAMTile,	EALiterals + 12
	.set AVOGraphic,		EALiterals + 16
	.set DODGraphic,		EALiterals + 20

	@ Inputs:
	@ r0: pointer to proc state
	@ r1: pointer to unit in RAM

	push	{r4-r6, lr}

	mov		r4, r0
	mov		r5, r1

	@ Grab time unit's been selected

	mov		r2, r0

	add		r2, #HoverFramecount
	ldrh	r2, [r2]

	mov		r1, #0x40
	and		r2, r1

	@ default to drawing defense

	ldr		r0, AVOGraphic
	ldr		r6, =MMBAVOGetter

	cmp		r2, #0x00
	beq		DrawDef

	@ Drawing resistance

	ldr		r0, DODGraphic
	ldr		r6, =MMBDODGetter

DrawDef:

	ldr		r2, MMBAVODODVRAMTile
	lsl		r2, r2, #0x14
	lsr		r2, r2, #0x0F @ strip palette, mult by 0x20
	mov		r1, #0x06
	lsl		r1, r1, #0x18 @ 0x06000000
	add		r1, r2, r1
	mov		r2, #0x40

	ldr		r3, =RegisterTileGraphics
	mov		lr, r3

	bllr

	ldr		r0, MMBAVODODBGPos
	ldr		r1, =WindowBuffer
	add		r0, r1, r0
	ldr		r1, MMBAVODODVRAMTile
	strh	r1, [r0]
	add		r0, #0x02
	add		r1, #0x01
	strh	r1, [r0]

	@ check if there's a unit

	mov		r0, r5
	cmp		r0, #0x00
	beq		End

	mov		r0, r4
	add		r0, #UnitFlag

	ldrb	r0, [r0]
	cmp		r0, #0x00
	bne		End

	mov		r0, r5
	mov		lr, r6
	bllr

	mov		r2, r0
	ldr		r1, =MMBAVODODX
	ldrb	r0, [r1]
	ldrb	r1, [r1, #0x02]

	add		r4, #WindowPosType
	ldrb	r4, [r4]
	lsl		r4, r4, #0x03
	ldr		r3, =WindowSideTable
	add		r3, r3, r4
	mov		r4, #0x03
	ldsb	r3, [r3, r4] @ -1 top 1 bottom
	cmp		r3, #0x00
	blt SkipBottom

	ldr		r3, MMBHeight
	mov		r4, #20
	sub		r4, r4, r3
	lsl		r4, r4, #0x03
	add		r1, r4, r1

SkipBottom:

	ldr		r3, =MMBDrawSignedNumber
	mov		lr, r3
	bllr

End:
	pop		{r4-r6}
	pop		{r0}
	bx		r0

.ltorg

EALiterals:
	@ MMBAVODODBGPos
	@ MMBAVODODX
	@ MMBAVODODY
	@ MMBHeight
	@ MMBAVODODVRAMTile
	@ AVOGraphic
	@ DODGraphic
