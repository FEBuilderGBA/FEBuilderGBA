.thumb
.include "../_TargetSelectionDefinitions.s"
@A button case

.equ ActionID, 0x03

push 	{r4, r14}
mov 	 r4, r0

@check to make sure tile is selectable
ldr 	r0, =ppRangeMapRows
lsl 	r3, r2, #0x2
ldr 	r0, [r0]
ldr 	r0, [r0, r3]
ldrb	r0, [r0, r1]

cmp 	r0, #0
beq BadTile

@store cooridinates in action struct
ldr 	r0, =pActionStruct
mov 	r3, #0x0
strb 	r3, [r0, #0xD]
mov 	r3, #ActionID
strb 	r3, [r0,#0x11]
strb 	r1, [r0, #0x13]
strb 	r2, [r0, #0x14]

@ bl AoE_ClearGraphics

mov 	r0, #0x6
b End

BadTile:
mov 	r0, #0x10

End:
pop 	{r4}
pop 	{r3}
bx	r3
.ltorg
.align
OffsetList:
