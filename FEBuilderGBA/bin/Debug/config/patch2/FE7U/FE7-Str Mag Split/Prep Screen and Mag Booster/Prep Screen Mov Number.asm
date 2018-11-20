.thumb
.org 0x0

push	{r14}
mov		r6,r2			@amt to add to r5 for portrait address, I think
ldr		r0,DisplayStatNumber
mov		r14,r0
mov 	r0,#0xC6
lsl		r0,r0,#0x1
add		r0,r0,r5
ldr		r1,[r4,#0x4]
ldrb	r1,[r1,#0x12]	@class mov
mov		r2,#0x1D
ldsb	r2,[r4,r2]		@mov bonus
add		r2,r2,r1
mov		r1,#0x2
cmp		r2,#0xF			@max mov; change this if needed
bne		NotMaxMov
mov		r1,#0x4
NotMaxMov:
.short 	0xF800
ldr		r0,DisplayStatNumber
mov		r14,r0
add		r0,r5,r6
ldrb	r2,[r4,#0x8]
mov		r1,#0x2	
.short	0xF800
pop		{r0}
bx		r0

.align
DisplayStatNumber:
.long 0x080061E4
