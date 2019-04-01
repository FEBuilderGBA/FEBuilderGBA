.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set textBottomHelp, EALiterals+0x00

TargetSelection_OnConstruction:
	push {r4, lr}
	
	@ Saving 6C in r4
	mov r4, r0
	
	ldr r0, textBottomHelp
	
	cmp r0, #0
	beq SkipBottomHelpText
	
	_blh prGetTextInBuffer
	
	mov r1, r0
	mov r0, r4
	
	_blh prBottomHelpDisplay_New
	
SkipBottomHelpText:
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ WORD textBottomHelp
