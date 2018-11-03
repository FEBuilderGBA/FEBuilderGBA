@thumb

;this function clears the Capturing bit in the current unit's turn word, regardless of whether it's set or not. Called when being forced to redraw the menu (I think).
;r0, r2 are busy
ldr		r3,=$03004df0
ldr		r3,[r3]
ldr		r1,[r3,#0xC]
push	{r2}
mov		r2,#0x80
lsl		r2, r2,#0x17
tst	r1, r2
beq	non

mvn		r2,r2
and		r1,r2
mov		r2,#0x10
mvn		r2,r2
and		r1,r2
str		r1,[r3,#0xC]
non:
pop		{r2}
mov		r3,#0x1C
ldsh	r1,[r2,r3]
mov		r3,#0xC
ldsh	r2,[r2,r3]
ldr	r3, =$08022848
mov	pc, r3


