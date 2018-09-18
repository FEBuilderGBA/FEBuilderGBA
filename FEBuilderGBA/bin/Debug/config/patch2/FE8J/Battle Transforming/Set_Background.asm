.thumb
.org 0

@.equ origin, 0x70B3C @FE8U
.equ  origin, 0x730E4 @FE8J
@.equ Compare_Allegiance_Func2, . + 0x24D8C - origin @FE8U
.equ Compare_Allegiance_Func2,  . + 0x24D3C - origin @FE8J
@.equ Func_6FA84, . + 0x6FA84 - origin @FE8U
.equ Func_6FA84,  . + 0x72034 - origin @FE8J

push	{r4-r7,r14}
@ldr	r0,=#0x203E120		@not a bloody clue what this is, but if it's not 0<=x<=2, then we skip everything
ldr		r0,=#0x203E11C		@FE8J
mov		r1,#0
ldsh	r0,[r0,r1]
cmp		r0,#0
blt		Label1
cmp		r0,#2
ble		Label2
Label1:
b		GoBack1
Label2:
@ldr	r0,=#0x203E188	@FE8U
ldr		r0,=#0x203E184	@FE8J
ldr		r6,[r0]				@left side's battle struct
ldr		r7,[r0,#0x4]		@right side's battle struct
@first, we check the left side for stuff
mov		r0,r6
mov		r1,r7
mov		r2,#0
bl		FindBackground		@also, if transforming animation, writes id to table in ram
@and then the right side
mov		r0,r7
mov		r1,r6
mov		r2,#1
bl		FindBackground
GoBack1:
pop		{r4-r7}
pop		{r0}
bx		r0

.ltorg

FindBackground:
@r0=battle struct of side we're looking at, r1=other side's battle struct, r2=0 for left, 1 for right
push	{r4-r7,r14}
mov		r4,r0
mov		r5,r1
mov		r6,r2
ldr		r0,[r4,#0x4]
ldrb	r0,[r0,#0x4]		@class id
cmp		r0,#0x65			@dracozombie
bne		CheckDK
mov		r1,#1
b		Call_6FA84
CheckDK:
cmp		r0,#0x66			@demon king
bne		CheckForTransform
mov		r1,#2
b		Call_6FA84
CheckForTransform:
mov		r1,#0x4A
ldrb	r1,[r4,r1]			@equipped item
lsl		r1,#0x8
orr		r0,r1
@r0 now has class/item short
ldr		r1,Transformation_Table
mov		r2,#0
TransformLoop:
ldrh	r3,[r1]
cmp		r0,r3
beq		IsTransforming
add		r1,#0x10
add		r2,#1				@index to store to ram so we can retrieve the correct entry later
cmp		r3,#0
bne		TransformLoop
mov		r1,#0
b		Call_6FA84
IsTransforming:
mov		r7,r2
mov		r0,#0x30
ldrb	r0,[r4,r0]			@status byte
mov		r1,#0xF
and		r0,r1
cmp		r0,#2				@sleep
beq		GoBack2
cmp		r0,#0xB				@petrified
beq		GoBack2				@basically, if either of these statuses is true, don't transform. Vanilla also doesn't transform if the opponent has a status staff, but I'm removing that
ldrb	r0,[r4,#0xB]
ldrb	r1,[r5,#0xB]
bl		Compare_Allegiance_Func2
cmp		r0,#0
bne		GoBack2				@if both units are on the same team, don't transform
@ldr	r0,=#0x203E104	@FE8U
ldr		r0,=#0x203E100	@FE8J
lsl		r1,r6,#1
ldrh	r0,[r0,r1]
cmp		r0,#0
beq		GoBack2				@not entirely sure what this is; suspect it's a bool for whether there's a unit on that side
ldr		r0,Transform_RAM_Loc
strb	r7,[r0,r6]			@store index of entry in transform table in ram for later functions; dunno where else to put it
mov		r1,#3
Call_6FA84:
ldr		r0,=#0x2000000
lsl		r2,r6,#3
ldr		r0,[r0,r2]
bl		Func_6FA84
GoBack2:
pop		{r4-r7}
pop		{r0}
bx		r0

.ltorg
.equ Transform_RAM_Loc, Transformation_Table+4
Transformation_Table:
@
