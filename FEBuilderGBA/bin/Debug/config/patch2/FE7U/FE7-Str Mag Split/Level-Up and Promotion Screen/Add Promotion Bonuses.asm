.thumb
.org 0x0

@r3 has unit's class data ptr, r4 = ram char ptr
ldr		r0,[r4,#0x4]
ldrb	r2,[r0,#0x5]	@new class id
lsl		r2,#0x2
ldr		r1,MagClassTable
add		r2,r1
mov		r1,#0x3
ldsb	r1,[r2,r1]		@mag promo bonus
mov		r0,r4
add		r0,#0x39
ldrb	r7,[r0]			@char mag
add		r7,r1,r7
ldrb	r1,[r2,#0x2]	@mag cap
cmp		r7,r1
ble		NotCapped
mov		r7,r1
NotCapped:
strb	r7,[r0]
mov		r0,r3
add		r0,#0x27
ldrb	r7,[r4,#0x18]
ldrb	r0,[r0]
add		r0,r7,r0
strb	r0,[r4,#0x18]
bx		r14

.align
MagClassTable:
