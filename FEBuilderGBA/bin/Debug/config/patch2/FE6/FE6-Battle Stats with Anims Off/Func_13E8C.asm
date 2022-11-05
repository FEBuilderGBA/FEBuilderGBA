.thumb
.org 0

@because r3 had a parameter, we pushed it prior to jumping here, so make sure to pop it back

pop		{r3}
push	{r4-r5,r14}
mov		r4,r0
cmp		r3,#0
ble		Label1
mov		r5,#0
Label2:
strh	r5,[r0]
sub		r0,#2
sub		r3,#1
cmp		r3,#0
bne		Label2
Label1:
ldr		r0,[sp,#0xC]
cmp		r0,#0
beq		Label3
mov		r0,r2
add		r0,#0xA
strh	r0,[r4]
sub		r4,#2
strh	r0,[r4]
b		GoBack
Label4:
ldrb	r0,[r1]
add		r0,r2
sub		r0,#0x30
strh	r0,[r4]
sub		r4,#2
sub		r1,#1
Label3:
ldrb	r0,[r1]
cmp		r0,#0x20
bne		Label4
GoBack:
pop		{r4-r5}
pop		{r0}
bx		r0
