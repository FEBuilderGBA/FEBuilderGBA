.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prGetTargetPosition,    EALiterals+0x00
.set prUnit_CanBeOnPosition, EALiterals+0x04
.set prIsTargetLegal,        EALiterals+0x08

Reposition_TargetCheck:
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
	ldr r3, [r3] @ ARG r0 = unit
	
	@ Loading unit position
	ldrb r1, [r3, #0x10] @ ARG r1 = unit.x
	ldrb r2, [r3, #0x11] @ ARG r2 = unit.y
	
	@ Getting potential target position in [r1, r2]
	mov r0, r4 @ Target Unit
	ldr r3, prGetTargetPosition
	_blr r3
	
	@ Checking that position
	mov r0, r4 @ Target Unit
	ldr r3, prUnit_CanBeOnPosition
	_blr r3
	
End:
	pop {r4}
	
	pop {r1}
	bx r1
	
.ltorg
.align

EALiterals:
	@ POIN prGetTargetPosition
	@ POIN prUnit_CanBeOnPosition
	@ POIN prIsTargetLegal
