.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

orr	r1,r2
str	r1,[r0,#0xC]

@ blh	#0x8037A6C	@FE8U
blh		#0x8037B04	@FE8J
@ blh	#0x80271A0	@FE8U
blh		#0x8027144	@FE8J

@hide active unit sprite if bumped into fog
@ ldr	r0,=#0x203A958	@FE8U
ldr		r0,=#0x203A954	@FE8J

ldrb	r0,[r0,#0x10]	@moved 0xFF squares if fog slam
cmp	r0,#0xFF
bne	End

@ ldr	r0,=#0x203A4EC	@FE8U
ldr		r0,=#0x203A4E8	@FE8J
@ blh	#0x0802810C	@FE8U	@stan's the man
blh	#0x080280A0		@FE8J	@stan's the man

End:
mov	r0,r5
@ blh	#0x08032818	@FE8U
blh		#0x08032764	@FE8J
.ltorg
.align
