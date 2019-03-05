.thumb
.include "_Definitions.h.s"

@ Only checks for terrain, ignores units

@ Arguments: r0 = Unit Struct, r1 = xPosition, r2 = yPosition
CanUnitStandOnPosition:
	push {r4, lr}
	
	ldr r4, =pCurrentMapSize
	
	@ BOUND CHECK BEGIN
	@ -----------------
	
	cmp r1, #0 @ x < 0
	blt ReturnFalse
	
	cmp r2, #0 @ y < 0
	blt ReturnFalse
	
	ldrh r3, [r4, #0]
	
	cmp r1, r3 @ x >= size.x
	bge ReturnFalse
	
	ldrh r3, [r4, #2]
	
	cmp r2, r3 @ y >= size.y
	bge ReturnFalse
	
	@ POSITION CHECK BEGIN
	@ --------------------
	
	@ Loading relevant map pointers
	ldr r4, =ppTerrainMapRows
	
	@ Loading actual map row arrays
	ldr r4, [r4]
	
	@ converting y pos into row pointer offset
	lsl r3, r2, #2 @ r3 = y*4
	
	@ getting the relevant row pointer for each map
	add r4, r3
	
	@ loading pointers
	ldr r4, [r4]
	
	@ loading terrain index at position and check
	ldrb r1, [r4, r1]
	_blh prUnit_CanCrossTerrain
	
	@ Pass return value to end
	b End
	
ReturnFalse:
	mov r0, #0
	
End:
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ nothing
