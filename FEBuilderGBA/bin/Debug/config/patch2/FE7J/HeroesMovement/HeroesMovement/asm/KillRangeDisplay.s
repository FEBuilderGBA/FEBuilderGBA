.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

KillRangeDisplay:
	push {lr}

	bl ClearMoveMap
	_blh prMoveRange_HideGfx
	
	mov r0, #0
	
	pop {r1}
	bx r1

.ltorg
.align

ClearMoveMap:
	push {lr}
	
	ldr r3, =ppMoveMapRows
	
	ldr r0, [r3]
	mov r1, #1
	neg r1, r1
	
	_blh prMap_Fill
	
	pop {r0}
	bx r0

.ltorg
.align
