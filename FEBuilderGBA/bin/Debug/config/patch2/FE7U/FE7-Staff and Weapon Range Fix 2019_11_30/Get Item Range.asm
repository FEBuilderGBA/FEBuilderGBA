.thumb
.org 0x0

@r0 has char data, r1 has item id/uses
push	{r4,r14}
mov		r2,#0xFF
and		r1,r2
lsl		r2,r1,#0x3
add		r2,r2,r1
lsl		r2,r2,#0x2
ldr		r1,Item_Table
add		r2,r2,r1
ldrb	r4,[r2,#0x19]		@range byte
cmp		r4,#0xFF			@total range
bne		NotTotal
mov		r0,#0x0
mov		r1,r0
b		GoBack
NotTotal:
mov		r1,#0xF
and		r1,r4
cmp		r1,#0x0
bne		NotMag2
push	{r4}
ldr		r4,Mag2Function
bl		goto_r4
pop		{r4}
cmp		r0,#0xF
bhi		MakeHalfword
lsr		r4,r4,#0x4
lsl		r4,r4,#0x4
orr		r4,r0
NotMag2:					@here, we make the range bitfield
ldr		r1,Bitfield_Constructor
mov		r0,r4
Label1:
cmp		r0,#0x20
blt		Label2
lsl		r1,r1,#0x1
sub		r0,#0x10
b		Label1
Label2:
neg		r0,r0
add		r0,#0x1F
lsl		r1,r0
lsr		r1,r0
mov		r0,r1
lsr		r0,r0,#0x11
mov		r1,#0x0
b		GoBack
MakeHalfword:
lsr		r1,r4,#0x4
lsl		r1,r1,#0x8
orr		r1,r0
mov		r0,#0x0
GoBack:
pop		{r4}
pop		{r2}
bx		r2

goto_r4:
bx		r4

.align
Mag2Function:
.long 0x080184B4+1
Bitfield_Constructor:
.long 0xFFFE0000
Item_Table:
