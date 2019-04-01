.thumb
.org 0x0

@this function clears the Capturing bit in the current unit's turn word, regardless of whether it's set or not. Called when being forced to redraw the menu (I think).
@r0, r2 are busy
ldr		r3,CurrentCharPtr
ldr		r3,[r3]
ldr		r1,[r3,#0xC]
push	{r2}
mov		r2,#0x80
lsl		r2,#0x17
mvn		r2,r2
and		r1,r2
str		r1,[r3,#0xC]
pop		{r2}
mov		r3,#0x1C
ldsh	r1,[r2,r3]
mov		r3,#0xC
ldsh	r2,[r2,r3]
sub		r1,r1,r2
mov		r2,#0x1
bx		r14

.align
CurrentCharPtr:
.long 0x03004E50
