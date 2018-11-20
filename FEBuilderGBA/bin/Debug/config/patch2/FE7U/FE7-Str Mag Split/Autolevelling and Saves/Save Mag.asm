.thumb
.org 0x0

@paste at 9faf0
push	{r5-r7,r14}
mov		r5,#0x0
ldr		r6,GetCharStruct
ldr		r7,MagSaveTable
Loop:
mov		r14,r6
mov		r0,r5
.short	0xF800
add		r0,#0x39
ldrb	r0,[r0]
strb	r0,[r7,r5]
add		r5,#0x1
cmp		r5,#0xBF
ble		Loop
ldr		r0,SaveFunction
mov		r14,r0
mov		r0,r7
mov		r2,#0xC0
mov		r1,r4
.short	0xF800
pop		{r5-r7}
pop		{r0}
bx		r0

.align
GetCharStruct:
.long 0x08018D0C
MagSaveTable:
.long 0x0203E7A0
SaveFunction:
.long 0x080BFBD8
