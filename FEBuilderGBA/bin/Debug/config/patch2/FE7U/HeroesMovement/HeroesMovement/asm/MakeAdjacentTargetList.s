.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prClearRangeMap, EALiterals+0x00
.set prTargetCheck,   EALiterals+0x04

MakeAdjacentTargetList:
	push {lr}
	
	@ Clearing Range Map
	ldr r0, prClearRangeMap
	_blr r0
	
	@ Loading Active Unit Position
	ldr r3, =ppActiveUnit
	ldr r3, [r3]
	
	ldrb r0, [r3, #0x10] @ x
	ldrb r1, [r3, #0x11] @ y
	
	@ Loading Address of routine to call
	adr r2, PotentiallyAddUnitToTargetList
	add r2, #1 @ Thumb bit
	
	@ for each unit adjactent to position [r0, r1] call r2
	_blh prForEachAdjacentUnit
	
	@ Clearing Range Map again to avoid range display glitches
	ldr r0, prClearRangeMap
	_blr r0
	
	pop {r0}
	bx r0

.ltorg
.align

PotentiallyAddUnitToTargetList:
	push {r4, lr}
	
	@ r4 = Target Unit Struct
	mov r4, r0
	
	ldr r3, prTargetCheck
	
	cmp r3, #0
	beq SkipChecking
	
	_blr r3
	
	cmp r0, #0
	beq SkipAdding
	
SkipChecking:
	ldrb r0, [r4, #0x10] @ Unit.x
	ldrb r1, [r4, #0x11] @ Unit.y
	ldrb r2, [r4, #0x0B] @ Unit.id
	
	ldr r3, =prAddTargetListEntry
	mov lr, r3
	
	mov r3, #0 @ Trap id, probably irrelevant
	
	.short 0xF800
	
SkipAdding:
	pop {r4}
	
	pop {r0}
	bx r0

.ltorg
.align

EALiterals:
	@ POIN prClearRangeAndMoveMap
	@ POIN prTargetCheck
