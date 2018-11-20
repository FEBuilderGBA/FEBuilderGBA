.thumb
.org 0x0

push	{r14}
ldrb	r1,[r4,#0x18]
add		r0,r0,r1
strb	r0,[r4,#0x18]
ldr		r0,GetGrowthChance
mov		r14,r0
ldr		r0,[r4]
ldrb	r0,[r0,#0x4]		@char number
lsl		r0,#0x1
ldr		r1,MagCharTable
add		r0,r1
ldrb	r0,[r0,#0x1]
.short	0xF800
mov		r1,r4
add		r1,#0x39
ldrb	r2,[r1]
add		r2,r0,r2
strb	r2,[r1]
ldr		r0,GetGrowthChance
mov		r14,r0
ldr		r0,[r4]
add		r0,#0x22
ldrb	r0,[r0]
.short	0xF800
pop		{r1}
bx		r1

.align
GetGrowthChance:
.long 0x080295E0
MagCharTable:
