.thumb
.org 0

@r0=some bitfield, used to determine which side is attacking, I guess
@currently 2029008+0xC

push	{r14}
mov		r1,#0x80
lsl		r1,#2
and		r0,r1
ldr		r1,Transform_RAM_Loc
cmp		r0,#0
beq		Label1
ldrb	r0,[r1,#1]				@character on right side's transformation table index
b		Label2
Label1:
ldrb	r0,[r1]					@^ for left side
Label2:
lsl		r0,#4
ldr		r1,Special_Animation_Table
add		r0,r1
pop		{r1}
bx		r1

.align
.equ Transform_RAM_Loc, Special_Animation_Table+4
Special_Animation_Table:
@
