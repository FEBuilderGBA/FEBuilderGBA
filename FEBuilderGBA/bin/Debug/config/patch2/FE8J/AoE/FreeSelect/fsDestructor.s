.thumb
.include "../_TargetSelectionDefinitions.s"

FreeSelect6C_Destructor:
	push {lr}
	
	@mov r4, r0
	ldr r0, [r4, #0x30]
	_blh TCS_Free
	
	_blh UnlockGameLogic
	
bl AoE_ClearGraphics
	
	@pop {r4}
	pop {r3}
BXR3:
	bx r3

.ltorg
.align

EALiterals:
	@ noting
