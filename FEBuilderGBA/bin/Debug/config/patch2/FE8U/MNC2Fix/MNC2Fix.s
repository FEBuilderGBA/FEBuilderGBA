	.thumb

	.global MoveToChapter2Hook
	.type   MoveToChapter2Hook, function

MoveToChapter2Hook:
	push {r2, r4-r7}

	mov r6, r2 @ var r6 = chapter id

	mov r4, #0 @?var r4 = i
	mov r7, #2 @ var r7 = 2 (flag)

	ldr r5, =0x030052B0 @=gGMData+48 @ var r5 = it

.lop:
	ldr  r3, =0x080BB5B1 @=WMLocation_GetChapterId

	mov  r0, r4 @?arg r0 = chapter id

	bl   BXR3

	cmp r0, r6
	bne .continue

	@ set flag for the chapter node

	ldrb r3, [r5]
	orr  r3, r7
	strb r3, [r5]

.continue:
	add r4, #1 @?i++
	add r5, #4 @ it++

	cmp r4, #29
	bne .lop

	pop {r2, r4-r7}

	ldr r3, =0x0800F498+1

BXR3:
	bx r3

	.pool
	.align
