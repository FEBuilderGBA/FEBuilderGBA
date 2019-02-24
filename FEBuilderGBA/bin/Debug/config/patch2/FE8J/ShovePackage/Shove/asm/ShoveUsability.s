.thumb

.include "../../_CommonASM/_Definitions.h.s"

.set prMakeShoveTargetList, EALiterals+0x00

ShoveUsability:
	push {lr}
	
	@ Loading Active Unit
	ldr r3, =ppActiveUnit
	ldr r0, [r3]
	
	@ Loading Unit Type
	ldrb r1, [r0, #0x0B]
	mov r3, #0x80
	cmp r1,r3
	bge ReturnNotUsable

	@ Making Target List
	ldr r3, prMakeShoveTargetList
	_blr r3
	
	@ Getting Size
	_blh prGetTargetListSize
	
	cmp r0, #0
	beq ReturnNotUsable
	
	mov r0, #1
	b End
	
ReturnNotUsable:
	mov r0, #3
	
End:
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prMakeShoveTargetList
