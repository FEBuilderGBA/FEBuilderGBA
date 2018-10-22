@thumb


push	{r4-r6,r14}
ldr		r0,=$03004df0	;CurrentCharPtr
ldr		r0,[r0]
ldr		r1,[r0,#0xC]		;status word
mov		r2,#0x50			;is rescuing + has moved
tst		r1,r2
bne		RetFalse
mov		r2,#0x80
lsl		r2,r2,#0x4				;is in ballista
tst		r1,r2
bne		RetFalse

;普通の攻撃出現判定
ldr	r0, =$0802495e
mov	pc, r0

RetFalse:
	mov		r0, #0x3
	pop		{r4-r6}
	pop		{r1}
	bx		r1



@ltorg
adr:
