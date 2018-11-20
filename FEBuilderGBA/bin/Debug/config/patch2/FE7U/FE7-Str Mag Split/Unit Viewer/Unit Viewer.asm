.thumb
.org 0x0

push	{r14}
ldr		r6,[r4]
ldr		r6,[r6]		@char data
ldr		r0,MagGetter
mov		r14,r0
mov		r0,r6
.short	0xF800
ldr		r1,[r6,#0x4]
ldrb	r1,[r1,#0x4]
lsl		r1,#0x2
ldr		r2,MagGetter+4		@actually MagClassTable
add		r2,r1
ldrb	r2,[r2,#0x2]
mov		r1,#0x2
cmp		r0,r2
bne		NoGlowyMag
mov		r1,#0x4
NoGlowyMag:
mov		r2,r0
ldr		r0,ProcessMag
mov		r14,r0
mov		r0,r5
add		r0,#0x18
.short	0xF800
pop		{r0}
bx		r0

.align
ProcessMag:
.long 0x080061E4
MagGetter:
