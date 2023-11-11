.thumb

@called at 7FE0C

push	{r14}
mov		r4,r0
ldr		r0,=#0x200316C
ldr		r3,=#0x80054E0			@Text_Clear, 3DC8
mov		r14,r3
.short	0xF800

ldr		r5,=#0x200310C
sub		r1,r5,#1
ldrb	r1,[r1]
cmp		r1,#0
beq		WriteStatLabels
ldr		r4,Alternate_Stat_Labels
WriteStatLabels:
mov		r0,r4
ldr		r3,=#0x807FA48
mov		r14,r3
.short	0xF800

ldr		r0,[r5,#0xC]
ldr		r3,=#0x80184DC
mov		r14,r3
.short	0xF800
pop		{r1}
bx		r1

.ltorg
Alternate_Stat_Labels:
@
