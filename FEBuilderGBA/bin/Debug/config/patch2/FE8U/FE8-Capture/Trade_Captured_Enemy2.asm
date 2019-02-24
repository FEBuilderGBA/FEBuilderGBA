.thumb
.org 0x0

@branched to 25298
@checks if the rescued person can be traded with, and sets flags, r1=char data, r2=allegiance byte
@r0=char data of rescuee

push	{r14}
push	{r0}
ldrb	r0,[r0,#0xB]
ldr		r1,[r5]
ldrb	r1,[r1,#0xB]
ldr		r2,Compare_Allegiance_Func
mov		r14,r2
.short	0xF800
pop		{r1}
ldrb	r2,[r1,#0xB]
cmp		r0,#0x0
bne		GoBack
mov		r0,#0x0
ldrb	r3,[r1,#0x13]
cmp		r3,#0x0
bne		GoBack
mov		r0,#0x1
GoBack:
cmp		r0,#0x0
pop		{r3}
bx		r3

.align
Compare_Allegiance_Func:
.long 0x08024DA4
