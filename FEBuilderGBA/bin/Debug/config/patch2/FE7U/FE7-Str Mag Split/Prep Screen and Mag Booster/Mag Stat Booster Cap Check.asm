.thumb
.org 0x0

mov		r2,r4
add		r2,#0x39
ldrb	r1,[r2]			@mag
ldr		r0,[r4,#0x4]
ldrb	r0,[r0,#0x4]
lsl		r0,#0x2
ldr		r3,MagClassCap
add		r0,r3
ldrb	r0,[r0,#0x2]
cmp		r1,r0
ble		NotOverCap
strb	r0,[r2]
NotOverCap:
mov		r2,#0x1D
ldsb	r2,[r4,r2]
mov		r1,#0x12
ldsb	r1,[r7,r1]
mov		r0,#0xF
sub		r0,r0,r1
cmp		r2,r0
bx		r14

.align
MagClassCap:
