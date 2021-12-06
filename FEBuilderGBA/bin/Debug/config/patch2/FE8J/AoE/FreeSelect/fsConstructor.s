.thumb
.include "../_TargetSelectionDefinitions.s"
.equ Proc_FreeSelect, PointerList + 0x0
@Free Select Constructor
@parameters:
	@r0=FS6CPointerList
push	{r4-r5,lr}
mov 	r4, r0
ldr 	r0, Proc_FreeSelect
mov 	r1, #0x3
_blh New6C
str 	r4, [r0, #0x2C]
mov 	r5, r0
@call specialized constructor if it exists
ldr 	r3, [r4]
cmp 	r3, #0x0
beq End
bl Jump
End:
mov 	r0, r5
pop 	{r4-r5}
pop 	{r3}
Jump:
bx	r3
.ltorg
.align

PointerList:
