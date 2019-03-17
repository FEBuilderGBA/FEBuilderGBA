.thumb
.org 0x0

push	{r14}
mov		r1,#0x1
ldr		r2,WeaponCheck
ldr		r3,WriteAllSquares
bl		goto_r3
pop		{r0}
bx		r0

goto_r3:
bx		r3
.align
WeaponCheck:
.long 0x08016574+1
WriteAllSquares:
