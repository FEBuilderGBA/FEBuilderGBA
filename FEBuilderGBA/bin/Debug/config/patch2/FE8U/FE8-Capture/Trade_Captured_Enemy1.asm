.thumb
.org 0x0

@branched to from 25224
@return true if the two people can trade
@r5 has a pointer to the target's char data ptr, r4=current char's data ptr

push	{r14}
ldrb	r0,[r0,#0xB]
ldrb	r1,[r4,#0xB]
ldr		r2,Compare_Allegiance_Func
mov		r14,r2
.short	0xF800
cmp		r0,#0x0
bne		GoBack			@if they're already on the same side, no need to check if captured
ldrb	r1,[r4,#0x13]
cmp		r1,#0x0
bne		GoBack
mov		r0,#0x1
GoBack:
pop		{r1}
bx		r1

.align
Compare_Allegiance_Func:
.long 0x08024DA4
