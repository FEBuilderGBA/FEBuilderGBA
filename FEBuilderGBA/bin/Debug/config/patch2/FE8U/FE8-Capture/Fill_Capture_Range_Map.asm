.thumb
.org 0x0

@r0=char data
@based on the heal staff fill-in-range function at 25E7C
push	{r4-r5,r14}
mov		r4,#0x10
ldsb	r4,[r0,r4]
mov		r5,#0x11
ldsb	r5,[r0,r5]
ldr		r1,Const1
str		r0,[r1]
ldr		r0,Clear_Map_Func
mov		r14,r0
ldr		r0,RangeMap
ldr		r0,[r0]
mov		r1,#0x0
.short	0xF800
ldr		r0,Fill_One_Range_Map		@1-1 range stuff
mov		r14,r0
mov		r0,r4
mov		r1,r5
ldr		r2,Capture_Target_Check
.short	0xF800
pop		{r4-r5}
pop		{r0}
bx		r0

.align
Const1:
.long 0x02033F3C
Clear_Map_Func:
.long 0x080197E4
RangeMap:
.long 0x0202E4E4
Fill_One_Range_Map:
.long 0x08024F70
Capture_Target_Check:
@.long 0x0802517C+1
