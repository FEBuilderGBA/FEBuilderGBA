.thumb
.org 0x0

ldrb	r1,[r5,#0x1D]
cmp		r2,r1
beq		NoMovChange
mov		r0,#0x1
NoMovChange:
mov		r1,r6
add		r1,#0x39
ldrb	r1,[r1]
mov		r2,r5
add		r2,#0x39
ldrb	r2,[r2]
cmp		r2,r1
beq		NoMagChange
mov		r0,#0x1
NoMagChange:
ldrb	r6,[r6,#0x1A]
ldrb	r5,[r5,#0x1A]
bx		r14
