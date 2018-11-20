.thumb
.org 0x0

push	{r14}
ldr		r0,MagGetter
mov		r14,r0
ldr		r0,[r4,#0x2C]
.short 	0xF800
mov		r1,r7
add		r1,#0x2E
strb	r0,[r1]
ldr		r0,[r4,#0x2C]
mov		r1,#0x1D
ldsb	r1,[r0,r1]
ldr		r0,[r0,#0x4]
ldrb	r0,[r0,#0x12]
add		r0,r0,r1
mov		r1,r7
add		r1,#0x35
strb	r0,[r1]
ldr		r0,[r4,#0x2C]
ldr		r1,[r4,#0x30]
lsl		r3,r1,#0x1
mov		r2,r0
add		r2,#0x1E
add		r2,r2,r3
pop		{r5}
bx		r5

.align
MagGetter:
