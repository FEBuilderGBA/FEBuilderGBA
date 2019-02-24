.thumb

.include "../../_CommonASM/_Definitions.h.s"

.set prMakeShoveTargetList,           EALiterals+0x00
.set pShoveTargetSelectionDefinition, EALiterals+0x04
	
ShoveEffect:
	push {lr}
	
	@ Loading Active Unit
	ldr r3, =ppActiveUnit
	ldr r0, [r3]

	@ Making Target List
	ldr r3, prMakeShoveTargetList
	_blr r3
	
	@ Making Target Selection 6C
	ldr r0, pShoveTargetSelectionDefinition
	_blh prTargetSelection_New
	
	@ 0x01 = ???, 0x02 = Kill Menu, 0x04 = Beep Sound, 0x10 = Clear Menu Gfx
	mov r0, #0x17
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prMakeShoveTargetList
	@ POIN pShoveTargetSelectionDefinition
