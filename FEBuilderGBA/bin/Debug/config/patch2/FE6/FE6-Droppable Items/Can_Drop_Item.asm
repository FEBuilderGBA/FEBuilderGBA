.thumb
.org 0

@r0=char data
@this function exists to make my life easier on the off chance that byte 2, 0x10 in the status halfword is actually used for something

ldrh	r0,[r0,#0xC]
mov		r1,#0x80
lsl		r1,#5
and		r0,r1
bx		r14
