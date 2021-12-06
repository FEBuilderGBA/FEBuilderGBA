.thumb
.include "../_TargetSelectionDefinitions.s"

push 	{r4, lr}
mov 	r4, r0
ldr 	r3, =pGameDataStruct
ldr 	r2, =ppRangeMapRows
ldr 	r2, [r2]
ldrh 	r0, [r3, #0x16]
lsl 	r0, #0x2
add 	r1, r0, r2
ldr 	r1, [r1]
ldrh 	r0, [r3, #0x14]
add 	r1, r1, r0
ldrb 	r0, [r1]
cmp 	r0, #0x0
beq NullCursor
mov 	r0, #0x20
b End
NullCursor:
mov 	r0, #0x40
End:
pop 	{r4}
pop 	{r3}
bx r3
.align
