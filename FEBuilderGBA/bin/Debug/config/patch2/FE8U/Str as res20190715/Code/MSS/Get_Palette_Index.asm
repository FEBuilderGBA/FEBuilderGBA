.thumb
.org 0x0

Get_Palette_Index:			@bl'd to in the draw_textID_at macro
@r0=growth, r1=stack pointer to keep the old value at [2028E70]+10 (a hack-y way of using a different palette bank), r2=0 if don't make colors
@table takes form of (number, palette index of stat if growth < number), terminated with 00
@essentially, this function returns a # 0-4, which corresponds to a chunk of a palette bank. If we're using a different palette bank than normal (0), we temporarily change that

push	{r4-r5,r14}
mov		r3,#3
cmp		r2,#0
beq		GetPaletteIndexEnd
ldr		r2,Growth_Colors_Table
ColorLoop:
ldrb	r3,[r2]
cmp		r0,r3
blt		FoundColor
cmp		r3,#0
beq		NotFoundColor
add		r2,#2
bne		ColorLoop
NotFoundColor:
mov		r3,#0x4			@if growth is higher than highest value in table, use glowy green
b		Label1
FoundColor:
ldrb	r3,[r2,#0x1]
Label1:
mov		r2,#0
DivideByFiveLoop:
cmp		r3,#5
blt		GotPaletteBank
sub		r3,#5
add		r2,#1
b		DivideByFiveLoop
GotPaletteBank:
cmp		r2,#0
bne		PaletteIndexShenanigans	
str		r2,[r1]				@no shenanigans required if we're using palette bank 0
b		GetPaletteIndexEnd
PaletteIndexShenanigans:
ldr		r4,Const1_2028E70
ldr		r4,[r4]
ldrh	r0,[r4,#0x10]
str		r0,[r1]				@storing the old value on the stack; will be changed back after writing the text tiles to bg buffer
add		r2,#7				@palette banks we'll be using are 8 and 9 (A if necessary, but I think 10 new colors should be enough)
lsl		r2,#0xC
add		r0,r2				@not entirely sure how this works, but I think this is the first tile of the text in question, so we're just tacking on a new palette bank to it
strh	r0,[r4,#0x10]
GetPaletteIndexEnd:
mov		r0,r3
pop		{r4-r5}
pop		{r1}
bx		r1

.align
Const1_2028E70:
.long 0x02028E70
Growth_Colors_Table:
@
