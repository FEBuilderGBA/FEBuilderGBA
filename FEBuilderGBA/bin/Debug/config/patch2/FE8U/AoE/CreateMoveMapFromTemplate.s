.thumb

@ By StanH 


.macro make_pair rd, rs1, rs2, rox=r3
	lsl \rox, \rs1, #16 @ clearing top 16 bits of part 1
	lsl \rd,  \rs2, #16 @ clearing top 16 bits of part 2
	lsr \rox,       #16 @ shifting back part 1
	orr \rd, \rox       @ OR
.endm

.macro get_pair_first rd, rs
	lsl \rd, \rs, #16 @ clearing second part of pair
	asr \rd, \rd, #16 @ shifting back
.endm

.macro get_pair_second rd, rs
	asr \rd, \rs, #16 @ shifting second part of pair (erasing first part in the process)
.endm

.macro _maxZero reg, oreg=r3
	@ if reg < 0, reg = 0, else no changes
	asr \oreg, \reg, #31
	bic \reg, \oreg
.endm

.macro _blh to, reg=r3
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm



.equ FE8U_Memcopy,       0x080D1C0C	@{U}
@.equ FE8U_Memcopy,       0x080D6908	@{J}

.equ FE8U_MapSizeStruct, 0x0202E4D4	@{U}
@.equ FE8U_MapSizeStruct, 0x0202E4D0	@{J}
.equ FE8U_MoveMapRows,  0x0202E4E0	@{U}
@.equ FE8U_MoveMapRows,  0x0202E4DC	@{J}

@ Ok so, definition time:
@ Header:
@   [UByte]: x size
@   [UByte]: y size
@   [UByte]: x origin
@   [UByte]: y origin
@ Data:
@   [UByte array]: 1 if in range, 0 if not

@ Arguments: r0 = center X, r1 = center Y, r2 = pointer to template


.global CreateMoveMapFromTemplate
.type CreateMoveMapFromTemplate, %function 

CreateMoveMapFromTemplate:
	push {r4-r7, lr}
	
	mov r4, r8
	push {r4} @ push r8
	
	@ /!\ This routine does not clear the range map!
	
	@ CURRENT:
		@ r0 = r0 (arg)
		@ r1 = r1 (arg)
		@ r2 = r2 (arg)
		@ *r3 is free
		@ *r4 is free
		@ *r5 is free
		@ *r6 is free
		@ *r7 is free
	
	@ tSizeX = byte[r2 + 0x00]
	@ tSizeY = byte[r2 + 0x01]
	
	ldrb r4, [r2, #0x00] @ r4 = tSizeX
	ldrb r5, [r2, #0x01] @ r5 = tSizeY
	
	@ anchorX = r0 - byte[r2 + 0x02]
	@ anchorY = r1 - byte[r2 + 0x03]
	
	ldrb r3, [r2, #0x02]
	sub  r0, r3 @ r0 = anchorX
	
	ldrb r3, [r2, #0x03]
	sub  r1, r3 @ r1 = anchorY
	
	@ mSizeX = short[FE8U_MapSizeStruct + 0x00]
	@ mSizeY = short[FE8U_MapSizeStruct + 0x02]
	
	ldr r3, =FE8U_MapSizeStruct
	
	ldrh r6, [r3, #0x00] @ r6 = mSizeX
	ldrh r7, [r3, #0x02] @ r7 = mSizeY
	
	@ CURRENT:
		@ r0 = anchorX
		@ r1 = anchorY
		@ r2 = r2
		@ *r3 is free
		@ r4 = tSizeX
		@ r5 = tSizeY
		@ r6 = mSizeX
		@ r7 = mSizeY
	
	@ sizeY = min(tSizeY, min(tSizeY + anchorY, mSizeY - anchorY))
	
	sub r3, r7, r1 @ r3 = mSizeY - anchorY
	@ FREE: r7 (mSizeY)
	
	add r7, r5, r1 @ r7 = tSizeY + anchorY
	
	cmp r3, r7
	blt OKAY_MIN_1
	
	mov r3, r7
	
OKAY_MIN_1:
	@ r3 = min(tSizeY + anchorY, mSizeY - anchorY)
	@ FREE: r7 (tSizeY + anchorY)
	
	cmp r3, r5
	blt OKAY_MIN_2
	
	mov r3, r5
	
OKAY_MIN_2:
	@ r3 = sizeY = min(tSizeY, min(tSizeY + anchorY, mSizeY - anchorY))
	@ FREE: r5 (tSizeY)
	
	@ CURRENT:
		@ r0 = anchorX
		@ r1 = anchorY
		@ r2 = r2
		@ r3 = sizeY
		@ r4 = tSizeX
		@ *r5 is free
		@ r6 = mSizeX
		@ *r7 is free
	
	@ sizeX = min(tSizeX, min(tSizeX + anchorX, mSizeX - anchorX))
	
	sub r5, r6, r0 @ r5 = mSizeX - anchorX
	@ FREE: r6 (mSizeX)
	
	add r6, r4, r0 @ r6 = tSizeX + anchorX
	
	cmp r5, r6
	blt OKAY_MIN_3
	
	mov r5, r6

OKAY_MIN_3:
	@ r5 = min(tSizeX + anchorX, mSizeX - anchorX)
	@ FREE: r6 (tSizeX + anchorX)
	
	cmp r5, r4
	blt OKAY_MIN_4
	
	mov r5, r4

OKAY_MIN_4:
	@ r5 = sizeX = min(tSizeX, min(tSizeX + anchorX, mSizeX - anchorX))
	
	@ CURRENT:
		@ r0 = anchorX
		@ r1 = anchorY
		@ r2 = r2
		@ r3 = sizeY
		@ r4 = tSizeX
		@ r5 = sizeX
		@ *r6 is free
		@ *r7 is free
	
	@ mStartX = max(0, +anchorX)
	@ tStartX = max(0, -anchorX)
	
	mov r6, r0
	_maxZero r6, r7 @ r6 = mStartX = max(0, +anchorX)
	
	neg r7, r0
	_maxZero r7, r0 @ r7 = tStartX = max(0, -anchorX)
	
	@ FREE: r0 (anchorX)
	
	@ CURRENT:
		@ *r0 is free
		@ r1 = anchorY
		@ r2 = r2
		@ r3 = sizeY
		@ r4 = tSizeX
		@ r5 = sizeX
		@ r6 = mStartX
		@ r7 = tStartX
	
	@ mStartY = max(0, +anchorY)
	@ tStartY = max(0, -anchorY)
	
	mov r8, r2 @ Push r2
	
	mov r0, r1
	_maxZero r0, r2 @ r0 = mStartY = max(0, +anchorY)
	
	neg r1, r1
	_maxZero r1, r2 @ r1 = tStartY = max(0, -anchorY)
	@ FREE: r2 (temp)
	
	@ CURRENT:
		@ r0 = mStartY
		@ r1 = tStartY
		@ *r2 is free
		@ r3 = sizeY
		@ r4 = tSizeX
		@ r5 = sizeX
		@ r6 = mStartX
		@ r7 = tStartX
		@ (r8 = r2)
	
	@ startPair = pair(mStartX, tStartX)
	
	@ r7 = startPair = pair(mStartX, tStartX)
	make_pair r7, r6, r7, r2
	@ FREE: r6 (mStartX)
	
	@ CURRENT:
		@ r0 = mStartY
		@ r1 = tStartY
		@ *r2 is free
		@ r3 = sizeY
		@ r4 = tSizeX
		@ r5 = sizeX
		@ *r6 is free
		@ r7 = startPair
		@ (r8 = r2)
	
	@ sizePair = pair(sizeX, tSizeX)
	
	@ r6 = sizePair = pair(sizeX, tSizeX)
	make_pair r6, r5, r4, r2
	@ FREE: r5 (sizeX)
	
	@ CURRENT:
		@ r0 = mStartY
		@ r1 = tStartY
		@ *r2 is free
		@ r3 = sizeY
		@ r4 = tSizeX
		@ *r5 is free
		@ r6 = sizePair
		@ r7 = startPair
		@ (r8 = r2)
	
	ldr r2, =FE8U_MoveMapRows @ r2 = FE8U_RangeMapRows
	ldr r2, [r2]
	lsl r0, #2 @ r0 = 4*mStartY
	
	add r5, r2, r0 @ r5 = mIt = FE8U_RangeMapRows + 4*mStartY
	@ FREE: r2 (FE8U_RangeMapRows), r0 (4*mStartY)
	
	@ CURRENT:
		@ *r0 is free
		@ r1 = tStartY
		@ *r2 is free
		@ r3 = sizeY
		@ r4 = tSizeX
		@ r5 = mIt
		@ r6 = sizePair
		@ r7 = startPair
		@ (r8 = r2)
	
	@ tIt = r2 + 4 + tSizeX*tStartY
	
	mul r1, r4 @ r1 = tSizeX*tStartY
	add r1, #4 @ r1 = 4 + tSizeX*tStartY
	add r8, r1 @ r8 = tIt = r2 + 4 + tSizeX*tStartY
	@ FREE: r1 (tStartY), r4 (tSizeX)
	
	@ CURRENT:
		@ *r0 is free
		@ *r1 is free
		@ *r2 is free
		@ r3 = sizeY
		@ *r4 is free
		@ r5 = mIt
		@ r6 = sizePair
		@ r7 = startPair
		@ r8 = tIt
	
	@ mEndIt = mIt + 4*sizeY
	
	lsl r3, #2 @ r3 = 4*sizeY
	add r4, r5, r3 @ r4 = mEndIt = mIt + 4*sizeY
	@ FREE: r3 (sizeY)
	
	@ CURRENT:
		@ *r0 is free
		@ *r1 is free
		@ *r2 is free
		@ *r3 is free
		@ r4 = mEndIt
		@ r5 = mIt
		@ r6 = sizePair
		@ r7 = startPair
		@ r8 = tIt
	
	@ Nothing in scratch registers: SUCCESS
	
	@ for (; mIt != mEndIt; ++mIt)
	@     memcopy(*mIt + startPair.first, tIt + startPair.second, sizePair.first)
	@     tIt = tIt + sizePair.second
	
	@ Goto end if (xSize <= 0)
	get_pair_first r0, r6
	ble End
	
	@ Goto end if (mIt >= mEndIt)
	cmp r5, r4
	bge End
	
StartYLoop:
	ldmia r5!, {r0} @ r0 = *(mIt++)
	
	get_pair_first r3, r7 @ r3 = startPair.first
	add r0, r3 @ r0 = *mIt + startPair.first
	
	get_pair_second r1, r7 @ r1 = startPair.second
	add r1, r8 @ r1 = tIt + startPair.second
	
	get_pair_first r2, r6 @ r2 = sizePair.first
	
	@ CURRENT:
		@ r0 = *mIt + startPair.first
		@ r1 = tIt + startPair.second
		@ r2 = sizePair.first
		@ *r3 is free
		@ r4 = mEndIt
		@ r5 = mIt
		@ r6 = sizePair
		@ r7 = startPair
		@ r8 = tIt
		@ *r12 is free

ContinueXLoop:
	@ Load template value
	ldrb r3, [r1]
	mov r12, r3
	
	@ Add value in range map
	ldrb r3, [r0]
	add r3, r12
	strb r3, [r0]
	
	@ Increment Iteration
	add r0, #1
	add r1, #1
	
	@ Decrement Counter
	sub r2, #1
	bgt ContinueXLoop
	
	get_pair_second r0, r6 @ r0 = sizePair.second
	add r8, r0 @ tIt = tIt + sizePair.second
	
	cmp r5, r4
	bne StartYLoop
	
End:
	pop {r4} @ pop r8
	mov r8, r4
	
	pop {r4-r7}
	
	pop {r1}
	bx r1

.ltorg
.align
