.thumb
.org 0x0

mov		r6,r14
ldsb	r1,[r4,r1]
ldr		r3,MakeBarFn		@probably not what it actually does, but whatever
mov		r14,r3
.short	0xF800				@writes con
str		r0,[sp,#0x2C]
ldr		r0,MagGetter
mov		r14,r0
mov		r0,r5
.short	0xF800
lsl		r1,r0,#0x1
add		r0,r0,r1
lsl		r0,r0,#0x3
ldr		r1,MakeBarFn
mov		r14,r1
ldr		r2,[r5,#0x4]
ldrb	r2,[r2,#0x4]
lsl		r2,#0x2
ldr		r1,MagGetter+4		@actually Mag Class Table
add		r1,r2
ldrb	r1,[r1,#0x2]
.short	0xF800
str		r0,[sp,#0x14]
ldr		r0,MakeBarFn
mov		r14,r0
ldr		r0,[r5,#0x4]
ldrb	r0,[r0,#0x12]		@mov
mov		r1,#0x1D
ldsb	r1,[r5,r1]			@mov bonus
add		r0,r0,r1
lsl		r1,r0,#0x1
add		r0,r0,r1
lsl		r0,r0,#0x3
mov		r1,#0xF				@mov cap
.short	0xF800
str		r0,[sp,#0x30]
mov		r5,#0x0
mov		r0,r6
add		r6,sp,#0xC
bx		r0

.align
MakeBarFn:
.long 0x080BFC88
MagGetter:
