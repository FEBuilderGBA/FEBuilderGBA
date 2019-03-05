.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

SwapSelection_OnChange:
	push {r4, lr}
	
	ldrb r0, [r1, #0]
	ldrb r1, [r1, #1]
	
	_blh prChangeActiveUnitFacing
	
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ no sir
