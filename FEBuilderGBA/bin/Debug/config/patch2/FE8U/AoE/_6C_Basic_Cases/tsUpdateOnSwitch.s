.thumb
.include "../_TargetSelectionDefinitions.s"

.equ UpdateItemBox, OffsetList + 0x0
.equ ICondition, OffsetList + 0x4

push 	{r4, lr}
mov 	r4, r1
mov 	r0, #0x0
ldsb 	r0, [r4, r0]
mov 	r1, #0x1
ldsb 	r1, [r4, r1]
_blh 	0x801F50C	@{U}
@_blh 	0x801F164	@{J}
mov 	r0, #0x2
ldsb 	r0, [r4, r0]
_blh GetUnit
ldr 	r1, ICondition
ldr 	r3, UpdateItemBox
_blr 	r3
pop 	{r4}
pop 	{r1}
bx 	r1
.align
.ltorg
OffsetList:
