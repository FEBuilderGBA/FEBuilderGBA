.thumb
.org 0x0

@r0 has char data, r1 has slot # (-1 in this case)
push	{r4,r14}
mov		r4,r0
ldr		r2,WeaponCheck
ldr		r3,ReturnRangeBitfield
bl		goto_r3
mov		r2,#0x0
mov		r12,r0					@ain't gonna be no ballista
mov		r2,r0
mov		r3,r1
mov		r0,#0x10
ldsb	r0,[r4,r0]
mov		r1,#0x11
ldsb	r1,[r4,r1]
ldr		r4,ReturnRangeBitfield+4	@Actually the "Range Write" function
bl		goto_r4
pop		{r4}
pop		{r0}
bx		r0

goto_r3:
bx		r3
goto_r4:
bx		r4

.align
WeaponCheck:
.long 0x08016574+1
ReturnRangeBitfield:
