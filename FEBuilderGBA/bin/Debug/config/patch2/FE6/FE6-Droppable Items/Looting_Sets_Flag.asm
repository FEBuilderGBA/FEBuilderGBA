.thumb
.org 0

@jumped to from 12238

ldr		r0,[sp]
ldrb	r1,[r0,#0xB]
mov		r2,#0xC0
tst		r1,r2
beq		GoBack
ldrh	r1,[r0,#0xC]
mov		r2,#0x10
lsl		r2,#0x8
orr		r1,r2
strh	r1,[r0,#0xC]
GoBack:
add		sp,#0x14
pop		{r7}
pop		{r0}
bx		r0
