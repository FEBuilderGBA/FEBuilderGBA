.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prMakeTargetList,          EALiterals+0x00
.set pTargetSelectorDefinition, EALiterals+0x04

TargetCommandEffect:
	push {lr}
	
	@ Loading Active Unit
	ldr r3, =ppActiveUnit
	ldr r0, [r3]

	@ Making Target List
	ldr r3, prMakeTargetList
	_blr r3
	
	@ Making Target Selection 6C
	ldr r0, pTargetSelectorDefinition
	_blh prTargetSelection_New
	
	@ 0x01 = ???, 0x02 = Kill Menu, 0x04 = Beep Sound, 0x10 = Clear Menu Gfx
	mov r0, #0x17
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prMakeTargetList
	@ POIN pTargetSelectorDefinition
