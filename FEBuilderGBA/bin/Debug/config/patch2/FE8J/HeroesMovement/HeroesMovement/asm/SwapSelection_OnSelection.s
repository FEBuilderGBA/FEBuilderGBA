.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set ACTION_SWAP, EALiterals+0x00

PivotSelection_OnSelection:
	push {r4, lr}
	
	@ r4 = Target Struct
	mov r4, r1
	
	@ r0 = Target Unit Struct
	ldrb r0, [r4, #2]
	_blh prUnit_GetStruct
	
	@ r3 = Action Struct
	ldr r3, =pActionStruct
	
	@ Target active x
	ldrb r1, [r0, #0x10]
	strb r1, [r3, #0x0E]
	
	@ Target active y
	ldrb r1, [r0, #0x11]
	strb r1, [r3, #0x0F]
	
	@ Target Unit index
	ldrb r0, [r4, #2]
	strb r0, [r3, #0x0D]
	
	@ r0 = Active Unit Struct
	ldr r0, =ppActiveUnit
	ldr r0, [r0]
	
	@ Target target x
	ldrb r1, [r0, #0x10]
	strb r1, [r3, #0x13]
	
	@ Target target y
	ldrb r1, [r0, #0x11]
	strb r1, [r3, #0x14]
	
	ldr r0, ACTION_SWAP
	strb r0, [r3, #0x11] @ Action Index
	
	@ 0x02 = Kill Unit Selection, 0x04 = Beep Sound, 0x10 = Clear Unit Selection Gfx
	mov r0, #0x16
	
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ WORD ACTION_SWAP
