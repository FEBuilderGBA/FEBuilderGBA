.thumb
.org 0x0

@bl'd to from 328F0
@r4=defender, r6=attacker battle structs
@r7 should contain char data ptr of person dropping item, 0 if capturing; r5 has char data of receiver

push	{r14}
ldrb	r5,[r6,#0xB]		@attacker allegiance
ldrb	r7,[r4,#0xB]		@defender allegiance
ldrb	r0,[r4,#0x13]		@defender current hp
cmp		r0,#0x0
beq		GetCharData
ldrb	r0,[r6,#0x13]		@attacker current hp
cmp		r0,#0x0
beq		SwitchCharacters
mov		r7,#0x0
b		GoBack				@if neither party is dead, skip this business
SwitchCharacters:
ldrb	r7,[r6,#0xB]
ldrb	r5,[r4,#0xB]
GetCharData:
ldr		r0,Get_Char_Data
mov		r14,r0
mov		r0,r5
.short	0xF800
mov		r5,r0
ldr		r0,Get_Char_Data
mov		r14,r0
mov		r0,r7
.short	0xF800
mov		r7,r0
ldr		r0,Is_Capture_Set
mov		r14,r0
mov		r0,r5
.short	0xF800
cmp		r0,#0x0
beq		GoBack
ldr		r0,[r5,#0xC]
mov		r1,#0x80
lsl		r1,#0x17
mvn		r1,r1
and		r0,r1
str		r0,[r5,#0xC]		@remove the 'is capturing' bit
ldr		r0,Write_Rescue_Data
mov		r14,r0
mov		r0,r5
mov		r1,r7
.short	0xF800
mov		r7,#0x0				@captured units don't drop anything
GoBack:
pop		{r0}
bx		r0

.align
Get_Char_Data:
.long 0x08019430
Write_Rescue_Data:
.long 0x0801834C
Is_Capture_Set:
@
