.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

strh	r0,[r5,#0x2E]
@ blh	#0x8026688	@FE8U
blh		#0x802662C	@FE8J
@ blh	#0x8031154	@FE8U
blh		#0x80310A0	@FE8J
@ blh	#0x80271A0	@FE8U
blh		#0x8027144	@FE8J
@ blh	#0x8026F94	@FE8U
blh		#0x8026F38	@FE8J
@ blh	#0x8026628	@FE8U
blh		#0x80265CC	@FE8J

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
@ blh	#0x08056332	@FE8U
blh		#0x080572BA	@FE8J
.ltorg
.align
