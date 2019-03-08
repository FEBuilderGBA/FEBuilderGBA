.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prUnitUnitMoveAnim_New, EALiterals+0x00

MoveTargetUnitAction:
	push {r4-r6,lr}
	
	@ PART 1: SETTING UP THE ANIMATIONS
	@ =================================
	
	@ Active Unit Animation
	@ ---------------------
	
	@ Delete all current MOVEUNITs
	_blh prMOVEUNIT_DeleteAll
	
	@ Loading action struct
	ldr r4, =pActionStruct
	
	ldr r0, =ppActiveUnit
	ldr r5, [r0] @ r5 = Active Unit Struct
	
	ldrb r0, [r4, #0x0D] @ Target Unit Index
	_blh prUnit_GetStruct
	mov r6, r0 @ r6 = Target Unit Struct
	
	@ Prepare call of GetFacingDirectionId
	ldr r3, =prGetFacingDirectionId
	mov lr, r3

	@ From: Active Unit Position
	ldrb r0, [r5, #0x10]
	ldrb r1, [r5, #0x11]
	
	@ To: Target Unit Position
	ldrb r2, [r6, #0x10]
	ldrb r3, [r6, #0x11]
	
	@ Call
	.short 0xF800
	
	mov r1, r0 @ r1 = Facing Direction
	mov r0, r5 @ r5 = Active Unit Struct
	
	@ Making the Active Unit Anim 6C
	ldr r3, prUnitUnitMoveAnim_New
	_blr r3
	
	@ Target Unit Animation
	@ ---------------------
	
	@ Prepare call of GetFacingDirectionId
	ldr r3, =prGetFacingDirectionId
	mov lr, r3

	@ From: Target Unit Position
	ldrb r0, [r6, #0x10]
	ldrb r1, [r6, #0x11]
	
	@ To: Active Unit Position
	ldrb r2, [r5, #0x10]
	ldrb r3, [r5, #0x11]
	
	@ Call
	.short 0xF800
	
	mov r1, r0 @ r1 = Facing Direction
	mov r0, r6 @ r5 = Target Unit Struct
	
	@ Making the Active Unit Anim 6C
	ldr r3, prUnitUnitMoveAnim_New
	_blr r3
	
	@ PART 2: APPLYING THE ACTUAL CHANGES
	@ ===================================
	
	@ Updating active pos
		@ x
		ldrb r0, [r4, #0x0E]
		strb r0, [r5, #0x10]
		
		@ y
		ldrb r0, [r4, #0x0F]
		strb r0, [r5, #0x11]
	
	@ Apply Movement
	mov r0, r5 @ Active Unit
	_blh prUnit_ApplyMovement
	
	@ Updating target pos
		@ x
		ldrb r0, [r4, #0x13]
		strb r0, [r6, #0x10]
		
		@ y
		ldrb r0, [r4, #0x14]
		strb r0, [r6, #0x11]
	
	@ Apply Movement
	mov r0, r6 @ Target Unit
	_blh prUnit_ApplyMovement
	
	@ Updating map sprites and stuff
	_blh prIDunnoReallyButIThinkItUpdatesStandingSprites
	
	@ Yield (6C), because if we don't the newly created MOVEUNITs will be deleted before the thread lock comes into play, and nothing would ever happen again sadly
	mov r0, #0
	
	pop {r4-r6}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prUnitUnitMoveAnim_New
