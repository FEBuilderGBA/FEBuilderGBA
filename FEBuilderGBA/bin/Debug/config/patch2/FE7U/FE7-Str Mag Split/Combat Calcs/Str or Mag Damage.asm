.thumb
.org 0x0

@r1=attacker or defender data in battle struct
add		r1,#0x4C
mov		r2,#0x14
ldr		r0,[r1]
mov		r3,#0x2
tst		r3,r0
beq		IsStr
mov		r2,#0x39
IsStr:
ldrb	r2,[r5,r2]
ldrh	r0,[r1,#0xE]	@current damage
add		r0,r0,r2
strh	r0,[r1,#0xE]
bx		r14
