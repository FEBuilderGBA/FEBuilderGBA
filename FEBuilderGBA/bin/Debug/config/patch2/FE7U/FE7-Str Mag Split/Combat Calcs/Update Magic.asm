.thumb
.org 0x0

@called from 2860C
@this updates magic from stat bonuses and stuff
@r0=def

push	{r14}
strb	r0,[r5,#0x17]
ldr		r0,Mag_Getter
mov		r14,r0
mov		r0,r6
.short	0xF800
mov		r1,r5
add		r1,#0x39
strb	r0,[r1]
ldr		r0,Luk_Getter
mov		r14,r0
mov		r0,r6
.short	0xF800
strb	r0,[r5,#0x19]
mov		r0,r6
pop		{r1}
bx		r1

.align
Luk_Getter:
.long 0x08018BB8
Mag_Getter:
@
