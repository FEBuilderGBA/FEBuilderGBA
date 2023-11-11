.thumb

sub		r6,#2
ldrb	r0,[r6]
cmp		r0,#0
beq		GoBack
mov		r0,#0
strb	r0,[r6]
ldr		r4,=#0x80C57CC
ldr		r0,=#0x200323C
ldr		r1,=#0x2022CF8
mov		r2,#0x12
mov		r3,#0x12
mov		r14,r4
.short	0xF800				@update bg0
ldr		r0,=#0x2003C3C
ldr		r1,=#0x2023CF8
mov		r2,#0x12
mov		r3,#0x12
mov		r14,r4
.short	0xF800				@update bg2
mov		r0,#0x5
ldr		r3,=#0x8000FFC
mov		r14,r3
.short	0xF800
GoBack:
add		sp,#0x8
pop		{r4-r6}
pop		{r0}
bx		r0

.ltorg
