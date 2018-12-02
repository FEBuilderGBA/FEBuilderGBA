.thumb
.include "_Definitions.h.s"

@ Arguments: r0 = x, r1 = y, r2 = range mask
FillRangeByMask:
	push {r4-r7, lr}
	
	_MakePair r4, r0, r1 @ r4 = (x, y)
	mov r5, r2
	
	mov r6, #31 @ Current Range
	mov r7, #0  @ Last Range bit
	
ContinueLoop:
	mov r0, r5
	lsr r0, r6
	
	mov r1, #1
	and r0, r1
	
	cmp r0, r7
	beq SkipForNow
	
	mov r7, r0
	
	ldr r3, =prMap_AddInRange
	mov lr, r3
	
	_GetPairFirst  r0, r4
	_GetPairSecond r1, r4
	
	mov r2, r6
	
	mov r3, #1
	
	cmp r7, #0
	bne SkipNegate
	
	neg r3, r3
	
SkipNegate:
	@ prMap_AddInRange(x, y, range, value)
	.short 0xF800
	
SkipForNow:
	sub r6, #1
	bge ContinueLoop
	
	pop {r4-r7}
	
	pop {r0}
	bx r0

.ltorg
.align

EALiterals:
	@ notin
