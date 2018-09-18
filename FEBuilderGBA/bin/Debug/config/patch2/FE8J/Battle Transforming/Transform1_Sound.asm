.thumb
.org 0

@jumped to at 7096C	@FE8U
@jumped to at 72F14	@FE8J
@actually also seems to handle the transition into the main battle animation, not just the sound of the initial change

push	{r4-r6,r14}
mov		r5,r0
ldr		r4,[r5,#0x5C]
ldrh	r0,[r4,#0xC]
ldr		r3,Find_Anim_Entry
bl		bx_r3
mov		r6,r0
ldrh	r0,[r5,#0x2C]		@frame count
add		r0,#1
strh	r0,[r5,#0x2C]
lsl		r0,#0x10
asr		r0,#0x10
cmp		r0,#0x1A			@this amount dictates how long the animation takes
bne		Label1
ldrh	r0,[r6,#0xA]		@sound id for beginning transformation
mov		r1,#0x80
lsl		r1,#1
@ldr	r3,=#0x8071991	@FE8U
ldr		r3,=#0x8073E75	@FE8J
bl		bx_r3
mov		r0,#2
ldsh	r1,[r4,r0]
ldrh	r0,[r6,#0xA]
mov		r2,#1
@ldr	r3,=#0x8071AB1	@FE8U
ldr		r3,=#0x8073F95	@FE8J
bl		bx_r3
Label1:
ldr		r0,[r4,#0x20]
ldrb	r0,[r0,#0x3]
mov		r1,#0x3F
and		r0,r1
cmp		r0,#0
bne		GoBack
ldrh	r0,[r6,#0xC]		@sound id for battle anim
mov		r1,#0x80
lsl		r1,#1
@ldr	r3,=#0x8071991	@FE8U
ldr		r3,=#0x8073E75	@FE8J
bl		bx_r3
mov		r0,#2
ldsh	r1,[r4,r0]
ldrh	r0,[r6,#0xC]
mov		r2,#1
@ldr	r3,=#0x8071AB1	@FE8U
ldr		r3,=#0x8073F95	@FE8J
bl		bx_r3
mov		r0,r4
ldrh	r1,[r6,#0x4]		@animation id for after initial transformation
sub		r1,#1
@ldr	r3,=#0x80589E1	@FE8U
ldr		r3,=#0x8059811	@FE8J
bl		bx_r3
mov		r0,r5
@ldr	r3,=#0x8002E95	@FE8U
ldr		r3,=#0x8002DE5	@FE8J
bl		bx_r3
GoBack:
pop		{r4-r6}
pop		{r0}
bx		r0

bx_r3:
bx		r3

.ltorg
Find_Anim_Entry:
@
