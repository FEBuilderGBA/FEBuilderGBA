.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prGetTargetPosition, EALiterals+0x00
.set prClearRangeMovMaps, EALiterals+0x04
.set ACTION_PUSH,         EALiterals+0x08

ShoveSelection_OnSelection:
	push {r4, lr}
	
	@ r4 = Target Struct
	mov r4, r1
	
	ldr r3, prClearRangeMovMaps
	_blr r3
	
	_blh prMoveRange_HideGfx
	
	@ LOADING STUFF FROM UNIT STRUCT
	@ ------------------------------
	
	@ Getting Target Struct
	ldrb r0, [r4, #2]
	_blh prUnit_GetStruct
	
	@ Getting Active Struct
	ldr r3, =ppActiveUnit
	ldr r3, [r3]
	
	@ [r0, r1] = source = [active.x, active.y]
	ldrb r1, [r3, #0x10]
	ldrb r2, [r3, #0x11]
	
	@ Getting Target position in [r1, r2]
	ldr r3, prGetTargetPosition
	_blr r3
	
	@ r0 = target unit index
	ldrb r0, [r4, #2]
	
	@ SAVING STUFF TO ACTION STRUCT
	@ -----------------------------
	
	ldr r3, =pActionStruct
	
	strb r0, [r3, #0x0D] @ Target Unit
	strb r1, [r3, #0x13] @ Action xTarget
	strb r2, [r3, #0x14] @ Action yTarget
	
	ldr r0, ACTION_PUSH
	strb r0, [r3, #0x11] @ Action Index
	
	@ 0x02 = Kill Unit Selection, 0x04 = Beep Sound, 0x10 = Clear Unit Selection Gfx
	mov r0, #0x16
	
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:

