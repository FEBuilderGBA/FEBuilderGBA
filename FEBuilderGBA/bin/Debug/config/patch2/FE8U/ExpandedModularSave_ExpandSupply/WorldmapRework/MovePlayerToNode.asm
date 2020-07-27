.thumb

.equ origin, 0x080BCCFC
.equ SomeSkirmishCheckFunction, . + 0x080BCA54 - origin
.equ PathfindingFunc_BCAB8, . + 0x080BCAB8 - origin
.equ PathfindingFunc_BCBAC, . + 0x080BCBAC - origin
.equ CPUSet, . + 0x080D1678 - origin
.equ gPathfindingBuffer, gPathMap+4

@original size: 232 bytes
@new size: 220 bytes, +8 for EA pointers

MovePlayerToNode:
	push {r4-r7,lr}
	mov r7, r10
	mov r6, r9
	mov r5, r8
	push {r5-r7}
	sub sp, #0x18
	mov r4, r2
	mov r5, #0xff
	mov r7, r0
	and r7, r5
	str r7, [sp, #0x14]
	and r1, r5
	and r4, r5
	mov r9, r1
	mov r10, r9
	ldr r0, =0x0201B100
	bl SomeSkirmishCheckFunction
	lsl r4, r4, #0x18
	asr r4, r4, #0x18
	mov r8, r4
	ldr r6, gPathfindingBuffer
	cmp r4, #0x0
	beq SkipStoreToBuffer
		mov r0, #0x20
		add r1, r6, r0
		str r0, [r1, #0x24] @makes this store to 0x44
SkipStoreToBuffer:
	mov r0, #0x20
	mov r1, #0x40
	str r0, [r6, r1]
	mov r4, #0x0
	ldr r5, =0x05000008
	str r4, [sp, #0xc]
	add r0, SP, #0xC	@A803
	mov r1, r6
	mov r2, r5
	bl CPUSet
	str r4, [sp, #0x10]
	add r0, sp, #0x10	@A804
	mov r1, r6
	add r1, #0x20
	mov r2, r5
	bl CPUSet
	strb r7, [r6, #0x0]
	mov r0, #0x20
	strb r7, [r6, r0] @yes this is 20 now
	mov r0, r8
	cmp r0, #0x0
	beq UsePathfind_BCAB8
		ldr r1, gPathMap
		mov r2, #0x1
		neg r2, r2
		lsl r3, r7, #0x18
		asr r3, r3, #0x18
		mov r4, r9
		lsl r0, r4, #0x18
		asr r0, r0, #0x18
		str r0, [sp, #0x0]
		mov r4, #0x1
		str r4, [sp, #0x4]
		str r2, [sp, #0x8]
		mov r0, r6
		bl PathfindingFunc_BCBAC
		b CheckPathfindResult
UsePathfind_BCAB8:
	ldr r1, gPathMap
	mov r2, #0x1
	neg r2, r2
	ldr r0, [sp, #0x14]
	lsl r3, r0, #0x18
	asr r3, r3, #0x18
	mov r4, r10
	lsl r0, r4, #0x18
	asr r0, r0, #0x18
	str r0, [sp, #0x0]
	mov r4, #0x1
	str r4, [sp, #0x4]
	mov r0, r6
	bl PathfindingFunc_BCAB8
CheckPathfindResult:
	cmp r0, #0x0
	beq NotFound
		mov r0, #0x21
		ldrb r0, [r6, r0] 
		strb r0, [r6, #0x1]
		mov r0, #0x40
		str r4, [r6, r0] @make this 0x40
		mov r0, #0x1
		b ExitFunc
NotFound:
	mov r1, #0x0
	ldr r0, gPathfindingBuffer
	ldr r0, [r0, #0x40]
	cmp r0, #0x1f
	bgt GetResult
		mov r1, #0x1
GetResult:
	mov r0, r1
ExitFunc:
	add sp, #0x18
	pop {r3-r5}
	mov r8, r3
	mov r9, r4
	mov r10, r5
	pop {r4-r7}
	pop {r1}
	bx r1

.align
.ltorg

gPathMap:
@WORD gPathMap
@WORD gPathfindingBuffer
