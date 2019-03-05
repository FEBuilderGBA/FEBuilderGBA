.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prUnit_CanBeOnPosition, EALiterals+0x00
.set PUSH_AMOUNT,            EALiterals+0x04

@ Arguments: r0 = Subject Unit Struct, r1 = Push Source X, r2 = Push Source Y
@ Returns: r0 = Pos Pair (additionally: r1 = Pos X, r2 = Pos Y)
GetPushPosition:
	push {r4-r7, lr}
	
	@ r4 = Unit Struct
	mov r4, r0
	
	ldrb r3, [r4, #0x10]
	sub  r1, r3, r1 @ r1 = direction.x = unit.x - source.x
	
	ldrb r3, [r0, #0x11]
	sub  r2, r3, r2 @ r2 = direction.y = unit.y - source.y
	
	@ r5 = Direction Pair
	_MakePair r5, r1, r2
	
	ldrb r0, [r4, #0x10]
	ldrb r1, [r4, #0x11]
	
	@ r6 = Current Push Target Pair
	_MakePair r6, r0, r1
	
	ldr r7, PUSH_AMOUNT
	
StartLoop:
	sub r7, #1
	blt Finish
	
	@ Getting Next Target X
	_GetPairFirst  r0, r5
	_GetPairFirst  r1, r6
	add r1, r0
	
	@ Getting Next Target Y
	_GetPairSecond r0, r5
	_GetPairSecond r2, r6
	add r2, r0
	
	@ Getting Unit Struct
	mov r0, r4
	
	ldr r3, prUnit_CanBeOnPosition
	_blr r3
	
	cmp r0, #0
	beq Finish
	
	@ Getting Next Target X
	_GetPairFirst  r0, r5
	_GetPairFirst  r1, r6
	add r1, r0
	
	@ Getting Next Target Y
	_GetPairSecond r0, r5
	_GetPairSecond r2, r6
	add r2, r0
	
	@ Setting Next Target X
	_MakePair r6, r1, r2
	
	b StartLoop
	
Finish:
	_GetPairFirst  r1, r6
	_GetPairSecond r2, r6
	
	mov r0, r6
	
	pop {r4-r7}
	
	pop {r3}
	bx r3

.ltorg
.align

EALiterals:
	@ POIN prUnit_CanBeOnPosition
	@ WORD PUSH_AMOUNT
