.thumb

@jumped to from 7FE78

@r4,r6 are free, r5=stat screen struct, and r6 is expected to be as well

mov		r6,r5
sub		r5,r6,#2
ldrb	r0,[r5]
cmp		r0,#0
beq		Label1				@if select wasn't pressed, buffers have already been cleared, no need to clear them again

ldr		r4,=#0x80C57BC
ldr		r0,=0x2022CF8		@bg0 buffer to clear
mov		r1,#0x12
mov		r2,#0xD
mov		r3,#0
mov		r14,r4
.short	0xF800
ldr		r0,=0x2023CF8		@bg2 buffer to clear
mov		r1,#0x12
mov		r2,#0xD
mov		r3,#0
mov		r14,r4
.short	0xF800

Label1:
ldr		r1,[r6,#0xC]
ldrb	r1,[r1,#0xB]
cmp		r1,#0x3F
ble		Label2
mov		r0,#0
strb	r0,[r5,#1]
Label2:
ldrb	r0,[r5,#1]			@toggle
cmp		r0,#0
bne		DisplayGrowths
ldr		r0,[r6,#0xC]
ldr		r1,=#0x807FE7B
bx		r1

DisplayGrowths:
mov		r4,#0
GrowthLoop:
ldr		r0,[r6,#0xC]
mov		r1,r4
bl		DrawGrowthFunc
add		r4,#1
cmp		r4,#7
blt		GrowthLoop
mov		r5,#0xF
ldr		r0,=#0x807FFAF
bx		r0

.ltorg

DrawGrowthFunc:
@r0=char data ptr, r1=number
push	{r4-r6,r14}
mov		r4,r0
lsl		r1,#2
ldr		r5,GrowthDisplayTable
add		r5,r1
ldr		r6,=#0x200323C
ldr		r0,[r4]				@char data
ldrb	r1,[r5]				@growth offset
ldrb	r2,[r0,r1]			@growth value
ldrh	r0,[r5,#2]			@place to display
add		r0,r6
mov		r1,#2				@palette index
ldr		r3,=#0x80061E4
mov		r14,r3
.short	0xF800
ldr		r0,[r4,#0xC]
mov		r1,#0x20
lsl		r1,#8
tst		r0,r1
beq		GoBack
ldr		r0,=#0x80296B4		@place where afa's drops is in the levelup routine
ldrb	r0,[r0]
ldrh	r1,[r5,#2]
add		r1,#2
add		r1,r6
ldr		r3,=#0x8006240
mov		r14,r3
.short	0xF800
GoBack:
pop		{r4-r6}
pop		{r0}
bx		r0

.ltorg
GrowthDisplayTable:
@
