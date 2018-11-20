.thumb
.org 0x0

@r0=char data
push	{r4,r14}
mov		r4,r0
ldr		r1,CanWield
mov		r14,r1
.short	0xF800			@returns first wieldable item
lsl		r0,r0,#0x10
lsr		r0,r0,#0x10
bl		GetMagBonus
mov		r1,#0x39
ldsb	r1,[r4,r1]
add		r0,r0,r1
pop		{r4}
pop		{r1}
bx		r1

.align
CanWield:
.long 0x08016764

GetMagBonus:
mov		r2,#0x0
cmp		r0,#0x0
beq		RetFalse
mov		r1,#0xFF
and		r0,r1
lsl		r1,r0,#0x3
add		r0,r0,r1
lsl		r0,r0,#0x2
ldr		r1,ItemTable
add		r0,r0,r1
ldr		r0,[r0,#0xC]
cmp		r0,#0x0
beq		RetFalse
ldrb	r2,[r0,#0x9]
lsl		r2,r2,#0x18
asr		r2,r2,#0x18
RetFalse:
mov		r0,r2
bx		r14

.align
ItemTable:
