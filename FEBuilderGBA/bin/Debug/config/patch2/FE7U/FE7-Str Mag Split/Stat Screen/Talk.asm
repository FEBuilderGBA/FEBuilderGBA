.thumb
.org 0x0

push	{r14}
ldr		r0,GetTalkPerson
mov		r14,r0
ldr		r0,[r6,#0xC]		@char data
ldr		r0,[r0]
ldrb	r0,[r0,#0x4]		@char byte
.short	0xF800				@given char byte, returns first talk event that hasn't occurred yet for this character
cmp		r0,#0x0
bne		FoundAPerson
ldr		r0,BlankTextID
b		HaveTextID
FoundAPerson:
mov		r1,#0x34
mul		r0,r1
ldr		r1,GetTalkPerson+4	@actually character table
add		r0,r1
ldrh	r0,[r0]
HaveTextID:
ldr		r1,WriteTextToRam
mov		r14,r1
.short	0xF800
mov		r3,r0
ldr		r0,DisplayText
mov		r14,r0
mov		r0,r6
mov		r1,#0x80
lsl		r1,r1,#0x1
add		r0,r1
mov		r1,#0x18
mov		r2,#0x2
.short	0xF800
ldr		r0,[r5,#0xC]
add		r0,#0x30
ldrb	r2,[r0]
mov		r0,#0xF
and		r0,r2
cmp		r0,#0x0				@hopefully the flags are still set... (they are)
pop		{r1}
bx		r1

.align
WriteTextToRam:
.long 0x08012C60
DisplayText:
.long 0x08005B18
BlankTextID:
.long 0x0000112A
GetTalkPerson:
