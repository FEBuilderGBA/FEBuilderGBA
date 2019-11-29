.thumb
.org 0x0

@r0 has item id, r1 has distance between the units, r2 has char data
push	{r4,r5,r14}
mov		r4,r1
mov		r1,#0xFF
and		r0,r1
lsl		r1,r0,#0x3
add		r0,r0,r1
lsl		r0,r0,#0x2
ldr		r1,ItemTable
add		r1,r1,r0
ldrb	r5,[r1,#0x19]
cmp		r5,#0xFF
bne		NotTotal
mov		r1,#0x1
b		GoBack
NotTotal:
mov		r0,#0xF
and		r0,r5
cmp		r0,#0x0
bne		NotMag2
mov		r0,r2
ldr		r3,Mag2Function
bl		goto_r3
NotMag2:
mov		r1,#0x0
cmp		r0,r4
blt		GoBack
lsr		r0,r5,#0x4
cmp		r0,r4
bgt		GoBack
mov		r1,#0x1
GoBack:
mov		r0,r1
pop		{r4,r5}
pop		{r1}
bx		r1

goto_r3:
bx		r3

.align
Mag2Function:
.long 0x080184B4+1
ItemTable:
