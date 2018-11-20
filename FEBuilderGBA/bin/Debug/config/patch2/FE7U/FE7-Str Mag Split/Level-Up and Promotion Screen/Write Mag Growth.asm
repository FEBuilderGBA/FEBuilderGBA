.thumb
.org 0x0
push	{r14}
add		r6,r0	
ldr		r0,GetGrowth
mov		r14,r0
ldr		r0,[r7]
ldrb	r0,[r0,#0x4]		@char number
lsl		r0,#0x1
ldr		r1,MagCharTable
add		r0,r1
ldrb	r0,[r0,#0x1]		@mag char growth
add		r0,r10
.short	0xF800				@returns amount of points to add to mag
push	{r0}
ldr		r0,GetCharData
mov		r14,r0
ldrb	r0,[r7,#0xB]
.short	0xF800
ldr		r1,[r0,#0x4]
add		r0,#0x39
ldrb	r0,[r0]
ldrb	r1,[r1,#0x4]		@class num
lsl		r1,#0x2
ldr		r2,MagCharTable+4	@actually MagClassTable
add		r2,r1
ldrb	r2,[r2,#0x2]
pop		{r1}
add		r6,r1
add		r3,r0,r1
cmp		r3,r2
ble		NotCapped
sub		r1,r2,r0
NotCapped:
mov		r0,r7
add		r0,#0x7A			@formerly con increase, but unused
strb	r1,[r0]
pop		{r0}
ldr		r1,[sp]
str		r1,[sp,#0xC]
ldr		r3,[sp,#0x4]
str		r3,[sp,#0x8]		@don't ask; I think this is stupid, too
mov		r10,r8
bx		r0

.align
GetGrowth:
.long 0x080295E0
GetCharData:
.long 0x08018D0C
MagCharTable:
