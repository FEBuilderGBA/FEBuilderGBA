.thumb
.org 0

@called at from 70908	@FE8U
@called at from 72EB0	@FE8J

push	{r4,r14}
ldrh	r0,[r5,#0xC]
ldr		r3,Find_Anim_Entry
bl		bx_r3
ldrh	r4,[r0,#0x2]			@initial transformation animation id
sub		r4,#1
mov		r1,r4
mov		r0,r5
@ldr	r3,=#0x80589E1	@FE8U
ldr		r3,=#0x8059811	@FE8J
bl		bx_r3
mov		r0,r5
mov		r1,#0
@ldr	r3,=#0x805A07D	@FE8U
ldr		r3,=#0x805AE21	@FE8J
bl		bx_r3
mov		r0,r4
pop		{r4}
pop		{r1}
bx		r1

bx_r3:
bx		r3

.ltorg
Find_Anim_Entry:
@
