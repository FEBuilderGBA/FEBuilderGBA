.thumb 

@branched to from 81414

and		r1,r0
ldr		r2,=#0x200310C
sub		r4,r2,#0x1
ldrb	r4,[r4]
ldr		r0,RamLocation
strb	r4,[r0]
mov		r0,#0x3
ldrb	r4,[r2]
and		r0,r4
orr		r1,r0
bx		r14

.ltorg
RamLocation:
@
