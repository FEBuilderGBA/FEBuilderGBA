.thumb
.org 0x0

@r0=iid/uses
lsr		r2,r0,#0x8
cmp		r2,#0x0
bne		GoBack
mov		r2,#0xFF
and		r2,r0
lsl		r0,r2,#0x3
add		r0,r2
lsl		r0,#0x2
ldr		r1,ItemTable
add		r3,r0,r1
ldr		r1,[r3,#0x8]
mov		r0,#0x8			@indestructible
tst		r0,r1
mov		r0,#0x0
bne		Indestructible
ldrb	r0,[r3,#0x14]
Indestructible:
lsl		r0,#0x8
add		r0,r2
GoBack:
bx		r14

.align
ItemTable:
