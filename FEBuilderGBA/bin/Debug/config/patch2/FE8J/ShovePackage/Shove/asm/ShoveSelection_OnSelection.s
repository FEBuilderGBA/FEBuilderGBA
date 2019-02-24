.thumb

.include "../../_CommonASM/_Definitions.h.s"

.set prGetShoveTargetOffset, EALiterals+0x00
.set ACTION_SHOVE,           EALiterals+0x04

ShoveSelection_OnSelection:
	push {r4, lr}
	
	@ Storing Target Struct
	mov r4, r1
	
	ldrb r0, [r4, #2]
	_blh prUnit_GetStruct
	mov r12, r0
	
	ldr r3, =ppActiveUnit
	ldr r3, [r3]
	
	ldrb r1, [r4, #0]    @ target x
	ldrb r0, [r3, #0x10] @ subject x
	sub r1, r0 @ r1 = (target x) - (subject x)
	
	ldrb r2, [r4, #1]    @ target  y
	ldrb r0, [r3, #0x11] @ subject y
	sub r2, r0 @ r2 = (target y) - (subject y)
	
	mov r0, r12
	
	ldr r3, prGetShoveTargetOffset
	_blr r3
	
	ldr r3, =pActionStruct
	
	_GetPairFirst r1, r0 @ shove offset
	ldrb r2, [r4, #0]    @ target x
	add r1, r2
	strb r1, [r3, #0x13] @ Action x2
	
	_GetPairSecond r1, r0 @ shove offset
	ldrb r2, [r4, #1]     @ target y
	add r1, r2
	strb r1, [r3, #0x14]  @ Action y2

	@ Action Id
	ldr r0, ACTION_SHOVE
	strb r0, [r3, #0x11]
	
	@ Target Unit Index
	ldrb r0, [r4, #2]
	strb r0, [r3, #0x0D]
	
	@ 0x01 = Center Camera On Target, 0x02 = Kill Unit Selection, 0x04 = Beep Sound, 0x10 = Clear Unit Selection Gfx
	mov r0, #0x17
	
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prGetShoveTargetOffset
	@ WORD ACTION_SHOVE
