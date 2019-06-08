.thumb
.org 0x0

.set Growth_Options, New_Palettes+4
.set Get_Hp_Growth, Growth_Options+4
.set Get_Palette_Index, Get_Hp_Growth+4

@r0=thing to add to r8, r7 has the pointer with char ptr at 0xC, r8=bg0 buffer in wram
push	{r4,r5,r14}
mov		r4,r0
add		sp,#-0x4
ldr		r0,Write_Palette_Func
mov		r14,r0
ldr		r0,New_Palettes
mov		r1,#8
lsl		r1,#5
mov		r2,#0x40
.short	0xF800
mov		r5,#0
mov		r1,#0x3				@default palette index
ldr		r0,[r7,#0xC]
ldrb	r3,[r0,#0xB]
mov		r2,#0xC0
tst		r3,r2
bne		NotAllied
ldr		r2,Growth_Options
mov		r5,#0x10
and		r5,r2
ldr		r1,Get_Hp_Growth
mov		r14,r1
.short	0xF800
mov		r1,sp
mov		r2,r5
ldr		r3,Get_Palette_Index
mov		r14,r3
.short	0xF800
mov		r1,r0
NotAllied:
ldr		r0,ProcessHP
mov		r14,r0
mov		r0,r4
add		r0,r8
mov		r2,#0x22
mov		r3,#0x23
.short	0xF800
cmp		r5,#0
beq		NoShenanigans		@don't need to restore the original tile id
ldr		r0,[sp]
cmp		r0,#0
beq		NoShenanigans
ldr		r1,Const_2028E70
ldr		r1,[r1]
strh	r0,[r1,#0x10]
NoShenanigans:
add		sp,#0x4
pop		{r4,r5}
pop		{r0}
bx		r0


.align
Write_Palette_Func:
.long 0x08000DB8
ProcessHP:
.long 0x08004D5C
Const_2028E70:
.long 0x02028E70
New_Palettes:
@
