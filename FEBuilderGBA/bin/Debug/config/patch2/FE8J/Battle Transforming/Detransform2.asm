.thumb
.org 0

@called at 70AF8	@FE8U
@called at 730A0	@FE8J

push	{r14}
ldrh	r0,[r4,#0xC]
ldr		r3,Find_Anim_Entry
bl		bx_r3
ldrh	r1,[r0,#0x8]		@final animation ID (Myrrh has 0xC7)
sub		r1,#1
mov		r0,r4
@ldr	r3,=#0x80589E1		@FE8U
ldr		r3,=#0x8059811		@FE8J
bl		bx_r3
ldr		r0,[r5,#0x5C]
mov		r1,#8
@ldr	r3,=#0x806FA55		@FE8U
ldr		r3,=#0x8072005		@FE8J
bl		bx_r3
pop		{r0}
bx		r0

bx_r3:
bx		r3

.ltorg
Find_Anim_Entry:
@
