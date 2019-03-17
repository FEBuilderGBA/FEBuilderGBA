.thumb
.org 0x0
@r4 has min nibble, r3 = r2 = max nibble
push	{r14}
lsl		r1,r4,#0x4
orr		r1,r2
cmp		r1,#0xFF
bne		NotTotal
mov		r4,#0x1
mov		r2,r1
b		GoBack
NotTotal:
cmp		r2,#0x0
bne		GoBack
ldr		r0,CurrentCharPtr
ldr		r0,[r0]
ldr		r3,GetMagOver2
bl		goto_r3
mov		r2,r0
GoBack:
pop		{r0}
bx		r0

goto_r3:
bx		r3

.align
CurrentCharPtr:
.long 0x03004E50
GetMagOver2:
.long 0x08018A1C+1
