.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prUnitUnitMoveAnim_New, EALiterals+0x00

MoveTargetUnitAction:
	push {r4-r5,lr}
	
	@ PART 1: SETTING UP THE ANIMATION
	@ --------------------------------
	
	@ Loading action struct
	ldr r4, =pActionStruct
	
	@ Loading target unit struct
	ldrb r0, [r4, #0x0D] @ Target Unit Index
	_blh prUnit_GetStruct
	mov r5, r0 @ r5 = Target Unit Struct
	
	@ Prepare call of GetFacingDirectionId
	ldr r3, =prGetFacingDirectionId
	mov lr, r3

	@ From: Current Target Unit Position
	ldrb r1, [r0, #0x11]
	ldrb r0, [r0, #0x10]
	
	@ To: Target Position
	ldrb r2, [r4, #0x13]
	ldrb r3, [r4, #0x14]
	
	@ Call
	.short 0xF800
	
	mov r1, r0 @ r1 = Facing Direction
	mov r0, r5 @ r5 = Target Unit Struct
	
	@ Making our animation 6C
	@ NOTE: we need to make that 6C *before* moving the units coordinates, since it will read the units coordinates to position the MOVEUNIT
	ldr r3, prUnitUnitMoveAnim_New
	_blr r3
	
	@ PART 2: APPLYING THE ACTUAL CHANGES
	@ -----------------------------------
	
	@ Updating x pos
	ldrb r1, [r4, #0x13]
	strb r1, [r5, #0x10]
	
	@ Updating y pos
	ldrb r1, [r4, #0x14]
	strb r1, [r5, #0x11]
	
	@ Apply Movement
	mov r0, r5 @ Target Unit
	_blh prUnit_ApplyMovement
	
	@ Updating map sprites and stuff
	_blh prIDunnoReallyButIThinkItUpdatesStandingSprites
	
	@ Yield (6C), because if we don't the newly created MOVEUNITs will be deleted before the thread lock comes into play, and nothing would ever happen again sadly
	mov r0, #0
	
	pop {r4-r5}
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prUnitUnitMoveAnim_New
