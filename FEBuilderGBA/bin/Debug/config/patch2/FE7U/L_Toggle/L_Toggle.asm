.thumb

@jumped to from 2A430

push	{r14}
ldr		r0,=#0x202BBF8
add		r0,#0x42
ldrb	r0,[r0]
lsl		r0,#0x1D
lsr		r0,#0x1E
cmp		r0,#2
bne		CheckLToggle
ldr		r2,=#0x203A3F0
ldrb	r0,[r2,#0xB]
mov		r1,#0xC0
mov		r3,r2
and		r0,r1
cmp		r0,#0
bne		Label1
ldr		r0,=#0x203A470
ldrb	r0,[r0,#0xB]
and		r0,r1
cmp		r0,#0
bne		Label2
mov		r0,r3
b		Label3
Label1:
ldr		r2,=#0x203A3F0
ldrb	r0,[r2,#0xB]
and		r0,r1
cmp		r0,#0
beq		Label2
mov		r0,#1
b		CheckLToggle
Label2:
mov		r0,r2
Label3:
ldr		r3,=#0x802A40C
mov		r14,r3
.short	0xF800
CheckLToggle:
ldr		r1,=#0x8B857F8
ldr		r1,[r1]
ldrh	r1,[r1,#0x4]		@button press
mov		r2,#0x80
lsl		r2,#2
and		r1,r2
cmp		r1,#0
beq		GoBack
mov		r1,#3
cmp		r0,#1
ble		Label4
mov		r0,#1
b		GoBack
Label4:
mov		r0,#3
GoBack:
pop		{r1}
bx		r1

.ltorg
