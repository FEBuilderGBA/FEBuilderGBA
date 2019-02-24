.thumb
.include "../../../_CommonASM/_Definitions.h.s"
.include "../_Config.h.s"

.set prMoveMOVEUNITTowards, EALiterals+0x00

@ arguments: r0 = 6C pointer, r1 = MOVEUNIT to move, r2 = speed
@ returns: 0 if nothing moved
MoveWait6C_MoveTowardsGoal:
	push {r4, lr}
	
	@ Moving speed into r3
	mov r3, r2
	
	@ Loading Target Unit Struct
	ldr r2, [r0, #0x38]
	
	@ Moving MOVEUNIT into r0
	mov r0, r1
	
	@ Getting Target Position
	ldrb r1, [r2, #0x10]
	ldrb r2, [r2, #0x11]
	
	@ MOVE
	ldr r4, prMoveMOVEUNITTowards
	_blr r4
	
	@ End (Return Value is passed)
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prMoveMOVEUNITTowards
