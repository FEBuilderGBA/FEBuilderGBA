.thumb
.include "../../../_CommonASM/_Definitions.h.s"
.include "../_Config.h.s"

.set prWaitMove6C_MoveTowardsGoal,  EALiterals+0x00
.set prWaitMove6C_MoveTowardsStart, EALiterals+0x04

ShoveAnimWait6C_Loop:
	push {r4, lr}
	
	@ Storing 6C into r4 for later use
	mov r4, r0
	
	@ SPIN SPIN SPIN
	.ifdef SPINNING_SHOVE
		mov r3, #0x2A @ 0x2A is where the spinning animation timer is located in the 6C
		ldrb r1, [r4, r3]
			add r1, #1 @ timer++
			
			@ and timer to 0xF (0b1111) to make it go from 15 to 0 and loop
			mov r2, #0xF
			and r1, r2
		strb r1, [r4, r3]
		lsr r1, #2 @ r1 = timer/4 (4 frames per direction)
		
		@ Setting direction for Target MOVEUNIT
		ldr r0, [r4, #0x30]
		_blh prMOVEUNIT_SetSprDirection
	.endif
	
	@ Setting up arguments
	mov r0, r4          @ 6C
	ldr r1, [r4, #0x2C] @ Subject MOVEUNIT
	mov r2, #BACK_SPEED @ move speed = 2 pixels per frame
	
	@ Moving Subject MOVEUNIT towards its actual position
	ldr r3, prWaitMove6C_MoveTowardsStart
	_blr r3
	
	@ Setting up arguments
	mov r0, r4            @ 6C
	ldr r1, [r4, #0x30]   @ Target MOVEUNIT
	mov r2, #PUSHED_SPEED @ move speed
	
	@ Moving Target MOVEUNIT towards its actual position
	ldr r3, prWaitMove6C_MoveTowardsGoal
	_blr r3
	
	@ Checking return value (0 means didn't move)
	cmp r0, #0
	bne End
	
	@ Breaking 6C Loop (which effectively means we end the animation)
	mov r0, r4
	_blh pr6C_BreakLoop
	
End:
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prWaitMove6C_MoveTowardsGoal
	@ POIN prWaitMove6C_MoveTowardsStart
