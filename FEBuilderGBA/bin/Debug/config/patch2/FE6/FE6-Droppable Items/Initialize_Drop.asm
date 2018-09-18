.thumb
.org 0

@fe8 equivalent is 328D0

push	{r4-r7,r14}
mov		r7,r8
push	{r7}
mov		r8,r0
mov		r7,#0
ldr		r6,=#0x2039214		@attacker battle struct
mov		r1,#0xB
ldsb	r2,[r6,r1]
add		r0,#0x64
strh	r2,[r0]
ldr		r4,=#0x2039290		@defender battle struct
ldsb	r2,[r4,r1]
strh	r2,[r0,#0x2]
mov		r0,#0x11
ldsb	r0,[r6,r0]
cmp		r0,#0
bne		Label1
ldrb	r0,[r6,#0xB]
ldr		r1,Get_Char_Data
mov		r14,r1
.short	0xF800
mov		r7,r0
ldrb	r0,[r4,#0xB]
ldr		r1,Get_Char_Data
mov		r14,r1
.short	0xF800
mov		r5,r0
Label1:
mov		r0,#0x11
ldsb	r0,[r4,r0]
cmp		r0,#0
bne		Label2
ldrb	r0,[r4,#0xB]
ldr		r1,Get_Char_Data
mov		r14,r1
.short	0xF800
mov		r7,r0
ldrb	r0,[r6,#0xB]
ldr		r1,Get_Char_Data
mov		r14,r1
.short	0xF800
mov		r5,r0
Label2:
cmp		r7,#0
beq		RetOne				@if r7=0, no one died
mov		r0,r7
ldr		r1,Can_Drop_Item
mov		r14,r1
.short	0xF800
cmp		r0,#0
beq		RetOne
ldrh	r0,[r7,#0x1C]		@first item
cmp		r0,#0
beq		RetOne
ldrb	r0,[r5,#0xB]
mov		r1,#0xC0
tst		r0,r1
bne		RetOne				@non-player units can't receive items
mov		r0,r7
ldr		r1,=#0x8017520		@returns first empty inventory slot
mov		r14,r1
.short	0xF800
sub		r0,#1
lsl		r0,#1
add		r0,#0x1C
ldrh	r1,[r7,r0]
mov		r0,r5
mov		r2,r8
ldr		r3,=0x80121F4		@handles the 'item given' routine, I think
mov		r14,r3
.short	0xF800
mov		r0,#0
b		GoBack
RetOne:
mov		r0,#1
GoBack:
pop		{r7}
mov		r8,r7
pop		{r4-r7}
pop		{r1}
bx		r1

.ltorg
Get_Char_Data:
.long 0x801860C
Can_Drop_Item:
@
