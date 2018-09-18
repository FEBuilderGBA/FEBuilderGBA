@thumb
@org				0x080C6988

lsl	r3,	r3,	#0x10
lsr	r3,	r3,	#0x18
lsl	r3,	r3,	#0x2
@-------------------------------@F782
@-------------------------------@F819
bl				0x0804DD50
cmp	r0,	#0x1
@-------------------------------@D100
bne				LOOP
mov	r3,	#0x0
LOOP:
add	r3,	r3,	#0x4
ldr	r0,	[r2,	#0x20]
sub	r0,	r0,	r3	@1AC0
str	r0,	[r2,	#0x20]
mov	r0,	r4		@1C20
pop	{r4, r5, pc}
