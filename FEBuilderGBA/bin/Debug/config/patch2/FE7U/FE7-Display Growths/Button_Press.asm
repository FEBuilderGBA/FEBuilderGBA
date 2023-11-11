.thumb

@jumped to from 813EE

mov		r1,#0x80
lsl		r1,#1
tst		r0,r1
beq		SelectButton
mov		r0,r5
mov		r1,#0
ldr		r3,=#0x8004720
mov		r14,r3
.short	0xF800
ldr		r0,=#0x200310C
ldr		r1,=#0x80813F9
bx		r1

SelectButton:
ldr		r1,=#0x200310C
ldrb	r0,[r1]
cmp		r0,#0
bne		GoBack			@not stat screen
ldr		r2,[r1,#0xC]
ldrb	r2,[r2,#0xB]
mov		r3,#0xC0
tst		r2,r3
bne		GoBack			@don't do anything if not player unit
sub		r1,#2
mov		r3,#1
strb	r3,[r1]
ldrb	r2,[r1,#1]
eor		r2,r3
strb	r2,[r1,#1]
ldr		r3,=#0x80804C8
mov		r14,r3
.short	0xF800
GoBack:
pop		{r4-r6}
pop		{r0}
bx		r0

.ltorg
