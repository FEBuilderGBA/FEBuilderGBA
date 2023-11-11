.thumb

push	{r14}
mov		r1,sp
ldmia	r0!,[r2,r3,r5]
stmia	r1!,[r2,r3,r5]
ldr		r0,[r0]
str		r0,[r1]
mov		r5,#0
str		r5,[sp,#0x14]		@0x10+4 because of push
add		r0,sp,#0x14
ldr		r1,=#0x6001400

ACTUALLY THIS MAY NOT BE NECESSARY