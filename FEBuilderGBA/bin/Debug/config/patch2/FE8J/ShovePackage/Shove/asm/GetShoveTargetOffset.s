.thumb
.include "../../_CommonASM/_Definitions.h.s"

.set prUnit_CanStandOnPosition, EALiterals+0x00

@ Arguments: r0 = Target Unit Struct, r1 = Direction X, r2 = Direction Y
@ Returns: Position Offset Pair
GetShoveTargetOffset:
	push {r4-r5, lr}
	
	@ Moving direction coordinates for later use
	mov r4, r1
	mov r5, r2
	
	@ Getting target X
	ldrb r1, [r0, #0x10]
	add r1, r4
	
	@ Getting target Y
	ldrb r2, [r0, #0x11]
	add r2, r5
	
	@ Checking position
	ldr r3, prUnit_CanStandOnPosition
	_blr r3
	
	@ Return pair(0, 0) = int(0) if cannot stand
	cmp r0, #0
	beq End
	
	@ Making Direction pair if can
	_MakePair r0, r4, r5
	
End:
	pop {r4-r5}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prUnit_CanStandOnPosition
