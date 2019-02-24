.thumb
.include "../../_CommonASM/_Definitions.h.s"

.set prWaitForMove6CNew, EALiterals+0x00

ShoveAction:
	push {r4}
	
	_blh prMOVEUNIT_DeleteAll
	
	ldr  r4, =pActionStruct
	ldrb r0, [r4, #0x0D] @ Target Unit Index
	
	_blh prUnit_GetStruct
	mov r1, r0
	
	ldr r0, =ppActiveUnit
	ldr r0, [r0]
	
	@ NOTE: we need to make that 6C *before* moving the units coordinates, since it will read the units coordinates to position the MOVEUNIT
	ldr r3, prWaitForMove6CNew
	_blr r3
	
	ldrb r0, [r4, #0x0D] @ Target Unit Index
	_blh prUnit_GetStruct
	
	@ Updating x
	ldrb r1, [r4, #0x13]
	strb r1, [r0, #0x10]
	
	@ Updating y
	ldrb r1, [r4, #0x14]
	strb r1, [r0, #0x11]
	
	_blh prUnit_ApplyMovement
	
	_blh prIDunnoReallyButIThinkItUpdatesStandingSprites
	
	@ Yield (6C), because if we don't the newly created MOVEUNITs will be deleted before the thread lock comes into play
	mov r0, #0
	
TestSkip:
	pop {r4}
	
	@Rewrite to be called by switch. by 7743
	ldr r1,=0x803215e+1
	bx r1

.ltorg
.align

EALiterals:
	@ nothing apparently
