.thumb
.org 0x0

@jumped to from 7BED6
@want to write this stuff to bg0, has name and hp numbers/graphics
@7BD54 is the example
@[r5] = pointer to battle struct of person we're looking for
@call 13168 with r0=background place to write to, r1=pointer to stack with ascii of numbers to write (kind of), r2=5020 (relates to tile number), r3=3 (number of tiles to clear), sp=bool(true if displaying ??)

ldr 	r0, Const_203E1F0
add		r0,#0x5E
ldrb	r0,[r0]			@number of boxes to show
cmp		r0,#0x2
beq		HasSecondUnit
b		GoBack
HasSecondUnit:
bl		Get_Battle_Name_Graphics	@put graphics in r0 and palette in r1
mov		r7,r1
ldr		r1,BattleStatTileLoc
swi		0x12			@lz77 decompression
ldr		r0,Copy_Palette_To_Ram
mov		r14,r0
mov		r0,r7
mov		r1,#0x60		@bank
mov		r2,#0x20		@length
.short	0xF800
mov		r7,#0x30
lsl		r7,#0x8			@to be orr'd with tile index and stored in bg (the 4 is the palette bank)
ldr		r6,[r5]			@battle struct
ldr		r4,Bg0Buffer

@HIT

mov		r0,#0x5
mov		r1,#0x4
mov		r2,r5
bl		CoordsToNum
add		r1,r0,r4
mov		r0,#0x64
ldrh	r0,[r6,r0]		@battle hit
cmp		r0,#0xFF
bne		Label10
lsl		r0,#0x18
asr		r0,#0x18
Label10:
bl		Num2Ascii
@HIT NAME
mov		r0,#0x1
mov		r1,#0x4
mov		r2,r5
bl		CoordsToNum
add		r0,r4
mov		r1,#0x40		@tile id
orr		r1,r7
strh	r1,[r0]
add		r1,#0x1
strh	r1,[r0,#0x2]

@DAMAGE

mov		r0,#0xA
mov		r1,#0x4
mov		r2,r5
bl		CoordsToNum
add		r1,r0,r4
mov		r0,#0x5A
ldrh	r0,[r6,r0]		@attack
cmp		r0,#0xFF
bne		DoesDamage
lsl		r0,#0x18
asr		r0,#0x18
b		DisplayDamage
DoesDamage:
mov		r3,#0x0
cmp		r3,r9
bne		Label6
mov		r3,#0x1
Label6:
mov		r2,#0x14
mul		r2,r3
ldr		r3,Const_203E1F0
add		r2,r3
ldr		r2,[r2]			@the other person
add		r2,#0x5C
ldrh	r2,[r2]			@other person's defense
sub		r0,r0,r2		@damage
cmp		r0,#0x0
bge		DisplayDamage
mov		r0,#0x0
DisplayDamage:
bl		Num2Ascii
@DMG NAME
mov		r0,#0x6
mov		r1,#0x4
mov		r2,r5
bl		CoordsToNum
add		r0,r4
mov		r1,#0x42		@tile id
orr		r1,r7
strh	r1,[r0]
add		r1,#0x1
strh	r1,[r0,#0x2]
add		r1,#0x1
strh	r1,[r0,#0x4]	@dmg takes 3 tiles

@CRIT

mov		r0,#0x5
mov		r1,#0x5
mov		r2,r5
bl		CoordsToNum
add		r1,r0,r4
mov 	r0,#0x6A
ldrh	r0,[r6,r0]		@battle crit
cmp		r0,#0xFF
bne		Label8
lsl		r0,#0x18
asr		r0,#0x18
Label8:
bl		Num2Ascii
@CRIT NAME
mov		r0,#0x1
mov		r1,#0x5
mov		r2,r5
bl		CoordsToNum
add		r0,r4
mov		r1,#0x45		@tile id
orr		r1,r7
strh	r1,[r0]
add		r1,#0x1
strh	r1,[r0,#0x2]

@AS

mov		r0,#0xA
mov		r1,#0x5
mov		r2,r5
bl		CoordsToNum
add		r1,r0,r4
ldr		r0,ActionStruct
ldrb	r0,[r0,#0x11]	@action
cmp		r0,#0x3			@staves?
bne		Label11
mov		r0,#0x1			@don't display AS when using staves
neg		r0,r0
b		Label9
Label11:
mov 	r0,#0x5E
ldrh	r0,[r6,r0]		@attack speed
cmp		r0,#0xFF
bne		Label9
lsl		r0,#0x18
asr		r0,#0x18
Label9:
bl		Num2Ascii
@AS NAME
mov		r0,#0x6
mov		r1,#0x5
mov		r2,r5
bl		CoordsToNum
add		r0,r4
mov		r1,#0x47		@tile id
orr		r1,r7
strh	r1,[r0]
add		r1,#0x1
strh	r1,[r0,#0x2]
add		r1,#0x1
strh	r1,[r0,#0x4]

GoBack:
pop		{r3-r5}
mov		r8,r3
mov		r9,r4
mov		r10,r5
pop		{r4-r6}
pop		{r0}
bx		r0

.align
Bg0Buffer:
.long 0x02022CA8
Const_203E1F0:
.long 0x0203E1F0
BattleStatTileLoc:
.long 0x06000800
Copy_Palette_To_Ram:
.long 0x08000DB8
ActionStruct:
.long 0x0203A958

Num2Ascii:
@r0=number, r1=place to write number to in bg0 buffer
@7BA28 is example
push	{r4-r7,r14}
add		sp,#-0x8
mov		r4,r0
mov		r5,r1
add		r7,sp,#0x4
mov		r0,#0x0
str		r0,[sp]
cmp		r4,#0x0
bge		NotDashes
mov		r0,#0x1
str		r0,[sp]
b		Label5
NotDashes:
mov		r6,#0x0
HundredsLoop:
cmp		r4,#0x63
ble		Label1
sub		r4,#0x64
add		r6,#0x1
b		HundredsLoop
Label1:
add		r6,#0x30			@ascii number
cmp		r6,#0x30
bne		Label2
mov		r6,#0x20			@space
Label2:
strb	r6,[r7,#0x1]		@digits are written in reverse order on stack (ones, tens, hundreds)
mov		r6,#0x0
TensLoop:
cmp		r4,#0x9
ble		Label3
sub		r4,#0xA
add		r6,#0x1
b		TensLoop
Label3:
add		r6,#0x30
cmp		r6,#0x30
bne		Label4
ldrb	r0,[r7,#0x1]
cmp		r0,#0x20
bne		Label4				@if hundreds place is blank, then this should be blank too, else, 0
mov		r6,#0x20
Label4:
strb	r6,[r7,#0x2]
add		r4,#0x30			@ones place shouldn't need any more funny math, and should always display something
strb	r4,[r7,#0x3]
mov		r4,#0x20
strb	r4,[r7]
Label5:
ldr		r0,Number_Tile_Copy_Func
mov		r14,r0
mov		r0,r5				@place
add		r1,r7,#0x3			@index on stack with beginning number
ldr		r2,Num_5020
mov		r3,#0x3				@number of tiles to clear to display this stuff
.short	0xF800
add		sp,#0x8
pop		{r4-r7}
pop		{r0}
bx		r0

.align
Number_Tile_Copy_Func:
.long 0x08013168
Num_5020:
.long 0x00005020

CoordsToNum:
@r0=x, r1=y, r2=203E1F0. Coordinates are relative to the box
@(32*(y+y')+x+x')*2 = 122/10E (E,1) (4,1) for the usual HP placement
ldrb	r3,[r2,#0x11]	@y coord of box
add		r1,r3
lsl		r1,#0x5
add		r0,r1
ldrb	r1,[r2,#0x10]
add		r0,r1
lsl		r0,#0x1
bx		r14

Get_Battle_Name_Graphics:
ldr		r0,Battle_Name_Graphics
ldr		r1,Battle_Name_Graphics+4
bx		r14

.align
Battle_Name_Graphics:
@.long 0x088025D8
@Palette
