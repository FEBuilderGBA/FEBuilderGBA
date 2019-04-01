.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prGetTargetPosition,   EALiterals+0x00
.set ACTION_MOVEACTIVEUNIT, EALiterals+0x04

PivotSelection_OnSelection:
	push {r4, lr}
	
	@ r4 = Target Struct
	mov r4, r1
	
	bl ClearRangeMoveMap
	_blh prMoveRange_HideGfx
	
	@ LOADING STUFF FROM UNIT STRUCT
	@ ------------------------------
	
	ldr r3, =ppActiveUnit
	ldr r0, [r3]
	
	@ [r0, r1] = [target.x, target.y]
	ldrb r1, [r4, #0]
	ldrb r2, [r4, #1]
	
	@ Getting Target position in [r1, r2]
	ldr r3, prGetTargetPosition
	_blr r3
	
	@ Shifting pos registers
	mov r0, r1
	mov r1, r2
	
	@ r2 = target unit index (actually only used for animation)
	ldrb r2, [r4, #2]
	
	@ SAVING STUFF TO ACTION STRUCT
	@ -----------------------------
	
	ldr r3, =pActionStruct
	
	strb r0, [r3, #0x0E] @ Action xMove
	strb r1, [r3, #0x0F] @ Action yMove
	strb r2, [r3, #0x0D] @ Target Unit
	
	ldr r2, ACTION_MOVEACTIVEUNIT
	strb r2, [r3, #0x11] @ Action Index
	
	@ 0x02 = Kill Unit Selection, 0x04 = Beep Sound, 0x10 = Clear Unit Selection Gfx
	mov r0, #0x16
	
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
	@ WORD ACTION_MOVEACTIVEUNIT
