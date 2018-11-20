.thumb
.org 0x0

push	{r14}
ldrb	r1,[r4,#0x17]
add		r0,r1,r0
strb	r0,[r4,#0x17]
ldr		r0,GetGrowthChance
mov		r14,r0
ldr		r0,[r4,#0x4]
ldrb	r0,[r0,#0x4]		@class number
lsl		r0,#0x2
ldr		r1,MagClassGrowth
add		r0,r1
ldrb	r0,[r0,#0x1]
mov		r1,r5
.short	0xF800
mov		r1,r4
add		r1,#0x39
ldrb	r2,[r1]
add		r2,r0,r2
strb	r2,[r1]
ldr		r0,[r4,#0x4]
add		r0,#0x20
ldrb	r0,[r0]
pop		{r1}
bx		r1

.align
GetGrowthChance:
.long 0x08029604
MagClassGrowth:
