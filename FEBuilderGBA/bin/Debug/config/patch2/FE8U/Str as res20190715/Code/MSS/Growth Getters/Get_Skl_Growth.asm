.thumb
.org 0x0

@r0=battle struct or char data ptr
ldr		r1,[r0]
ldrb	r1,[r1,#30]	@skl growth
mov		r2,#12		@index of skl boost
ldr		r3,Extra_Growth_Boosts
bx		r3

.align
Extra_Growth_Boosts:
@
