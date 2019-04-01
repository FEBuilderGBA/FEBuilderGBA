@Icecube's fix to properly display weapon range on the stats menu
@Paste at 16CC0
.thumb

.org 0xA240
Function1:

.org 0x16CC0
@r0 has item id/uses short
push	{r4-r7,r14}
mov		r7,#0x0
mov		r1,#0xFF
and		r0,r1
lsl		r1,r0,#0x3
add		r1,r1,r0
lsl		r1,r1,#0x2
ldr		r0,ItemTable
add		r1,r1,r0
ldrb	r6,[r1,#0x19]
ldr		r0,BlankTextID		@actually 3 spaces, not blank, but blank for our purposes
cmp		r6,#0x0
beq		GotoFunction1
cmp		r6,#0xFF
bne		NotTotal
sub		r0,#0x1				@'Total' text id 52A
GotoFunction1:
mov		r7,#0x1				@set flag
b		NotMagOver2
NotTotal:
lsl		r5,r6,#0x1C
lsr		r5,r5,#0x1C
cmp		r5,#0x0
bne		NotMagOver2
sub		r0,#0x9				@'MP/2' text id 522
mov		r7,#0x1				@set flag
NotMagOver2:
bl		Function1			@takes r0=text id as an argument, returns ram pointer to modify
cmp		r7,#0x1				@is flag set
beq		End
lsr		r4,r6,#0x4			@r4 has min, r5 has max
cmp		r5,r4
bne		DiffMinMax

mov		r1,#0x0
strb	r1,[r0,#0x6]
mov		r1,#0x1F
strb	r1,[r0,#0x5]
mov		r1,#0x7F
strb	r1,[r0,#0x2]
mov		r1,#0x20
strb	r1,[r0]
strb	r1,[r0,#0x1]
strb	r1,[r0,#0x2]
cmp		r4,#0xA
blt		NotDoubleDigits
sub		r4,#0xA
mov		r1,#0x31
NotDoubleDigits:
add		r4,#0x30
strb	r1,[r0,#0x3]
strb	r4,[r0,#0x4]
b		End
@<space> <space> <space> <tens digit> <ones digit> <(I think this means 'ignore this character')> <terminator>

DiffMinMax:
mov		r1,#0x0
strb	r1,[r0,#0x5]
mov		r1,#0x7F
strb	r1,[r0,#0x2]
cmp		r4,#0xA
blt		MinNotDoubleDigits
sub		r4,#0xA
mov		r1,#0x31
strb	r1,[r0]				@if tens digit is not there, 0x20 is already written, so we are ok		
MinNotDoubleDigits:
add		r4,#0x30
strb	r4,[r0,#0x1]
mov		r1,#0x1F
add		r5,#0x30
cmp		r5,#0x3A
blt		MaxNotDoubleDigits
sub		r5,#0xA
mov		r1,r5
mov		r5,#0x31
MaxNotDoubleDigits:
strb	r5,[r0,#0x3]
strb	r1,[r0,#0x4]
@<min tens digit> <min ones digit> <dash> <max tens digit> <max ones digit> <terminator> (if max doesn't have a tens digit, write the ones digit there and 1F to 0x4)

End:
pop		{r4-r7}
pop		{r1}
bx		r1

.align
BlankTextID:
.long 0x0000052B
ItemTable:
@.long 0x08B09B10
