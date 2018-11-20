.thumb
.org 0x0

mov		r0,r5
add		r0,#0x79
ldrb	r1,[r4,#0x19]
ldrb	r0,[r0]
add		r0,r1,r0
strb	r0,[r4,#0x19]
mov		r0,r5
add		r0,#0x7A
mov		r1,r4
add		r1,#0x39
ldrb	r0,[r0]
ldrb	r2,[r1]
add		r0,r0,r2
strb	r0,[r1]
bx		r14
