.thumb

@called at 814AC

ldr		r1,RamLocation
ldrb	r1,[r1]
sub		r7,r2,#1
strb	r1,[r7]
mov		r1,#3
ldrb	r7,[r4,#0x14]
and		r1,r7
strb	r1,[r2]
str		r0,[r2,#0xC]
str		r3,[r2,#0x14]
bx		r14

.align
RamLocation:
@
