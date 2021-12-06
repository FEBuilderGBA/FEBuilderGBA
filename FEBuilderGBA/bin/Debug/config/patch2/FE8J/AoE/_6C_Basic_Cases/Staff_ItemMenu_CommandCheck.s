.thumb
.include "../_TargetSelectionDefinitions.s"

@arguments:
	@r0 = Pointer to Command definition
	@r1 = Command definition index
	@r2 = Pointer to Command Condition
push 	{r4-r5, lr}
mov 	r4, r1
mov 	r5, r2
ldr 	r0, =pActionStruct
ldrb 	r0, [r0, #0xD]
_blh GetUnit
mov 	r2, r0
lsl 	r1, r4, #0x1
add 	r0, #0x1E
add 	r0, r0, r1
ldrh 	r0, [r0]
cmp 	r0, #0x0
bne CommandCheck
mov 	r0, #0x3
b End
CommandCheck:
@cmp 	r5, #0x0
@beq CommandUsable
mov 	r1, r2
_blr r5
cmp 	r0, #0x0
bne CommandUsable
mov 	r0, #0x2
b End
CommandUsable:
mov 	r0, #0x1
End:
pop 	{r4-r5}
pop 	{r3}
bx r3

.align
.ltorg
OffsetList:
