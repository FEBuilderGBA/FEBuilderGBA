.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

orr	r1,r2
str	r1,[r0,#0xC]

blh	#0x8037A6C
blh	#0x80271A0

@hide active unit sprite if bumped into fog
ldr	r0,=#0x203A958
ldrb	r0,[r0,#0x10]	@moved 0xFF squares if fog slam
cmp	r0,#0xFF
bne	End
ldr	r0,=#0x203A4EC
blh	#0x0802810C	@stan's the man

End:
mov	r0,r5
blh	#0x08032818
.ltorg
.align
