.macro blr reg
	mov lr, \reg
	.short 0xF800
.endm
.macro blh to,reg=r4
	push {\reg}
	ldr \reg,=\to
	mov r14,\reg
	pop {\reg}
	.short 0xF800
.endm

.macro SET_FUNC name, value
	.global \name
	.type   \name, function
	.set    \name, \value
.endm
.macro SET_DATA name, value
	.global \name
	.type   \name, object
	.set    \name, \value
.endm


SET_FUNC GetGameClock, 0x8000D29	@{U}
@SET_FUNC GetGameClock, 0x08000CD9	@{J}

SET_DATA gPalBlueRangeSquare, 0x8A02F34	@{U
@SET_DATA gPalBlueRangeSquare, 0x08A74FEC	@{J}

SET_FUNC CopyToPaletteBuffer, 0x8000DB9	@{U}
@SET_FUNC CopyToPaletteBuffer, 0x08000D69	@{J}

SET_DATA gPalRedRangeSquare, 0x8A02F94	@{U}
@SET_DATA gPalRedRangeSquare, 0x08A7504C	@{J}

SET_DATA gPalGreenRangeSquare, 0x8A02FF4	@{U}
@SET_DATA gPalGreenRangeSquare, 0x08A750AC	@{J}


.macro _blh0 to, reg=r0
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro blh0 to, reg=r0
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro _blh1 to, reg=r1
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro blh1 to, reg=r1
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro _blh2 to, reg=r2
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro blh2 to, reg=r2
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro _blh3 to, reg=r3
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro blh3 to, reg=r3
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro _blh4 to, reg=r4
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro blh4 to, reg=r4
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro _blh5 to, reg=r5
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro blh5 to, reg=r5
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro _blh6 to, reg=r6
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro blh6 to, reg=r6
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro _blh7 to, reg=r7
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro blh7 to, reg=r7
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro _blh8 to, reg=r8
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm
.macro blh8 to, reg=r8
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm

.macro _blh00 to, reg=r4
	push {\reg}
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
	pop {\reg}
.endm

.macro _blr reg
	mov lr, \reg
	.short 0xF800
.endm

.macro _blh to,reg=r4
	push {\reg}
	ldr \reg,=\to
	mov r14,\reg
	pop {\reg}
	.short 0xF800
.endm

.macro SET_FUNC_INLINE name @, value
	.global \name
	.type   \name, function
.endm
.macro SET_DATA_INLINE name @, value
	.global \name
	.type   \name, object
.endm

@ (rd != rox)) MUST be true
.macro _MakePair rd, rs1, rs2, rox=r3
	lsl \rox, \rs1, #16 @ clearing top 16 bits of part 1
	lsl \rd,  \rs2, #16 @ clearing top 16 bits of part 2
	lsr \rox,       #16 @ shifting back part 1
	orr \rd, \rox       @ OR
.endm

.macro _GetPairFirst rd, rs
	lsl \rd, \rs, #16 @ clearing second part of pair
	asr \rd, \rd, #16 @ shifting back
.endm

.macro _GetPairSecond rd, rs
	asr \rd, \rs, #16 @ shifting second part of pair (erasing first part in the process)
.endm

.macro _MakeSign rd, rs, rox=r3
	neg \rox, \rs
    asr \rox, \rox, #31
    asr \rd,  \rs,  #31
    sub \rd,  \rox
.endm

