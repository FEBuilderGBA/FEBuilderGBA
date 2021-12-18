.thumb

.global NuAiFillDangerMap
.type NuAiFillDangerMap, %function


@for FE8U
.equ origin, 0x0803e320	@{U}
.equ GetUnitStruct, . + 0x08019430 - origin	@{U}
.equ AreUnitsAllied, . + 0x08024D8C - origin	@{U}
.equ CanUnitUseAsWeapon, . + 0x08016574 - origin	@{U}
.equ GetItemMight, . + 0x080175DC - origin	@{U}
.equ CouldUnitBeInRangeHeuristic, . + 0x0803AC90 - origin	@{U}
.equ FillMovementAndRangeMapForItem, . + 0x0803B558 - origin	@{U}
.equ GetUnitPower, . + 0x080191B0 - origin	@{U}
.equ gUnitArray, 0x0202BE4C	@{U}
.equ SizeOfUnitStruct,	0x48	@{U}
.equ gUnitArrayTerm, gUnitArray + (0x85 * 0x48)	@Player+Enemy+NPC	@{U}
.equ gMapRange, 0x0202E4E4	@{U}
.equ gMapMovement2, 0x0202E4F0	@{U}
.equ gMapSize, 0x0202E4D4	@{U}
.equ gActiveUnitIndex, 0x0202BE44	@{U}
.equ gActiveUnit, 0x03004E50	@{U}

@for FE8J
@.equ origin, 0x0803E2B0	@{J}
@.equ GetUnitStruct, . + 0x08019108 - origin	@{J}
@.equ AreUnitsAllied, . + 0x08024d3c - origin	@{J}
@.equ CanUnitUseAsWeapon, . + 0x0801631c - origin	@{J}
@.equ GetItemMight, . + 0x08017384 - origin	@{J}
@.equ CouldUnitBeInRangeHeuristic, . + 0x0803acb0 - origin	@{J}
@.equ FillMovementAndRangeMapForItem, . + 0x0803b560 - origin	@{J}
@.equ GetUnitPower, . + 0x08018ec4 - origin	@{J}
@.equ gUnitArray, 0x0202BE48	@{J}
@.equ SizeOfUnitStruct,	0x48	@{J}
@.equ gUnitArrayTerm, gUnitArray + (0x85 * 0x48)	@Player+Enemy+NPC	@{J}
@.equ gMapRange, 0x0202E4E0	@{J}
@.equ gMapMovement2, 0x0202E4EC	@{J}
@.equ gMapSize, 0x0202E4D0	@{J}
@.equ gActiveUnitIndex, 0x0202BE40	@{J}
@.equ gActiveUnit, 0x03004DF0	@{J}

@for FE7J
@.equ origin, 0x0803989c	@{J}
@.equ GetUnitStruct, . + 0x080190f4 - origin	@{J}
@.equ AreUnitsAllied, . + 0x08023D3C - origin	@{J}
@.equ CanUnitUseAsWeapon, . + 0x08016620 - origin	@{J}
@.equ GetItemMight, . + 0x080176E8 - origin	@{J}
@.equ CouldUnitBeInRangeHeuristic, . + 0x0803627c - origin	@{J}
@.equ FillMovementAndRangeMapForItem, . + 0x08036b28 - origin	@{J}
@.equ GetUnitPower, . + 0x08018ec0 - origin	@{J}
@.equ gUnitArray, 0x0202BD4C	@{J}
@.equ SizeOfUnitStruct,	0x48	@{J}
@.equ gUnitArrayTerm, gUnitArray + (0x85 * 0x48)	@Player+Enemy+NPC	@{J}
@.equ gMapRange, 0x0202E3E4	@{J}
@.equ gMapMovement2, 0x0202E3F0	@{J}
@.equ gMapSize, 0x0202E3D4	@{J}
@.equ gActiveUnitIndex, 0x0202BD44	@{J}
@.equ gActiveUnit, 0x030045B0	@{J}


@for FE7U
@.equ origin, 0x080393E8	@{U}
@.equ GetUnitStruct, . + 0x08018D0C - origin	@{U}
@.equ AreUnitsAllied, . + 0x080238B0 - origin	@{U}
@.equ CanUnitUseAsWeapon, . + 0x080161A4 - origin	@{U}
@.equ GetItemMight, . + 0x080172E0 - origin	@{U}
@.equ CouldUnitBeInRangeHeuristic, . + 0x08035DA4 - origin	@{U}
@.equ FillMovementAndRangeMapForItem, . + 0x08036650 - origin	@{U}
@.equ GetUnitPower, . + 0x08018AD0 - origin	@{U}
@.equ gUnitArray, 0x0202BD50	@{U}
@.equ SizeOfUnitStruct,	0x48	@{U}
@.equ gUnitArrayTerm, gUnitArray + (0x85 * 0x48)	@Player+Enemy+NPC	@{U}
@.equ gMapRange, 0x0202E3E8	@{U}
@.equ gMapMovement2, 0x0202E3F4	@{U}
@.equ gMapSize, 0x0202E3D8	@{U}
@.equ gActiveUnitIndex, 0x0202BD48	@{U}
@.equ gActiveUnit, 0x03004690	@{U}


@最強に早いルーチン!!!!!!

NuAiFillDangerMap:
	push {r4,r5,r6,r7,lr}
	mov r7, r11
	mov r6, r10
	mov r5, r9
	mov r4, r8
	push {r4,r5,r6,r7}

	ldr r4, =gUnitArray

	@これいる?
	LDR  r0, =gActiveUnitIndex
	LDRB r0, [r0, #0x0]  @ ActiveUnit->Number
	mov r10, r0          @ ActiveUnit->Number

	LDR     r0, =gActiveUnit
	LDR     r0, [r0, #0x0]  @ active unit
	mov r11, r0             @ActiveUnit
	@maybe assert(ldrb [r11,#0xb] == r10 )

	UnitLoop:
	ldr r0, [r4, #0x0]	@Unit->Unit
	cmp r0, #0x0
	beq ContinueNextUnit	@ if unit->unit == null

	LDR     r2, [r0, #0xC]	@Unit->Status
	LDR     r3, =0x0000100D
	TST     r2, r3
	BNE     ContinueNextUnit    @ if unit->state & (hidden|dead|undeployed|bit12)

	mov     r0, r10
	LDRB    r1, [r4, #0xB]      @unit->Number
	BL      AreUnitsAllied
	CMP     r0, #0x1
	BNE     ContinueNextUnit    @ if allied with activeUnit

	@check items
	mov     r0, #0x0
	mov     r9, r0        @best item
	mov     r1, #0x1E
	add     r5, r4, r1    @loop item addr
	mov     r1, #0x2 * 5  @アイテムは5個
	add     r6, r5, r1    @loop end item addr
	mov     r7, r0        @best item might
	mov     r8, r0        @item temp

	ItemLoop:
		ldrh    r1, [r5]
		cmp     r1, #0x0
		beq     BreakItemLoop
		
		mov     r8, r1  @save temp item

		mov     r0, r4  @unit
		@mov     r1, r8  @item
		BL      CanUnitUseAsWeapon
		CMP     r0, #0x1
		BNE     ContinueNextItem    @ if CanUnitUseAsWeapon(unit, item)

		mov     r0, r8  @item
		BL      GetItemMight
		cmp     r0, r7
		blt     ContinueNextItem    @ if might_tmp > might

		mov     r7, r0  @update best item might
		mov     r9, r8  @update best item
	
		ContinueNextItem:
		cmp     r5, r6
		bge     BreakItemLoop
		add     r5, #0x2
		b       ItemLoop

	BreakItemLoop:
	mov r0, r9  @best item
	cmp r0, #0x0
	beq ContinueNextUnit

	mov     r0, r11   @active unit
	mov     r1, r4    @unit
	mov     r2, r9    @item
	BL      CouldUnitBeInRangeHeuristic
	cmp     r0, #0x1
	bne     ContinueNextUnit

    mov     r0, r4    @unit
    mov     r1, r9    @item
    BL      FillMovementAndRangeMapForItem

    mov     r0, r4    @unit
	BL      GetUnitPower
	
	@attack_value = (unit_power + might) >> 1
	add     r7, r0    @ might+unit_power
	asr     r7, #0x1  @ >>1

	LDR     r0, =gMapRange
	LDR     r4, [r0]
	LDR     r0, =gMapMovement2
	LDR     r5, [r0]

	LDR     r3, =gMapSize
	LDRH    r0, [r3, #0x0]  @MapWidth
	mov     r8, r0

	LDRH    r6, [r3, #0x2]  @MapHeight


	LoopY:
	cmp r6, #0x0
	beq ContinueNextUnit
	sub r6, #0x1  @iy --
	mov r3, r8    @ix = init

		LoopX:
		cmp r3, #0x0
		beq LoopY
		sub r3, #0x1  @ix --

			lsl     r2, r6, #0x2  @iy * sizeof(pointer)
			ldr     r0, [r4, r2]
			ldrb    r0, [r0, r3]
			cmp     r0, #0x0
			beq     LoopX         @ gMapRange[iy][ix] == 0

			ldr     r0, [r5, r2]
			ldrb    r1, [r0, r3]
			add     r1, r7
			strb    r1, [r0, r3]  @ gMapOther[iy][ix] += attack_value
			b       LoopX

	ContinueNextUnit:
	add r4, #SizeOfUnitStruct   @unit++  @next unit

	ldr r0, =gUnitArrayTerm	@Player+Enemy+NPC
	cmp r4, r0
	blt UnitLoop

	pop {r0,r1,r2,r3,r4,r5,r6,r7}
	mov r8, r0
	mov r9, r1
	mov r10, r2
	mov r11, r3
	pop {r0}
	bx r0

.align
.ltorg
