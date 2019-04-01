.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prUnitUnitMoveAnim_New, EALiterals+0x00

MoveActiveUnitAction:
	push {r4,lr}
	
	@ PART 1: SETTING UP THE ANIMATION
	@ --------------------------------
	
	@ Delete all current MOVEUNITs
	_blh prMOVEUNIT_DeleteAll
	
	@ Loading action struct
	ldr  r4, =pActionStruct
	ldrb r0, [r4, #0x0D] @ Target Unit Index
	
	@ Loading target unit struct
	_blh prUnit_GetStruct

	@ Prepare call of GetFacingDirectionId
	ldr r3, =prGetFacingDirectionId
	mov lr, r3
	
	@ From: Target *Unit* Position
	ldrb r1, [r0, #0x11]
	ldrb r0, [r0, #0x10]
	
	@ To: Target Position
	ldrb r2, [r4, #0x0E]
	ldrb r3, [r4, #0x0F]
	
	@ Call
	.short 0xF800
	
	@ r1 = Facing Direction
	mov r1, r0
	
	@ Loading active unit struct
	ldr r0, =ppActiveUnit
	ldr r0, [r0]
	
	@ Making our animation 6C
	@ NOTE: we need to make that 6C *before* moving the units coordinates, since it will read the units coordinates to position the MOVEUNIT
	ldr r3, prUnitUnitMoveAnim_New
	_blr r3
	
	@ PART 2: APPLYING THE ACTUAL CHANGES
	@ -----------------------------------
	
	@ Loading active unit again
	ldr r0, =ppActiveUnit
	ldr r0, [r0]
	
	@ Updating x pos
	ldrb r1, [r4, #0x0E]
	strb r1, [r0, #0x10]
	
	@ Updating y pos
	ldrb r1, [r4, #0x0F]
	strb r1, [r0, #0x11]
	
	@ Apply Movement
	_blh prUnit_ApplyMovement
	
	@ Updating map sprites and stuff
	_blh prIDunnoReallyButIThinkItUpdatesStandingSprites
	
	@ Yield (6C), because if we don't the newly created MOVEUNITs will be deleted before the thread lock comes into play, and nothing would ever happen again sadly
	mov r0, #0
	
	pop {r4}
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prUnitUnitMoveAnim_New
