.thumb

.org 0x7FD28
WriteStatNumber:

.org 0x7FE10
.short	0x4E8F				@ldr r6,StatMenuPtr
ldr		r0,[r6,#0xC]
bl		MagGetter
str		r0,[sp]
ldr		r0,[r6,#0xC]
mov		r3,#0x39
ldsb	r3,[r0,r3]
ldr		r0,[r0,#0x4]
ldrb	r0,[r0,#0x4]		@class number
lsl		r0,#0x2
ldr		r1,MagClassTable	@cap
add		r0,r1
ldrb	r0,[r0,#0x2]
str		r0,[sp,#0x4]
mov		r0,#0x1
mov		r1,#0x5
mov		r2,#0x3
bl		WriteStatNumber
b		GetStrength

.align
MagClassTable:

.org 0x7FE78
GetStrength:

.org 0x2E5CB8
MagGetter:


