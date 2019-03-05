.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prGetTargetPosition,    EALiterals+0x00
.set prUnit_CanBeOnPosition, EALiterals+0x04
.set prIsTargetLegal,        EALiterals+0x08

Pivot_TargetCheck:
	push {r4, lr}
	
	@ r3 = legality check routine ptr
	ldr r3, prIsTargetLegal
	
	@ if no legality check has been set, skip
	cmp r3, #0
	beq SkipLegalCheck
	
	@ "pushing" target unit struct
	mov r4, r0
	
	@ call
	_blr r3
	
	cmp r0, #0
	beq End @ Returns 0 since r0 is 0
	
	@ "popping" target unit struct
	mov r0, r4
	
SkipLegalCheck:
	@ r4 = Active Unit Struct
	ldr r4, =ppActiveUnit
	ldr r4, [r4]
	
	@ Loading target position
	ldrb r1, [r0, #0x10] @ ARG r1 = target.x
	ldrb r2, [r0, #0x11] @ ARG r2 = target.y

	mov r0, r4 @ ARG r0 = unit
	
	@ Getting Target position in [r1, r2]
	ldr r3, prGetTargetPosition
	_blr r3
	
	@ Checking position
	mov r0, r4
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
