.thumb
.include 	"_Definitions.h.s"
SET_FUNC_INLINE		_1D9DC_Loop6C_MoveLimitView

@ new_MapMovement: 	0x1 Blue/ 	0x20 Red/ 	0x40 Green
@ new_RangeMap:		0x2 Red/ 	0x4 Green/	0x10 Blue

_1D9DC_Loop6C_MoveLimitView:
	push	{r3-r7, lr}

	mov		r4, r0
	_blh1	GetGameClock|1

	lsr		r5, r0, #0x1
	mov		r0, #0x1F
	and		r5, r0
	
	add		r4, #0x4A
	ldrh	r1,[r4]

.TestRoutine:
	mov		r0, #0x1
	and		r0, r1
	cmp		r0, #0x0
	beq		.NotBit1

	lsl		r0, r5, #0x1
	ldr		r1, =gPalBlueRangeSquare
	add		r0, r0, r1
	mov		r1, #0x82
	mov		r2, #0x20
	_blh3	CopyToPaletteBuffer

.NotBit1:
	ldrh	r1,[r4]
	mov		r0, #0x2
	and		r0, r1
	cmp		r0, #0x0
	beq		.NotBit2
	
	lsl		r0, r5, #0x1
	ldr		r1, =gPalRedRangeSquare
	add		r0, r0, r1
	mov		r1, #0xA2
	mov		r2, #0x20
	_blh3	CopyToPaletteBuffer	
	
.NotBit2:
	ldrh	r1,[r4]
	mov		r0, #0x4
	and		r0, r1
	cmp		r0, #0x0
	beq		.NotBit4

	lsl		r0, r5, #0x1
	ldr		r1, =gPalGreenRangeSquare
	add		r0, r0, r1
	mov		r1, #0xA2
	mov		r2, #0x20
	_blh3	CopyToPaletteBuffer		
	
.NotBit4:
	ldrh	r1,[r4]
	mov		r0, #0x10
	and		r0, r1
	cmp		r0, #0x0
	beq		.NotBit10
	
	lsl		r0, r5, #0x1
	ldr		r1, =gPalBlueRangeSquare
	add		r0, r0, r1
	mov		r1, #0xA2
	mov		r2, #0x20
	_blh3	CopyToPaletteBuffer	
	
.NotBit10:





@-------------------- New Here -------------------
	ldrh	r1,[r4]
	mov		r0, #0x20
	and		r0, r1
	cmp		r0, #0x0
	beq		.NotBit20
	
	lsl		r0, r5, #0x1
	ldr		r1, =gPalRedRangeSquare
	add		r0, r0, r1
	mov		r1, #0x82
	mov		r2, #0x20
	_blh3	CopyToPaletteBuffer
	
.NotBit20:
	ldrh	r1,[r4]
	mov		r0, #0x40
	and		r0, r1
	cmp		r0, #0x0
	beq		.NotBit40
	
	lsl		r0, r5, #0x1
	@ldr		r1, =gPalGreenRangeSquare
	ldr r1, =RangeSquarePurplePalette
	add		r0, r0, r1
	mov		r1, #0x82
	mov		r2, #0x20
	_blh3	CopyToPaletteBuffer
	
.NotBit40:
@-------------------------------------------------





.End:
pop		{r3-r7}
pop		{r0}
bx		r0
