.thumb
.org 0x0

@r0=battle struct or char data ptr
ldr		r1,[r0]
add		r1,#32
ldrb	r1,[r1]		@def growth
mov		r2,#14		@index of def boost
ldr		r3,Extra_Growth_Boosts
bx		r3

.align
Extra_Growth_Boosts:
@
