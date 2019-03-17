.thumb
.org 0x0

@r0 has char data, r1 has flag (1 to check ballista, 0 to not), r2 has check address to bx to 
push	{r4-r7,r14}
mov		r7,r8
push	{r7}
cmp		r1,#0x0
beq		NoBallistaAbility
ldr		r1,[r0]
ldr		r3,[r0,#0x4]
ldr		r1,[r1,#0x28]			@char abilities
ldr		r3,[r3,#0x28]			@class abilities
orr		r1,r3
mov		r3,#0x80
and		r1,r3
cmp		r1,#0x0
beq		NoBallistaAbility
mov		r1,#0x1
NoBallistaAbility:
mov		r12,r1					@r12 has ballista check flag
mov		r1,#0x1
neg		r1,r1
ldr		r3,GetRangeBitfield
bl		goto_r3					@returns range bitfield in r0 and min/max halfword in r1 (if some range was > 0xF)
mov		r8,r1					@min/max h-word
mov		r7,r0					@range bitfield
ldr		r0,MapSize				@contains width and height as halfword, although they are 1-aligned rather than 0-aligned, so we subtract 1
mov		r6,#0x0
ldsh	r6,[r0,r6]				@width
sub		r6,#0x1
mov		r4,#0x2
ldsh	r4,[r0,r4]				@height
sub		r4,#0x1
LoopThroughY:
mov		r5,r6					@r5 has current x coord, r6 has max width
LoopThroughX:
ldr		r0,CanMoveMap			@contains row pointers to a map showing squares that can be moved to. Returns 0xFF for false.
ldr		r0,[r0]
lsl		r1,r4,#0x2
add		r0,r0,r1
ldr		r0,[r0]
add		r0,r0,r5
ldrb	r0,[r0]
cmp		r0,#0x78				@Why 0x78? I have no idea. Value = 0xFF if cannot be moved to, otherwise, the number of squares away from current point
bhi		DecrementX
ldr		r0,OccupiedTileMap		@contains row pointers to a map showing occupied squares with the occupant's allegiance byte
ldr		r0,[r0]
lsl		r1,r4,#0x2
add		r0,r0,r1
ldr		r0,[r0]
add		r0,r0,r5
ldrb	r0,[r0]
cmp		r0,#0x0
bne		DecrementX
ldr		r0,UnknownMap			@no idea what's here
ldr		r0,[r0]
lsl		r1,r4,#0x2
add		r0,r0,r1
ldr		r0,[r0]
add		r0,r0,r5
ldrb	r0,[r0]
cmp		r0,#0x0
bne		DecrementX
mov		r0,r5					@current x
mov		r1,r4					@current y
mov		r2,r7					@range bitfield
mov		r3,r8					@range halfword (also, r12 has flag)
push	{r4}
ldr		r4,GetRangeBitfield+4	@writes the range at that square
bl		goto_r4
pop		{r4}
DecrementX:
sub		r5,#0x1
cmp		r5,#0x0
bge		LoopThroughX
sub		r4,#0x1
cmp		r4,#0x0
bge		LoopThroughY
ldr		r1,CanMoveMap
ldr		r1,[r1]
ldr		r0,UnknownPtr
str		r1,[r0]
pop		{r7}
mov		r8,r7
pop		{r4-r7}
pop		{r0}
bx		r0

goto_r3:
bx 		r3

goto_r4:
bx		r4

.align
MapSize:
.long 0x0202E4D4
CanMoveMap:
.long 0x202E4E0
OccupiedTileMap:
.long 0x202E4D8
UnknownMap:
.long 0x0202E4F0
UnknownPtr:
.long 0x030049A0
GetRangeBitfield:
@WriteRange:
