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
.long 0x08016B28

GetMagBonus:
mov		r1,#0xFF
and		r0,r1
lsl		r1,r0,#0x3
add		r0,r0,r1
lsl		r0,r0,#0x2
ldr		r1,ItemTable
add		r0,r0,r1
ldr		r0,[r0,#0xC]
mov 	r2,#0x9
cmp		r0,#0x0
beq		RetFalse
ldsb	r0,[r0,r2]
RetFalse:
ldr		r1,Venno_Func	@Gets stat bonus in index r2 of all items
bx		r1

.align
Venno_Func:
.long 0x08D40360+1
ItemTable:
