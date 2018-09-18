.thumb
.org 0

@called at 70A70

push	{r4,r14}
ldrh	r0,[r5,#0xC]
ldr		r3,Find_Anim_Entry
bl		bx_r3
mov		r4,r0
ldrh	r0,[r4,#0xE]		@sound effect during detransformation. 0xDD for Myrrh
mov		r1,#0x80
lsl		r1,#1
ldr		r3,=#0x8071991
bl		bx_r3
mov		r1,#0x2
ldsh	r1,[r5,r1]
ldrh	r0,[r4,#0xE]		@same SE as before
mov		r2,#1
ldr		r3,=#0x8071AB1
bl		bx_r3
ldrh	r1,[r4,#0x6]		@anim id for detransforming (0xC6 for Myrrh)
sub		r1,#1
mov		r0,r5
ldr		r3,=#0x80589E1
bl		bx_r3
mov		r0,r5
mov		r1,#0
ldr		r3,=#0x805A07D
bl		bx_r3
mov		r0,r6
ldr		r3,=#0x8002E95
bl		bx_r3
ldrh	r1,[r4,#0x2]		@initial animation id... I don't quite understand what's going on here
sub		r1,#1
pop		{r4}
pop		{r0}
bx		r0

bx_r3:
bx		r3

.ltorg
Find_Anim_Entry:
@
