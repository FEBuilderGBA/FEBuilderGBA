.thumb
.org 0x0

@jumped to from 2B9C4
@r0=(class) growth, r1=number of levels

push	{r4,r14}
mov		r4,r0
mul		r4,r1
ldr		r0,Growth_Options
mov		r1,#0x1		@ is fixed mode even available
tst		r0,r1
beq		NormalGrowths
mov		r1,#0x4		@ if it is on, does fixed mode affect enemy autolevelling
tst		r0,r1
beq		NormalGrowths
lsr		r0,#0x10	@ event id
ldr		r1,Check_Event_ID
mov		r14,r1
.short	0xF800
cmp		r0,#0x0
beq		NormalGrowths

@Fixed growths mode
mov		r0,#0x0
FixedLoop:
cmp		r4,#100
blt		GoBack
sub		r4,#100
add		r0,#1
b		FixedLoop

NormalGrowths:
ldr		r0,Generate_RN
mov		r14,r0
mov		r0,r4
cmp		r4,#0
bge		Label1
add		r0,r4,#3
Label1:
asr		r0,#2
.short	0xF800
mov		r1,r0
ldr		r0,Func_2B9A0
mov		r14,r0
mov		r0,r4
cmp		r4,#0
bge		Label2
add		r0,r4,#7
Label2:
asr		r0,#0x3
sub		r0,r1,r0
add		r0,r4
.short	0xF800

GoBack:
pop		{r4}
pop		{r1}
bx		r1

.align
Check_Event_ID:
.long 0x08083DA8
Generate_RN:
.long 0x08000C80
Func_2B9A0:
.long 0x0802B9A0
Growth_Options:
@
