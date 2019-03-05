.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prUnitMoveAnim_New, EALiterals+0x00

PushAnim_BeginTargetPush:
	push {r4-r5, lr}
	
	@ Loading action struct
	ldr r4, =pActionStruct
	
	@ Loading target unit struct
	ldrb r0, [r4, #0x0D] @ Target Unit Index
	_blh prUnit_GetStruct
	mov r5, r0 @ r5 = Target Unit Struct
	
	@ Prepare call of GetFacingDirectionId
	ldr r3, =prGetFacingDirectionId
	mov lr, r3
	
	@ From: Target Position
	ldrb r0, [r4, #0x13]
	ldrb r1, [r4, #0x14]
	
	@ To: Current Target Unit Position
	ldrb r2, [r5, #0x10]
	ldrb r3, [r5, #0x11]
	
	@ Call
	.short 0xF800
	
	mov r1, r0 @ r1 = Facing Direction
	mov r0, r5 @ r5 = Target Unit Struct
	
	@ Making our animation 6C
	ldr r3, prUnitMoveAnim_New
	_blr r3
	
	@ STEP 2: APPLY ACTION
	@ --------------------
	
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
	
	pop {r4-r5}
	
	pop {r0}
	bx r0

.ltorg
.align

EALiterals:
	@ prUnitMoveAnim_New
