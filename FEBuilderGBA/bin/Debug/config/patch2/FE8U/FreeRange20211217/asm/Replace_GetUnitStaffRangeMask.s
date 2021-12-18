.thumb
.include "_Definitions.h.s"

@ Arguments: r0 = Unit Struct, r1 = Item Slot Index (-1 for all)
@ .org 0x016FE4
Replace_GetUnitStaffRangeMask:
	push {r4-r6, lr}
	
	cmp r1, #0
	blt HandleAllItems
	
	lsl r1, #1 @ r1 = slot * sizeof(item)
	add r1, #0x1E @ Item Array Start + slot * sizeof(item)
	
	ldrh r1, [r0, r1] @ r1 = Item Short
	
	bl GetStaffRangeMaskForUnit
	b End
	
HandleAllItems:
	mov r4, r0
	
	mov r5, #0 @ i
	mov r6, #0 @ Current Mask
	
ContinueItemLoop:
	lsl r1, r5, #1
	add r1, #0x1E
	
	ldrh r1, [r4, r1]
	
	cmp r1, #0
	beq EndItemLoop
	
	mov r0, r4
	bl GetStaffRangeMaskForUnit
	
	orr r6, r0
	
	add r5, #1
	cmp r5, #5
	blt ContinueItemLoop
	
EndItemLoop:
	mov r0, r6

End:
	pop {r4-r6}
	
	pop {r1}
	bx r1

.ltorg

@ Arguments: r0 = Unit Struct, r1 = Item
GetStaffRangeMaskForUnit:
	push {r4-r5, lr}
	
	mov r4, r0
	mov r5, r1
	
	_blh CanUnitUseItem
	
	cmp r0, #0
	beq GetStaffRangeMaskForUnit_Zero
	
	mov r0, r5
	_blh GetItemMaxRange
	
	cmp r0, #0
	bne GetStaffRangeMaskForUnit_Std
	
	mov r0, r4
	_blh GetUnitMagBy2Range
	
	add r0, #1
	
	mov r1, #1
	lsl r1, r0
	sub r1, #1
	
	mov r4, r1
	
	mov r0, r5
	_blh GetItemMinRange
	
	mov r1, #1
	lsl r1, r0
	sub r0, r1, #1
	
	eor r0, r4
	
	b GetStaffRangeMaskForUnit_End
	
GetStaffRangeMaskForUnit_Std:
	mov r0, r5
	_blh GetWeaponRangeMask
	
	b GetStaffRangeMaskForUnit_End
	
GetStaffRangeMaskForUnit_Zero:
	mov r0, #0

GetStaffRangeMaskForUnit_End:
	pop {r4-r5, pc}

.ltorg
.align

EALiterals:
	@ notin
