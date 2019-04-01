.thumb
.org 0x0

@r0=active char data pointer, r1 has 1 (which we will negate)
@I have no idea why this even exists
push	{r14}
mov		r7,r8
push	{r7}
neg		r1,r1
ldr		r3,ReturnRangeBitfield
bl		goto_r3
mov		r7,r0
mov		r8,r1
ldr		r0,RangeMapPtr
ldr		r0,[r0]
mov		r1,#0x0
ldr		r3,ClearRangeMap
bl		goto_r3
mov		r5,#0x81
ldr		r6,Pointer1
LoopThroughEnemies:
mov		r5,r0
ldr		r3,GetCharData
bl		goto_r3
cmp		r0,#0x0
beq		GetNextEnemy
ldr		r1,[r0]
cmp		r1,#0x0
beq		GetNextEnemy
mov		r4,r0
mov		r0,#0x10
ldsb	r0,[r4,r0]
mov		r1,#0x11
ldsb	r1,[r4,r1]
mov		r2,#0x0
mov		r12,r2
mov		r2,r7
mov		r3,r8
push	{r4}
ldr		r4,ReturnRangeBitfield+4		@actually WriteRange function
bl		goto_r4
pop		{r4}
ldrb	r0,[r4,#0x10]
strb	r0,[r6,#0x13]
ldrb	r0,[r4,#0x11]
strb	r0,[r6,#0x14]					@yes, we're effectively overwriting the coordinates every loop. No, I do not know why.
GetNextEnemy:
add		r5,#0x1
cmp		r5,#0xBF
ble		LoopThroughEnemies
pop		{r7}
mov		r8,r7
pop		{r0}
bx		r0

goto_r3:
bx		r3

goto_r4:
bx		r4

.align
RangeMapPtr:
.long 0x0202E4F0
Pointer1:
.long 0x0203A958

ClearRangeMap:
.long 0x080197E4+1
GetCharData:
.long 0x08019430+1
ReturnRangeBitfield:
