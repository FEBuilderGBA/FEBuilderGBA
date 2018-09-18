.thumb
.include "_Definitions.h.s"

.set prFillRangeByMask, EALiterals+0x00

@ .org 0x01ACBC
Replace_FillReachRangeForUnit:
	push {r4-r7, lr}
	
	mov r4, r0
	
	mov r1, #1
	neg r1, r1
	
	_blh prUnit_GetRangeMap
	
	mov r5, r0
	
	mov r6, #0 @ yIt
	mov r7, #0 @ xIt
	
	ldr  r0, =pCurrentMapSize
	ldrh r6, [r0, #2] @ r6 = yIt
	
	sub r6, #1
	
StartLoop_Y:
	ldr  r0, =pCurrentMapSize
	ldrh r7, [r0] @ r7 = xIt
	
	sub r7, #1

StartLoop_X:
	lsl r2, r6, #2 @ r2 = yIt * sizeof(pointer)
	
	ldr  r0, =ppMoveMapRows
	ldr  r0, [r0]
	ldr  r0, [r0, r2]
	ldrb r0, [r0, r7]
	
	cmp r0, #120
	bhi ContinueLoop_X
	
	ldr  r0, =ppUnitMapRows
	ldr  r0, [r0]
	ldr  r0, [r0, r2]
	ldrb r0, [r0, r7]
	
	cmp r0, #0
	bne ContinueLoop_X
	
	ldr  r0, =ppOtherMoveMapRows
	ldr  r0, [r0]
	ldr  r0, [r0, r2]
	ldrb r0, [r0, r7]
	
	cmp r0, #0
	bne ContinueLoop_X
	
	@ (r0, r1) = (char, class)
	ldr r0, [r4]
	ldr r1, [r4, #4]
	
	@ (r0, r1) = (char.attr, class.attr)
	ldr r0, [r0, #0x28]
	ldr r1, [r1, #0x28]
	
	@ r0 = Unit Attributes
	orr r0, r1
	
	mov r1, #0x80 @ Ballista Flag
	tst r0, r1
	beq HandleStandard
	
	@ Ballista Handling
	
	mov r0, r7
	mov r1, r6
	
	_blh prGetBallistaItemAt
	
	cmp r0, #0
	beq HandleStandard @ No Ballista
	
	_blh prItem_GetRangeMask
	
	mov r2, r0
	orr r2, r5
	
	b FillRange
	
HandleStandard:
	mov r2, r5
	
FillRange:
	mov r0, r7
	mov r1, r6
	
	ldr r3, prFillRangeByMask
	_blr r3
	
ContinueLoop_X:
	sub r7, #1
	bge StartLoop_X

ContinueLoop_Y:
	sub r6, #1
	bge StartLoop_Y
	
	ldr r1, =ppMoveMapRows
	ldr r1, [r1]
	
	ldr r0, =#0x030049A0 @ Subject Map?
	str r1, [r0]
	
	pop {r4-r7}
	
	pop {r0}
	bx r0

.ltorg
.align

EALiterals:
	@ POIN prFillRangeByMask
