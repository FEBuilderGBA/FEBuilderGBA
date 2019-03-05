.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prGetPushPosition, EALiterals+0x00
.set prIsTargetLegal,   EALiterals+0x04

Shove_TargetCheck:
	push {r4, lr}
	
	@ r4 = Target Unit Struct
	mov r4, r0
	
	@ r3 = legality check routine ptr
	ldr r3, prIsTargetLegal
	
	@ if no legality check has been set, skip
	cmp r3, #0
	beq SkipLegalCheck
	
	@ call
	_blr r3
	
	cmp r0, #0
	beq End @ Returns 0 since r0 is 0
	
SkipLegalCheck:
	@ Loading active unit
	ldr r3, =ppActiveUnit
	ldr r3, [r3]
	
	@ Loading active unit position
	ldrb r1, [r3, #0x10] @ ARG r1 = source x
	ldrb r2, [r3, #0x11] @ ARG r2 = source y
	
	@ Getting potential target position pair in r0
	mov r0, r4 @ ARG r0 = Target Unit
	ldr r3, prGetPushPosition
	_blr r3
	
	ldrb r1, [r4, #0x10]
	ldrb r2, [r4, #0x11]
	
	_MakePair r1, r1, r2
	
	cmp r0, r1
	beq ReturnFalse @ Push position is same as initial pos
	
	mov r0, #42
	
	b End
	
ReturnFalse:
	mov r0, #0

End:
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	