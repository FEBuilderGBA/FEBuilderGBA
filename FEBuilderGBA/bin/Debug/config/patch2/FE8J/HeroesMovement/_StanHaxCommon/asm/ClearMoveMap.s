.thumb
.include "_Definitions.h.s"

ClearRangeAndMoveMap:
	push {lr}

	@ Filling Move map with -1
	@ ------------------------
	
	ldr r0, =ppMoveMapRows
	ldr r0, [r0]
	
	mov r1, #1
	neg r1, r1
	
	_blh prMap_Fill
	
	@ End
	@ ---
	
	pop {r0}
	bx r0

.ltorg
.align
