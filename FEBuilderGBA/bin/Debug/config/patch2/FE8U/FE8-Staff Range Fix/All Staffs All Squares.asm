.thumb
.org 0x0

push	{r14}
mov		r1,#0x0
ldr		r2,StaffCheck
ldr		r3,WriteAllSquares
bl		goto_r3
pop		{r0}
bx		r0

goto_r3:
bx		r3

.align
StaffCheck:
.long 0x080167A4+1
WriteAllSquares:
