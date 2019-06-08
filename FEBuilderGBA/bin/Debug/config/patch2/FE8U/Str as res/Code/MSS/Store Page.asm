.thumb
.org 0x0

@branched to from 888BC via r3
@saves the byte at 2003BFC-1 to a place in ram, which I use to determine which aspect of a page should be shown. Needs to be saved because this area is used for animations during combat.
and		r1,r0
ldr		r3,StatScreenStruct
sub		r2,r3,#0x1
ldrb	r2,[r2]
ldr		r0,RamLocation
strb	r2,[r0]
ldrb	r2,[r3]
mov		r0,#0x3
and		r0,r2
orr		r1,r0
bx		r14

.align
StatScreenStruct:
.long 0x02003BFC
RamLocation:
