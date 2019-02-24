.thumb
.org 0x0

push	{r14}
ldr		r0,MemorySlot
ldr		r2,[r0,#0x14]		@slot 5, the counter
mov		r1,#0x0
cmp		r2,#0x4
bgt		NotZero				@if we've iterated through the the inventory slots, finish
add		r2,#0x1
str		r2,[r0,#0x14]
ldr		r1,[r0,#0x10]
ldrh	r2,[r1]
add		r1,#0x2
ldrh	r3,[r1]
cmp		r3,#0x0
bne		NotZero
mov		r1,#0x0
NotZero:
str		r1,[r0,#0x10]
str		r2,[r0,#0xC]
pop		{r0}
bx		r0

.align
MemorySlot:
.long 0x030004B8
