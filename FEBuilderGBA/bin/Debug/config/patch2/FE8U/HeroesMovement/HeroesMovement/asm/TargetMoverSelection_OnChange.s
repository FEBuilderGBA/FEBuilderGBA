.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prGetTargetPosition, EALiterals+0x00

PivotSelection_OnChange:
	push {r4, lr}
	
	@ Saving Target Struct in r4
	mov r4, r1
	
	ldrb r0, [r4, #0]
	ldrb r1, [r4, #1]
	
	_blh prChangeActiveUnitFacing
	
	bl ClearRangeMoveMap
	_blh prMoveRange_HideGfx
	
	@ Loading Target Unit Struct
	ldrb r0, [r4, #2]
	_blh prUnit_GetStruct
	
	@ Loading Active Unit Struct
	ldr r3, =ppActiveUnit
	ldr r3, [r3]
	
	@ [r0, r1] = [active.x, active.y]
	ldrb r1, [r3, #0x10]
	ldrb r2, [r3, #0x11]
	
	@ Getting Target position in [r1, r2]
	ldr r3, prGetTargetPosition
	_blr r3
	
	ldr r3, =ppMoveMapRows
	ldr r3, [r3]
	
	@ Loading row
	lsl r2, #2 @ y*4
	add r3, r2
	ldr r3, [r3]
	
	add r3, r1 @ x
	mov r0, #0
	strb r0, [r3]
	
	mov r0, #1 @ &1 = Blue Move Display
	_blh prMoveRange_ShowGfx
	
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

ClearRangeMoveMap:
	push {lr}
	
	ldr r3, =ppMoveMapRows
	
	ldr r0, [r3]
	mov r1, #1
	neg r1, r1
	
	_blh prMap_Fill
	
	ldr r3, =ppRangeMapRows
	
	ldr r0, [r3]
	mov r1, #0
	
	_blh prMap_Fill
	
	pop {r0}
	bx r0

.ltorg
.align

EALiterals:
	@ POIN prGetTargetPosition
