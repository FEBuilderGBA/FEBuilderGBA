.thumb
.org 0x0

@we have to calculate the growth manually
ldr		r0,Ptr1				@suspect this is the target queue, or something related to it
ldr		r1,[r7]
lsl		r2,r1,#0x2
add		r1,r1,r2
lsl		r1,r1,#0x2
add		r0,#0x4
add		r0,r0,r1
ldr		r0,[r0]				@should be battle struct
add		r0,#0x7A
ldrb	r0,[r0]
add		sp,#0x8
pop		{r7}
pop		{r1}
bx		r1

.align
Ptr1:
.long 0x0203E0FC
