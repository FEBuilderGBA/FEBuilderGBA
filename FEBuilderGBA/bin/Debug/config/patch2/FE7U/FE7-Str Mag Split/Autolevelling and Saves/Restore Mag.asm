.thumb
.org 0x0

push	{r5-r7,r14}
mov		r7,r1
mov		r2,#0xC0
ldr		r3,[r0]
mov		r0,r4
bl		goto_r3
mov		r5,#0x0
ldr		r6,GetCharStruct
Loop:
mov		r0,r5
mov		r3,r6
bl		goto_r3
add		r0,#0x39
ldrb	r1,[r7,r5]
strb	r1,[r0]
add		r5,#0x1
cmp		r5,#0xBF
ble		Loop
pop		{r5-r7}
pop		{r0}
bx		r0

goto_r3:
bx		r3

.align
GetCharStruct:
.long 0x08018D0C+1
