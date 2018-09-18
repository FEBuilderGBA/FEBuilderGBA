.thumb
push    	{r4-r6,r14}
mov     	r5,r0
mov     	r6,r1
ldr     	r4,ChapterState
ldrh    	r0,[r4,#0x10]
cmp     	r0,#0x1				@Is it turn 1?
bne     	OptionCheck
bl			FindFirstDeployed
mov     	r1,r0
ldrb    	r0,[r1,#0x10]
strb    	r0,[r4,#0x12]
ldrb    	r0,[r1,#0x11]
strb    	r0,[r4,#0x13]
OptionCheck:
mov     	r0,r4
add     	r0,#0x40
ldrb    	r0,[r0]
lsl     	r0,r0,#0x1B
cmp     	r0,#0x0				@Is autocursor set?
blt     	CoordStore		
bl      	FindFirstDeployed	@If so, find first unit in party
mov     	r1,r0
mov     	r0,#0x10
ldsb    	r0,[r1,r0]
str     	r0,[r5]
mov     	r0,#0x11
ldsb    	r0,[r1,r0]
b       	CoordStore2
CoordStore:
ldrb    	r0,[r4,#0x12]
str     	r0,[r5]
ldrb    	r0,[r4,#0x13]
CoordStore2:
str     	r0,[r6]
pop     	{r4-r6}
pop     	{r0}
bx      	r0

.align 2
FindFirstDeployed:
push		{r3,r4}
ldr			r2,DeploymentPointers
mov			r4,#0x1
LoopStart:
lsl			r1,r4,#0x2
ldr			r3,[r1,r2]
ldr			r1,[r3]				@Check if unit doesn't exist
cmp			r1,#0x0
beq			LoopEnd
ldr			r1,[r3,#0xC]
mov			r0,#0x1
lsl			r0,r0,#0x10
add			r0,#0xC				@check if unit is deployed
and			r0,r1
cmp			r0,#0x0
bne			LoopEnd
mov			r0,#0x10
ldsb		r0,[r3,r0]
cmp			r0,#0x0
blt			LoopEnd				@Check if unit is off the map
mov			r0,r3
b			DeploymentReturn
LoopEnd:
add			r4,#0x1
b			LoopStart
DeploymentReturn:
pop			{r3,r4}
bx			r14


.align 2
ChapterState:
.long 0x0202BBF8
DeploymentPointers:
.long 0x08B92EB0
