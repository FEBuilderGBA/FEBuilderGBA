.thumb
.org 0x2 @if reassembling this, don't forget to remove the two 00 bytes in the beginning; otherwise EA will mess up the write

@push 	{r14}
@bl 	Range_Write		@writes range, returns item id in r0
ldr		r1,SomeTable
FindPtr:
ldrb	r2,[r1]			@load item id from entry
cmp		r2,r0
beq		PtrFound
cmp		r2,#0x0
beq		PtrFound		@last entry will use the heal pointer, just to make sure the game doesn't crash
add		r1,#0x8
b		FindPtr
PtrFound:
ldr		r0,[r1,#0x4]
ldr 	r3,Place
bl 		goto_r3
pop 	{r0}
bx 		r0

goto_r3:
bx		r3

.align
Place:
.long 0x08024EAC+1
SomeTable:
