.thumb
.org 0x0

@branched to from 8895C via r1
@loads the byte to store at 2003BFC-1 from a place in ram, which I use to determine which aspect of a page should be shown.
ldrb	r4,[r5,#0x14]		@stat screen page
mov		r1,#0x3
and		r1,r4
strb	r1,[r2]
str		r0,[r2,#0xC]
sub		r3,r2,#0x1
ldr		r1,RamLocation
ldrb	r1,[r1]
strb	r1,[r3]
mov		r3,#0x0
str		r3,[r2,#0x14]
bx		r14

.align
RamLocation:
