.thumb
.org 0x0

@r0,r5,r6 are free, r3 is free but needs to be 0 at the end
ldrb	r0,[r1,#0x4]		@char num
ldr		r3,MagCharTable
lsl		r0,#0x1
add		r0,r3
mov		r3,#0x0
ldsb	r0,[r0,r3]			@char base
ldrb	r5,[r2,#0x4]		@class num
lsl		r5,#0x2
ldr		r3,MagCharTable+4	@MagClassTable
add		r5,r3
mov		r3,#0x0
ldsb	r5,[r5,r3]			@class base
add		r0,r0,r5
mov		r3,r4
add		r3,#0x39
strb	r0,[r3]
mov		r3,#0x0
ldrb	r6,[r1,#0x10]
ldrb	r5,[r2,#0xF]
add		r0,r5,r6
strb	r0,[r4,#0x17]
ldrb	r6,[r1,#0x11]
ldrb	r2,[r2,#0x10]
bx		r14

.align
MagCharTable:
