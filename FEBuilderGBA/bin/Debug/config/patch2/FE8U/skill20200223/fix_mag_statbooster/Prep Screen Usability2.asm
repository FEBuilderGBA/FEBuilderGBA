.thumb
.org 0x0

ldsb	r0,[r5,r0]
cmp		r0,r1
beq		NoMovChange
mov		r2,#0x1
NoMovChange:
mov		r1,#0x3A
ldsb	r1,[r6,r1]
mov		r0,#0x3A
ldsb	r0,[r5,r0]
cmp		r0,r1
beq		NoMagChange
mov		r2,#0x1
NoMagChange:
mov		r1,#0x1A
ldsb	r1,[r6,r1]
bx		r14
