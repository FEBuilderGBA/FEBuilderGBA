.thumb
.org 0x0

@r0 = char data pointer, r1 = item id
push	{r4-r7,r14}
mov		r7,r8
push	{r7}
mov		r7,r0
mov		r6,#0xFF
and		r6,r1			@item id
mov		r8,r6
lsl		r1,r6,#0x3
add		r1,r1,r6
lsl		r1,r1,#0x2
ldr		r6,Item_Table
add		r1,r1,r6
ldrb	r6,[r1,#0x19]	@range byte
mov		r4,#0x10
ldsb	r4,[r0,r4]		@x coord
mov		r5,#0x11
ldsb	r5,[r0,r5]		@y coord
ldr		r1,Pointer1
str		r0,[r1]
ldr		r0,Pointer2		@points to beginning of table of row pointers for range map
ldr		r0,[r0]
mov		r1,#0x0
ldr		r3,Function1
bl		goto_r3			@zeroes out the range map
mov		r0,r4
mov		r1,r5
ldr		r3,Function2
bl		goto_r3			@stores coordinates at 204DCF4 and 0x00000000 at 203DFF8
mov 	r3,#0x1
mov		r0,r6
mov		r1,r7
bl		Get_Range_Nibble	@returns max range nibble
mov		r2,r0
mov		r0,r4
mov		r1,r5
push	{r4}
ldr		r4,Function4
bl		goto_r4			@fills in the range map
pop		{r4}
mov		r3,#0x1
neg		r3,r3
mov		r0,r6
bl		Get_Range_Nibble	@returns min range nibble
mov		r2,r0
mov		r0,r4
mov		r1,r5
push	{r4}
ldr		r4,Function4
bl		goto_r4			@clears the range map of unreacheable tiles
pop		{r4}
mov		r0,r8
pop		{r7}
mov		r8,r7
pop		{r4-r7}
pop		{r1}
bx		r1

Get_Range_Nibble:
push	{r14}
cmp		r3,#0x1
beq		MaxNibble
cmp		r0,#0xFF
beq		ReturnZero
lsr		r0,r0,#0x4
sub		r0,#0x1
b		GoBack
ReturnZero:
mov		r0,#0x0
b		GoBack
MaxNibble:
cmp		r0,#0xFF
beq		GoBack
mov		r2,#0xF
and		r0,r2
cmp		r0,#0x0
bne		GoBack
mov		r0,r1
ldr		r3,Function3
bl		goto_r3			@given char data, returns mag/2 range
mov		r3,#0x1
GoBack:
pop		{r1}
bx		r1

goto_r3:
bx		r3
goto_r4:
bx		r4

.align
Pointer1:
.long 0x02033F3C
Pointer2:
.long 0x0202E4E4
Function1:
.long 0x080197E4+1
Function2:
.long 0x0804F8A4+1
Function3:
.long 0x08018A1C+1
Function4:
.long 0x0801AABC+1
Item_Table:

