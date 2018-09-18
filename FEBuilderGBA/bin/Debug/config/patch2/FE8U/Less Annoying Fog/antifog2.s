.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

strh	r0,[r5,#0x2E]
blh	#0x8026688
blh	#0x8031154
blh	#0x80271A0
blh	#0x8026F94
blh	#0x8026628

@hide active unit sprite if bumped into fog
ldr	r0,=#0x203A958
ldrb	r0,[r0,#0x10]	@moved 0xFF squares if fog slam
cmp	r0,#0xFF
bne	End
ldr	r0,=#0x203A4EC
blh	#0x0802810C	@stan's the man

End:
blh	#0x08056332
.ltorg
.align
