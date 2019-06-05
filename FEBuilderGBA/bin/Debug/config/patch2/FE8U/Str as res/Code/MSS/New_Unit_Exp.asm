.thumb
.org 0x0

@bl'd to from 17E98
@r4=char data

push	{r14}
mov		r3,#0xFF
ldrb	r0,[r4,#0xB]
mov		r1,#0xC0
tst		r0,r1
bne		GoBack
ldr		r0,[r4,#0x4]
ldrb	r0,[r0,#0x4]		@class number
ldr		r1,Class_Level_Cap_Table
ldrb	r1,[r1,r0]
ldrb	r0,[r4,#0x8]		@level
cmp		r0,r1
bge		GoBack
mov		r3,#0x0
GoBack:
mov		r0,r3
pop		{r1}
bx		r1

.align
Class_Level_Cap_Table:
@
