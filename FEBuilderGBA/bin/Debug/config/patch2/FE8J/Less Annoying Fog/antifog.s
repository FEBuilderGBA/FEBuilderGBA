.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
@this makes the unit action end if B is pressed
@ ldr	r0,=#0x202BCB0	@FE8U
ldr		r0,=#0x202BCAC	@FE8J
add	r0,#0x3D
mov	r1,#1
strb	r1,[r0]

@set unit status to fog slam (original instructions)
mov	r0,#0x0A
strb	r0,[r5]
mov	r0,#0x04
strb	r0,[r5,#0x01]
mov	r0,#0x1E
strb	r0,[r6,#0x11]

@no mov left
mov	r0,#0xFF
strb	r0,[r6,#0x10]

@move unit and update fog
@ldr	r0,=#0x020257B0	@FE8U
ldr		r0,=#0x020257AC	@FE8J	maybe
@ blh	#0x08019CBC	@FE8U
blh		#0x08019994	@FE8J
@ ldr	r1,=#0x0203A958	@FE8U
ldr		r1,=#0x0203A954	@FE8J
ldrb	r0,[r1,#0x0E]
ldrb	r1,[r1,#0x0F]
@ blh	#0x08018740	@FE8U
blh		#0x08018454	@FE8J
@ blh	#0x0801A1F4	@FE8U
blh		#0x08019ECC	@FE8J
@ blh	#0x08019C3C	@FE8U
blh		#0x08019914	@FE8J
mov	r0,#0
@ blh	#0x0801DDC4	@FE8U
blh		#0x0801DA28	@FE8J


@ blh	#0x080271A0	@FE8U @draw sprites
blh		#0x08027144	@FE8J @draw sprites
mov	r0,r7
@ blh	#0x0802810C	@FE8U @hide current unit sprite, thanks stan
blh		#0x080280A0	@FE8J @hide current unit sprite, thanks stan

End:
@ ldr	r1,=#0x0801A8D4	@FE8U
ldr		r1,=#0x0801A5B0	@FE8J
mov	lr,r1
.short	0xF800
.ltorg
.align
