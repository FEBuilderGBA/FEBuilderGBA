.thumb
.org 0x2 @if reassembling this, don't forget to remove the two 00 bytes in the beginning; otherwise EA will mess up the write

@push 	{r14}
@bl 	Range_Write
ldr 	r0,thing
ldr 	r3,Place
bl 		goto_r3
pop 	{r0}
bx 		r0

goto_r3:
bx		r3

.align
thing:
.long 0x08025794+1
Place:
.long 0x08024F18+1
