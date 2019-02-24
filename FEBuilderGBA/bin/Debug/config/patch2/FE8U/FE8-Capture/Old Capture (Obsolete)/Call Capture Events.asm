.thumb
.org 0x0

@atm, just make everything drop. Afterwards, check for capture first; if false, check for drop last item flag
@r5=attacker char struct, r7=defender char struct, r8=funky struct for 'drop last item'
@slots: 2=recipient char number, 3=current item, 4=pointer to inventory slot of defender (0 if done), 5=counter
ldr		r0,[r5,#0xC]
mov		r1,#0x2
lsl		r2,r1,#0x18
tst		r0,r2
bne		CheckCapture
ldrb	r0,[r7,#0xD]
mov		r1,#0x10		@drop last item flag
and		r0,r1
ldr		r1,DropLastItem
bx		r1

.align
DropLastItem:
.long 0x08032934+1

CheckCapture:
mvn		r2,r2
and		r0,r2
str		r0,[r5,#0xC]
add		r7,#0x1E
ldrb	r0,[r7]
cmp		r0,#0x0
beq		GoBack			@don't do anything if there is no loot to be had
ldr		r0,MemorySlot
ldr		r1,[r5,#0x4]
ldrb	r1,[r1,#0x4]
str		r1,[r0,#0x8]	@recipient char num goes in slot 2
str		r7,[r0,#0x10]
mov		r6,#0x0
mov		r5,r7
ReplacementLoop:
ldrb	r4,[r5]
cmp		r4,#0x0
beq		CallEvents
mov		r0,r4
bl		CheckForSubstitution	@returns the item to sub out, or the original item if no sub
cmp		r0,r4
beq		NoChange
strh	r0,[r5]
NoChange:
add		r6,#0x1
add		r5,#0x2
cmp		r6,#0x4
ble		ReplacementLoop
CallEvents:
ldr		r0,ExecuteEvent
mov		r14,r0
ldr		r0,EventOffset
mov		r1,#0x0
.short	0xF800
GoBack:
mov		r0,#0x1			@might need to be 0, not sure
pop		{r7}
mov		r8,r7
pop		{r4-r7}
pop		{r1}
bx		r1

CheckForSubstitution:
push	{r4,r5,r14}
ldr		r4,EventOffset+4	@the list of substitutions
SubLoop:
ldrb	r5,[r4]
cmp		r5,r0
bne		Label1
ldrb	r0,[r4,#0x1]
b		RetSubstitution
Label1:
add		r4,#0x2
cmp		r5,#0x0
bne		SubLoop
RetSubstitution:
pop		{r4-r5}
pop		{r1}
bx		r1

.align
MemorySlot:
.long 0x030004B8
ExecuteEvent:
.long 0x0800D07C
EventOffset:
