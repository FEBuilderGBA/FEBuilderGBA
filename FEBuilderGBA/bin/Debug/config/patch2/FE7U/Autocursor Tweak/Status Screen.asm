.thumb
push		{r4-r5,r14}
ldr			r2,DeploymentPointers
mov			r4,#0x1
LoopStart:
lsl			r5,r4,#0x2			
ldr			r5,[r5,r2]
ldr			r1,[r5]
cmp			r1,#0x0
beq			LoopEnd
ldr			r1,[r5,#0xC]
mov			r0,#0x1
lsl			r0,r0,#0x10
add			r0,#0xC		@check if unit is deployed and present; bits 0x1000C
and			r1,r0
cmp			r1,#0x0
bne			LoopEnd
mov			r0,#0x10
ldsb		r0,[r5,r0]
cmp			r0,#0x0
blt			LoopEnd
mov			r0,r5
b			Return
LoopEnd:
add			r4,#0x1
cmp			r4,#0x3F
ble			LoopStart
mov			r0,#0x0
Return:
pop			{r4,r5}
pop			{r1}
bx			r1

.align 2
DeploymentPointers:
.long 0x08B92EB0
