.thumb
.include "../../../_CommonASM/_Definitions.h.s"
.include "../_Config.h.s"

.set prWaitMove6C_MoveTowardsGoal, EALiterals+0x00

ShoveAnimWait6C_Loop:
	push {lr}
	
	@ Argument setup
	@ (Implied)         @ r0 = 6C
	ldr r1, [r0, #0x2C] @ r1 = Subject MOVEUNIT
	mov r2, #PUSH_SPEED @ r2 = move speed
	
	@ Moving Subject Towards Target
	ldr r3, prWaitMove6C_MoveTowardsGoal
	_blr r3
	
	@ Force 6C Yield (Else the whole pre-animation will happen in less than a frame)
	mov r0, #0
	
	@ End
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prWaitMove6C_MoveTowardsGoal
