.thumb
.include "../../_CommonASM/_Definitions.h.s"

.set prGetShoveTargetOffset, EALiterals+0x00

MakeShoveTargetList:
	push {r4, lr}
	
	mov r4, r0 @ Unit
	
	@ Clearing Range Map
	bl ClearRangeMap
	
	ldrb r0, [r4, #0x10] @ x
	ldrb r1, [r4, #0x11] @ y
	
	@ Loading Address of routine to call
	adr r2, AddUnitToTargetListIfShovable
	add r2, #1 @ Thumb bit
	
	_blh prForEachAdjacentUnit
	
	pop {r4}
	
	pop {r0}
	bx r0

.ltorg
.align

ClearRangeMap:
	push {lr}
	
	ldr r3, =ppRangeMapRows
	ldr r0, [r3]
	
	mov r1, #0
	
	_blh prMap_Fill
	
	pop {r0}
	bx r0

.ltorg
.align

AddUnitToTargetListIfShovable:
	push {r4, lr}
	
	mov r4, r0
	
	@ Loading Unit Type
	ldrb r0, [r4, #0x0B]
	mov r3, #0x80
	cmp r0,r3
	bge NoShov4U

	ldr r3, =ppActiveUnit
	ldr r3, [r3]

	ldrb r1, [r4, #0x10] @ target  x
	ldrb r0, [r3, #0x10] @ subject x
	sub r1, r0 @ r1 = (target x) - (subject x)
	
	ldrb r2, [r4, #0x11] @ target  y
	ldrb r0, [r3, #0x11] @ subject y
	sub r2, r0 @ r2 = (target y) - (subject y)
	
	mov r0, r4
	
	ldr r3, prGetShoveTargetOffset
	_blr r3
	
	cmp r0, #0
	beq NoShov4U
	
	mov r3, r4
	
	ldrb r0, [r3, #0x10] @ Unit.x
	ldrb r1, [r3, #0x11] @ Unit.y
	ldrb r2, [r3, #0x0B] @ Unit.id
	
	ldr r3, =prAddTargetListEntry
	mov lr, r3
	
	mov r3, #0 @ Trap id, probably irrelevant
	
	.short 0xF800
	
NoShov4U:
	pop {r4}
	
	pop {r0}
	bx r0

.ltorg
.align

EALiterals:
	@ POIN prGetShoveTargetOffset
