.thumb
.org 0x0

@r0 and r1 are important. This function just sets the Capturing bit before bx'ing to the usual Attack menu effect ptr
push	{r0,r1}
ldr		r0,CurrentCharPtr
ldr		r0,[r0]
ldr		r1,[r0,#0xC]
mov		r2,#0x80
lsl		r2,#0x17		@byte 4, 0x40 for Capture
orr		r1,r2
str		r1,[r0,#0xC]
pop		{r0,r1}
ldr		r2,Attack_Effect_Func
bx		r2

.align
CurrentCharPtr:
.long 0x03004E50
Attack_Effect_Func:
.long 0x08022B30+1
