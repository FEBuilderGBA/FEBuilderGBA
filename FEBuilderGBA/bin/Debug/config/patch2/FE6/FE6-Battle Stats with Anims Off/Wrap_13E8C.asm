.thumb
.org 0

push	{r14}
add		sp,#-0x4
mov		r3,#0
str		r3,[sp]
ldr		r3,=#0x8013E8C
mov		r14,r3
ldr		r2,[r7,#0x8]
ldr		r3,[r7,#0xC]
ldr		r0,[r7]
.short	0xF800
ldr		r0,[r7,#0xC]
add		sp,#0x4
pop		{r1}
bx		r1

.ltorg
