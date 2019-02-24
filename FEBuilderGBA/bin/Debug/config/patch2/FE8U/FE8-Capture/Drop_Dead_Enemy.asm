.thumb
.org 0x0

@set bit 0x1 of byte 1 if unit has 0 hp when being dropped
@jumped to from 183B0
@r2=char data of droppee

strb	r0,[r5,#0x1B]
strb	r0,[r4,#0x1B]
strb	r6,[r4,#0x10]
strb	r7,[r4,#0x11]
ldrb	r0,[r4,#0x13]
cmp		r0,#0x0
bne		End
ldr		r0,[r4,#0xC]
mov		r1,#0x1
orr		r0,r1
str		r0,[r4,#0xC]
End:
pop		{r4-r7}
pop		{r0}
bx		r0
