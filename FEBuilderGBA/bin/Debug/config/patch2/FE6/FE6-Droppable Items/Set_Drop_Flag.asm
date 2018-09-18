.thumb
.org 0

@jumped to from 30AA4
@r0=AI 3/4 we will orr with, r1=pointer to events, r2=char data ptr+0x40

ldrb	r1,[r1,#0xF]
mov		r3,#0xBF		@ negation of 0x40
and		r3,r1
lsl		r3,#0x8
orr		r0,r3
strh	r0,[r2]
mov		r3,#0x40
tst		r1,r3
beq		Label1
mov		r0,#0x40
sub		r0,r2,r0
ldrh	r1,[r0,#0xC]	@status
mov		r2,#0x10
lsl		r2,#0x8
orr		r1,r2
strh	r1,[r0,#0xC]
Label1:
bx		r14
