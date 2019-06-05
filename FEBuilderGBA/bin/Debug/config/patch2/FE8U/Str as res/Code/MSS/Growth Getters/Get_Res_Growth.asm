.thumb
.org 0x0

@r0=battle struct or char data ptr
ldr		r1,[r0]
add		r1,#33
ldrb	r1,[r1]		@res growth
mov		r2,#15		@index of res boost
ldr		r3,Extra_Growth_Boosts
bx		r3

.align
Extra_Growth_Boosts:
@
