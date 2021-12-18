.thumb

.global MapAddInRange7743
.type MapAddInRange7743, %function


@for FE8U
.equ gMapSize, 0x0202E4D4		@{U}
.equ gSubjectMap, 0x030049A0	@{U}

@for FE8J
@.equ gMapSize, 0x0202E4D0		@{J}
@.equ gSubjectMap, 0x03004940	@{J}

@for FE7J
@.equ gMapSize, 0x0202E3D4		@{J}
@.equ gSubjectMap, 0x03004100	@{J}

@for FE7U
@.equ gMapSize, 0x0202E3D8		@{U}
@.equ gSubjectMap, 0x030041E0	@{U}


@最強に早いルーチン!!!!!!
@バニラの5倍速い

@r0 x
@r1 y
@r2 rang
@r3 value 1 or -1
MapAddInRange7743:
	push {r4,r5,r6,r7,lr}
	mov r7, r11
	mov r6, r10
	mov r5, r9
	mov r4, r8
	push {r4,r5,r6,r7}

	@if (range == 0)
	cmp r2, #0x0
	bne ForUp

ForRange0:
	@if (x < 0 || x >= gBmMapSize.x) return;
	cmp  r0, #0x0
	blt  Exit

	LDR  r5, =gMapSize
	LDRH r2, [r5, #0x0]  @MapWidth
	cmp  r0, r2
	bge  Exit

	@if (y < 0 || y >= gBmMapSize.y) return;
	cmp  r1, #0x0
	blt  Exit

	LDRH r2, [r5, #0x2]  @MapHeight
	cmp  r1, r2
	bge  Exit

	@gWorkingBmMap[y][x] += value;
	LDR     r4, =gSubjectMap
	LDR     r4, [r4]

	lsl  r1, r1, #0x2
	ldr  r4, [r4, r1]	@gWorkingBmMap[iy]
	add  r4, r0
	ldrb r1, [r4]
	add  r1, r3
	strb r1, [r4]
	b    Exit

ForUp:
	mov  r11, r3 @value (+=は高レジスタも可なので)
	mov  r3, r2  @range backup
	mov  r8, r1  @y backup
	mov  r7, r0  @x backup
	mov  r6, r2  @iRange
	mov  r5, r1  @iy

	LDR  r1, =gMapSize
	LDRH r0, [r1, #0x0]  @MapWidth
	mov  r10, r0

	LDRH r0, [r1, #0x2]  @MapHeight
	mov  r9, r0

	LDR r0, =gSubjectMap
	LDR r4, [r0]

	@for(iRange = range ; irange <= 1 ; iRange-- )
	ForUp_Loop:
		@iy = y - iRange;
		mov r0, r8	@y backup
		sub r5, r0, r6
		
		@if (iy < 0)	continue;
		@cmp r5, #0x0	@直前がsubで同レジスタなのでなので不要
		blt ForUp_Continue	@continue
		
		@if (iy >= gBmMapSize.y) return;
		cmp  r5, r9
		bge  Exit

		lsl r0, r5, #0x2
		ldr r5, [r4, r0]	@gWorkingBmMap[iy]

		ForUp_CheckxMax:
		@xMax = x + (range - iRange);
		sub r0, r3, r6
		add r2, r7, r0
		add r2, #0x1

		@if (xMax > gBmMapSize_x)	xMax = gBmMapSize_x;
		cmp  r2, r10
		ble  ForUp_MakexMaxP
		mov  r2, r10

		ForUp_MakexMaxP:
		add  r2, r2, r5		@&gWorkingBmMap[iy][iMax]

		ForUp_CheckLower:
		@xMin = x - (range - iRange);
		sub r0, r3, r6
		sub r0, r7, r0

		@if (xMin < 0)	xMin = 0;
		@cmp r0, #0x0	@直前がsubで同レジスタなのでなので不要
		bge ForUp_MakexMin
		mov r0, #0x0

		ForUp_MakexMin:
		add r0 , r5			@&gWorkingBmMap[iy][xMin]

		@for (ix = xMin; ix < xMax; ++ix) gWorkingBmMap[iy][ix] += value;
		ForUp_AddLoop:
			ldrb r1, [r0]
			add  r1, r11
			strb r1, [r0]

			add r0, #0x1
			cmp r0, r2      @ix < xMax
			blt ForUp_AddLoop

	ForUp_Continue:
		sub r6, #0x1
		@cmp r6, #0x0	@直前がsubで同レジスタなのでなので不要
		bgt ForUp_Loop

ForDown:
	mov  r6, #0x0 @iRange = range backup
	mov  r5, r8   @iy  = y backup

	@for(iRange = 0 ; iRange > range ; iRange++ )
	ForDown_Loop:

		@iy = y + iRange;
		mov r0, r8	@y backup
		add r5, r0, r6

		@if (iy < 0)	continue;
		@cmp r5, #0x0	@直前がsubで同レジスタなのでなので不要
		blt ForDown_Continue	@continue

		@if (iy >= gBmMapSize_y) return
		cmp  r5, r9
		bge  Exit

		lsl r0, r5, #0x2
		ldr r5, [r4, r0]	@gWorkingBmMap[iy]

		ForDown_CheckxMax:
		@xMax = x + (range - iRange);
		sub r0, r3, r6
		add r2, r7, r0
		add r2, #0x1

		@if (xMax > gBmMapSize_x)	xMax = gBmMapSize_x;
		cmp  r2, r10
		ble  ForDown_MakexMaxP
		mov  r2, r10

		ForDown_MakexMaxP:
		add  r2, r2, r5		@&gWorkingBmMap[iy][iMax]

		ForDown_CheckLower:
		@xMin = x - (range - iRange);
		sub r0, r3, r6
		sub r0, r7, r0

		@if (xMin < 0)	xMin = 0;
		@cmp r0, #0x0	@直前がsubで同レジスタなのでなので不要
		bge ForDown_MakexMin
		mov r0, #0x0

		ForDown_MakexMin:
		add r0 , r5			@&gWorkingBmMap[iy][xMin]

        @for (ix = xMin; ix < xMax; ++ix) gWorkingBmMap[iy][ix] += value;
		ForDown_AddLoop:
			ldrb r1, [r0]
			add  r1, r11
			strb r1, [r0]

			add r0, #0x1
			cmp r0, r2      @ix < xMax
			blt ForDown_AddLoop

	ForDown_Continue:
		add r6, #0x1
		cmp r6, r3
		ble ForDown_Loop

Exit:

	pop {r0,r1,r2,r3,r4,r5,r6,r7}
	mov r8, r0
	mov r9, r1
	mov r10, r2
	mov r11, r3
	pop {r0}
	bx r0

.align
.ltorg
