.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prUnit_CanStandOnPosition, EALiterals+0x00
.set prIsTargetLegal,           EALiterals+0x04

Swap_TargetCheck:
	push {r4, lr}
	
	@ r4 = target unit struct
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
	ldr r0, =ppActiveUnit
	ldr r0, [r0] @ ARG r0 = unit
	
	@ Loading target position
	ldrb r1, [r4, #0x10] @ ARG r1 = target x
	ldrb r2, [r4, #0x11] @ ARG r2 = target y
	
	@ Can Active Unit Stand where Target is?
	ldr r3, prUnit_CanStandOnPosition
	_blr r3
	
	cmp r0, #0
	beq End @ Returns 0
	
	@ Loading active unit again
	ldr r0, =ppActiveUnit
	ldr r3, [r0]
	
	@ ARG r0 = unit
	mov r0, r4
	
	@ Loading target position
	ldrb r1, [r3, #0x10] @ ARG r1 = target x
	ldrb r2, [r3, #0x11] @ ARG r2 = target y
	
	@ Can Target Unit Stand where Active is?
	ldr r3, prUnit_CanStandOnPosition
	_blr r3
	
End:
	pop {r4}
	
	pop {r1}
	bx r1
	
.ltorg
.align

EALiterals:
	@ POIN prUnit_CanStandOnPosition
	@ POIN prIsTargetLegal
