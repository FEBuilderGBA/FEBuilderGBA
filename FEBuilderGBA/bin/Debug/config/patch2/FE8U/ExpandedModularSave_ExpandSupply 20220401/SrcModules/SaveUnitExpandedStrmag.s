
	.thumb

	@ THE PLAN:

	@ GameSaveUnit:
	@   +00 | u8 character
	@   +01 | u8 class
	@   +02 | u8 mhp
	@   +03 | u8 str / pow
	@   +04 | u8 mag
	@   +05 | u8 skl
	@   +06 | u8 spd
	@   +07 | u8 def
	@   +08 | u8 res
	@   +09 | u8 lck
	@   +0A | u8 conBonus
	@   +0B | u8 movBonus
	@   +0C | 3 bytes:
	@       +000 | xPos  : 6
	@       +006 | yPos  : 6
	@       +012 | level : 5
	@       +017 | exp   : 7
	@   +0F | u8[8] wexp
	@   +17 | u8[7] supports
	@   +1E | u16[5] items
	@   +28 | u32 state
	@   +2C | (end)

	GameSaveUnit.size = 0x2C

	@ SuspendSaveUnit (Common):
	@   +00 | <GameSaveUnit>
	@   +2C | u8 chp
	@   +2D | u8 rescueUnit
	@   +2E | u8 ballista
	@   +2F | u8 status/duration
	@   +30 | u8 torch/purewater
	@   +31 | (end)

	@ SuspendSaveUnit (Blue):
	@   +00 | <SuspendSaveUnit>
	@   +31 | u8 support gains
	@   +32 | (end, 2 byte padding)

	PlayerSuspendSaveUnit.size = 0x34

	@ SuspendSaveUnit (Other):
	@   +00 | <SuspendSaveUnit>
	@   +31 | u8 ai1
	@   +32 | u8 ai2
	@   +33 | u8 ai1Cur
	@   +34 | u8 ai2Cur
	@   +35 | u8 aiFlag
	@   +36 | u16 aiConf @ "ai 3 & 4"
	@   +38 | (end, no padding)

	OtherSuspendSaveUnit.size = 0x38

	GetUnit          = 0x08019430+1
	ClearUnit        = 0x080177F4+1
	GetCharacterData = 0x08019464+1
	GetClassData     = 0x08019444+1
	SetUnitHp        = 0x08019368+1
	GetUnitMaxHp     = 0x08019190+1

	WriteAndVerifySramFast = 0x080D184C+1
	ReadSramFastAddr       = 0x030067A0   @ pointer to the actual ReadSramFast function

PackGameSaveUnit.clear_unit:
	@ clear unit if it's character is null

	push {r0-r2}

	ldr r3, =ClearUnit

	mov r0, r1

	bl  BXR3

	pop {r0-r2}

	b PackGameSaveUnit.set_character

PackGameSaveUnit:
	@ arguments:
	@ - r0 = target buffer address
	@ - r1 = source unit
	@ returns:
	@ - r0 = target buffer address

	push {r4-r5, lr}

	@ CHARACTER

	ldr  r2, [r1, #0x00] @ r2 = Unit->pCharacterData

	cmp  r2, #0
	beq PackGameSaveUnit.clear_unit

	ldrb r2, [r2, #0x04] @ r2 = Unit->pCharacterData->id

PackGameSaveUnit.set_character:
	strb r2, [r0, #0x00] @ GameSaveUnit->character = Unit->pCharacterData->id

	@ CLASS

	ldr  r2, [r1, #0x04] @ r2 = Unit->pClassData

	cmp  r2, #0
	beq PackGameSaveUnit.set_class

	ldrb r2, [r2, #0x04] @ r2 = Unit->pClassData->id

PackGameSaveUnit.set_class:
	strb r2, [r0, #0x01] @ GameSaveUnit->class = Unit->pClassData->id

	@ STATS

	ldrb r2, [r1, #0x12] @ r2 = Unit->mhp
	strb r2, [r0, #0x02] @ GameSaveUnit->mhp = Unit->mhp

	ldrb r2, [r1, #0x14] @ r2 = Unit->pow
	strb r2, [r0, #0x03] @ GameSaveUnit->str = Unit->pow

	mov r3, #0x3A
	ldrb r2, [r1, r3] @ r2 = Unit->mag
	strb r2, [r0, #0x04] @ GameSaveUnit->mag = Unit->mag

	ldrb r2, [r1, #0x15] @ r2 = Unit->skl
	strb r2, [r0, #0x05] @ GameSaveUnit->skl = Unit->skl

	ldrb r2, [r1, #0x16] @ r2 = Unit->spd
	strb r2, [r0, #0x06] @ GameSaveUnit->spd = Unit->spd

	ldrb r2, [r1, #0x17] @ r2 = Unit->def
	strb r2, [r0, #0x07] @ GameSaveUnit->def = Unit->def

	ldrb r2, [r1, #0x18] @ r2 = Unit->res
	strb r2, [r0, #0x08] @ GameSaveUnit->res = Unit->res

	ldrb r2, [r1, #0x19] @ r2 = Unit->lck
	strb r2, [r0, #0x09] @ GameSaveUnit->lck = Unit->lck

	ldrb r2, [r1, #0x1A] @ r2 = Unit->conBonus
	strb r2, [r0, #0x0A] @ GameSaveUnit->conBonus = Unit->conBonus

	ldrb r2, [r1, #0x1D] @ r2 = Unit->movBonus
	strb r2, [r0, #0x0B] @ GameSaveUnit->movBonus = Unit->movBonus

	@ THE ONE PACKED BIT (position, level, exp)

	mov  r4, #0x3F @ 6 bits set

	ldrb r2, [r1, #0x10] @ r2 = Unit->xPos
	and  r2, r4

	ldrb r3, [r1, #0x11] @ r3 = Unit->yPos
	and  r3, r4

	lsl  r3, #6
	orr  r2, r3 @ r2 |= Unit->yPos << 6

	mov  r4, #0x1F @ 5 bits set

	ldrb r3, [r1, #0x08] @ r3 = Unit->level
	and  r3, r4

	lsl  r3, #12
	orr  r2, r3 @ r2 |= Unit->level << 12

	ldrb r3, [r1, #0x09] @ r3 = Unit->exp
	@ no and @ this will get truncated anyway

	lsl  r3, #17
	orr  r2, r3 @ r2 |= Unit->level << 17

	strh r2, [r0, #0x0C]
	lsr  r2, #16
	strb r2, [r0, #0x0E]

	@ WEAPON EXP

	mov r3, #0x0F
	add r3, r0 @ r3 = &GameSaveUnit->wexp

	mov r4, #0x28
	add r4, r1 @ r4 = &Unit->wexp

	mov r5, #7

PackGameSaveUnit.lop_wexp:
	ldrb r2, [r4, r5]
	strb r2, [r3, r5]

	sub r5, #1
	bge PackGameSaveUnit.lop_wexp

	@ SUPPORTS

	mov r3, #0x17
	add r3, r0 @ r3 = &GameSaveUnit->supports

	mov r4, #0x32
	add r4, r1 @ r4 = &Unit->supports

	mov r5, #6

PackGameSaveUnit.lop_supports:
	ldrb r2, [r4, r5]
	strb r2, [r3, r5]

	sub r5, #1
	bge PackGameSaveUnit.lop_supports

	@ ITEMS

	@ Okay so here we're going to be sneaky
	@ Since the Unit's and GameSaveUnit items happen to both start at +1E
	@ We'll use the same "offset" register for both.

	mov r3, #0x1E @ r3 = offsetof(Unit, items) = offsetof(GameSaveUnit, items)
	mov r5, #4

PackGameSaveUnit.lop_items:
	ldrh r2, [r1, r3]
	strh r2, [r0, r3]

	add r3, #2

	sub r5, #1
	bge PackGameSaveUnit.lop_items

	@ STATE

	ldr r2, [r1, #0x0C] @ r2 = Unit->state
	str r2, [r0, #0x28] @ GameSaveUnit->state = Unit->state

	@ END

	pop {r4-r5}

	pop {r1}
	bx  r1

	.pool
	.align

UnpackGameSaveUnit:
	@ arguments:
	@ - r0 = target unit
	@ - r1 = source buffer address
	@ returns:
	@ - r0 = target unit

	push {r4-r5, lr}

	mov r4, r0 @ r4 = target unit
	mov r5, r1 @ r5 = source buffer

	@ CHARACTER

	ldr r3, =GetCharacterData

	ldrb r0, [r5, #0x00]

	bl BXR3

	str r0, [r4, #0x00]

	@ CLASS

	ldr r3, =GetClassData

	ldrb r0, [r5, #0x01]

	bl BXR3

	str  r0, [r4, #0x04]

	@ STATS

	ldrb r2, [r5, #0x02] @ r2 = GameSaveUnit->mhp
	strb r2, [r4, #0x12] @  = GameSaveUnit->mhp

	ldrb r2, [r5, #0x03] @ r2 = GameSaveUnit->pow
	strb r2, [r4, #0x14] @ GameSaveUnit->str = GameSaveUnit->pow

	ldrb r2, [r5, #0x04] @ r2 = GameSaveUnit->mag
	mov r3, #0x3A
	strb r2, [r4, r3]

	ldrb r2, [r5, #0x05] @ r2 = GameSaveUnit->skl
	strb r2, [r4, #0x15] @ Unit->skl = GameSaveUnit->skl

	ldrb r2, [r5, #0x06] @ r2 = GameSaveUnit->spd
	strb r2, [r4, #0x16] @ Unit->spd = GameSaveUnit->spd

	ldrb r2, [r5, #0x07] @ r2 = GameSaveUnit->def
	strb r2, [r4, #0x17] @ Unit->def = GameSaveUnit->def

	ldrb r2, [r5, #0x08] @ r2 = GameSaveUnit->res
	strb r2, [r4, #0x18] @ Unit->res = GameSaveUnit->res

	ldrb r2, [r5, #0x09] @ r2 = GameSaveUnit->lck
	strb r2, [r4, #0x19] @ Unit->lck = GameSaveUnit->lck

	ldrb r2, [r5, #0x0A] @ r2 = GameSaveUnit->conBonus
	strb r2, [r4, #0x1A] @ Unit->conBonus = GameSaveUnit->conBonus

	ldrb r2, [r5, #0x0B] @ r2 = GameSaveUnit->movBonus
	strb r2, [r4, #0x1D] @ Unit->movBonus = GameSaveUnit->movBonus

	@ THE ONE PACKED BIT (position, level, exp)

	ldr  r2, [r5, #0x0C]

	mov  r3, #0x3F
	and  r3, r2 @ r3 = GameSaveUnit->xPos

	cmp  r3, #0x3F
	bne  UnpackGameSaveUnit.yes_x

	mov  r3, #0xFF

UnpackGameSaveUnit.yes_x:
	strb r3, [r4, #0x10]

	lsr  r2, #6

	mov  r3, #0x3F
	and  r3, r2 @ r3 = GameSaveUnit->yPos

	cmp  r3, #0x3F
	bne UnpackGameSaveUnit.yes_y

	mov  r3, #0xFF

UnpackGameSaveUnit.yes_y:
	strb r3, [r4, #0x11]

	lsr  r2, #6

	mov  r3, #0x1F
	and  r3, r2 @ r3 = Unit->exp

	strb r3, [r4, #0x08]

	lsr  r2, #5

	mov  r3, #0x7F
	and  r3, r2 @ r3 = GameSaveUnit->exp

	cmp  r3, #0x7F
	bne UnpackGameSaveUnit.yes_exp

	mov  r3, #0xFF

UnpackGameSaveUnit.yes_exp:
	strb r3, [r4, #0x09]

	@ WEAPON EXP

	mov r0, #0x28
	add r0, r4 @ r0 = &Unit->wexp

	mov r1, #0x0F
	add r1, r5 @ r1 = &GameSaveUnit->wexp

	mov r3, #7

UnpackGameSaveUnit.lop_wexp:
	ldrb r2, [r1, r3]
	strb r2, [r0, r3]

	sub r3, #1
	bge UnpackGameSaveUnit.lop_wexp

	@ SUPPORTS

	mov r0, #0x32
	add r0, r4 @ r0 = &Unit->supports

	mov r1, #0x17
	add r1, r5 @ r1 = &GameSaveUnit->supports

	mov r3, #6

UnpackGameSaveUnit.lop_supports:
	ldrb r2, [r1, r3]
	strb r2, [r0, r3]

	sub r3, #1
	bge UnpackGameSaveUnit.lop_supports

	@ ITEMS

	@ Same trick as for packing (see PackGameSaveUnit)

	mov r1, #0x1E @ r1 = offsetof(Unit, items) = offsetof(GameSaveUnit, items)
	mov r3, #4

UnpackGameSaveUnit.lop_items:
	ldrh r2, [r5, r1]
	strh r2, [r4, r1]

	add r1, #2

	sub r3, #1
	bge UnpackGameSaveUnit.lop_items

	@ STATE

	ldr r2, [r5, #0x28] @ r2 = GameSaveUnit->state
	str r2, [r4, #0x0C] @ Unit->state = GameSaveUnit->state

	@ MISC

	@ Set current hp to max

	ldr r3, =GetUnitMaxHp

	mov r0, r4 @ GetUnitMaxHp arg r0 = Unit

	bl  BXR3

	ldr r3, =SetUnitHp

	mov r1, r0 @ SetUnitHp arg r1 = Hp
	mov r0, r4 @ SetUnitHp arg r0 = Unit

	bl  BXR3

	@ END

	mov r0, r4

	pop {r4-r5}

	pop {r3}
BXR3:
	bx  r3

	.pool
	.align

PackPlayerSuspendSaveUnit:
	@ arguments:
	@ - r0 = target buffer address
	@ - r1 = source unit
	@ returns:
	@ - r0 = target buffer address

	push {r4, lr}

	mov r4, r1

	bl PackGameSaveUnit

	mov  r2, #0x2C
	add  r2, r0 @ r2 = su+0x2C, for easier addressing (0x2C+ is out of ldrb/strb range)

	ldrb r1, [r4, #0x13] @ r1 = u->chp
	strb r1, [r2, #0x00] @ su->chp = u->chp

	ldrb r1, [r4, #0x1B] @ r1 = u->rescue
	strb r1, [r2, #0x01] @ su->rescue = u->rescue

	ldrb r1, [r4, #0x1C] @ r1 = u->blst
	strb r1, [r2, #0x02] @ su->blst = u->blst

	mov  r1, #0x30
	ldrb r1, [r4, r1]    @ r1 = u->sd
	strb r1, [r2, #0x03] @ su->sd = u->sd

	mov  r1, #0x31
	ldrb r1, [r4, r1]    @ r1 = u->tb
	strb r1, [r2, #0x04] @ su->tb = u->tb

	mov  r1, #0x39
	ldrb r1, [r4, r1]    @ r1 = u->supportbits
	strb r1, [r2, #0x05] @ su->supportbits = u->supportbits

	pop {r4}

	pop {r1}
	bx  r1

	.pool
	.align

UnpackPlayerSuspendSaveUnit:
	@ arguments:
	@ - r0 = target unit
	@ - r1 = source buffer address
	@ returns:
	@ - r0 = target unit

	push {r4, lr}

	mov r4, r1

	bl UnpackGameSaveUnit

	mov  r2, #0x2C
	add  r2, r4 @ r2 = su+0x2C, for easier addressing (0x2C+ is out of ldrb/strb range)

	ldrb r1, [r2, #0x00] @ r1 = su->chp
	strb r1, [r0, #0x13] @ u->chp = su->chp

	ldrb r1, [r2, #0x01] @ r1 = su->rescue
	strb r1, [r0, #0x1B] @ u->rescue = su->rescue

	ldrb r1, [r2, #0x02] @ r1 = su->blst
	strb r1, [r0, #0x1C] @ u->blst = su->blst

	mov  r3, #0x30
	ldrb r1, [r2, #0x03] @ r1 = su->sd
	strb r1, [r0, r3]    @ u->sd = su->sd

	mov  r3, #0x31
	ldrb r1, [r2, #0x04] @ r1 = su->tb
	strb r1, [r0, r3]    @ u->tb = su->tb

	mov  r3, #0x39
	ldrb r1, [r2, #0x05] @ r1 = su->supportbits
	strb r1, [r0, r3]    @ u->supportbits = su->supportbits

	pop {r4}

	pop {r1}
	bx  r1

	.pool
	.align

PackOtherSuspendSaveUnit:
	@ arguments:
	@ - r0 = target buffer address
	@ - r1 = source unit
	@ returns:
	@ - r0 = target buffer address

	push {r4, lr}

	mov r4, r1

	bl PackGameSaveUnit

	mov  r2, #0x2C
	add  r2, r0 @ r2 = su+0x2C, for easier addressing (0x2C+ is out of ldrb/strb range)

	ldrb r1, [r4, #0x13] @ r1 = u->chp
	strb r1, [r2, #0x00] @ su->chp = u->chp

	ldrb r1, [r4, #0x1B] @ r1 = u->rescue
	strb r1, [r2, #0x01] @ su->rescue = u->rescue

	ldrb r1, [r4, #0x1C] @ r1 = u->blst
	strb r1, [r2, #0x02] @ su->blst = u->blst

	mov  r1, #0x30
	ldrb r1, [r4, r1]    @ r1 = u->sd
	strb r1, [r2, #0x03] @ su->sd = u->sd

	mov  r1, #0x31
	ldrb r1, [r4, r1]    @ r1 = u->tb
	strb r1, [r2, #0x04] @ su->tb = u->tb

	mov  r3, #0x40
	add  r3, r4 @ r3 = unit+0x40, for easier addressing

	ldrb r1, [r3, #0x02] @ r1 = u->ai1
	strb r1, [r2, #0x05] @ su->ai1 = u->ai1

	ldrb r1, [r3, #0x04] @ r1 = u->ai2
	strb r1, [r2, #0x06] @ su->ai2 = u->ai2

	ldrb r1, [r3, #0x03] @ r1 = u->ai1Cur
	strb r1, [r2, #0x07] @ su->ai1Cur = u->ai1Cur

	ldrb r1, [r3, #0x05] @ r1 = u->ai2Cur
	strb r1, [r2, #0x08] @ su->ai2Cur = u->ai2Cur

	ldrb r1, [r4, #0x0A] @ r1 = u->aiFlag
	strb r1, [r2, #0x09] @ su->aiFlag = u->aiFlag

	ldrh r1, [r3, #0x00] @ r1 = u->aiConf
	strh r1, [r2, #0x0A] @ su->aiConf = u->aiConf

	pop {r4}

	pop {r1}
	bx  r1

	.pool
	.align

UnpackOtherSuspendSaveUnit:
	@ arguments:
	@ - r0 = target unit
	@ - r1 = source buffer address
	@ returns:
	@ - r0 = target unit

	push {r4, lr}

	mov r4, r1

	bl UnpackGameSaveUnit

	mov  r2, #0x2C
	add  r2, r4 @ r2 = su+0x2C, for easier addressing (0x2C+ is out of ldrb/strb range)

	ldrb r1, [r2, #0x00] @ r1 = su->chp
	strb r1, [r0, #0x13] @ u->chp = su->chp

	ldrb r1, [r2, #0x01] @ r1 = su->rescue
	strb r1, [r0, #0x1B] @ u->rescue = su->rescue

	ldrb r1, [r2, #0x02] @ r1 = su->blst
	strb r1, [r0, #0x1C] @ u->blst = su->blst

	mov  r3, #0x30
	ldrb r1, [r2, #0x03] @ r1 = su->sd
	strb r1, [r0, r3]    @ u->sd = su->sd

	mov  r3, #0x31
	ldrb r1, [r2, #0x04] @ r1 = su->tb
	strb r1, [r0, r3]    @ u->tb = su->tb

	mov  r3, #0x40
	add  r3, r0 @ r3 = unit+0x40, for easier addressing

	ldrb r1, [r2, #0x05] @ r1 = su->ai1
	strb r1, [r3, #0x02] @ u->ai1 = su->ai1

	ldrb r1, [r2, #0x06] @ r1 = su->ai2
	strb r1, [r3, #0x04] @ u->ai2 = su->ai2

	ldrb r1, [r2, #0x07] @ r1 = su->ai1Cur
	strb r1, [r3, #0x03] @ u->ai1Cur = su->ai1Cur

	ldrb r1, [r2, #0x08] @ r1 = su->ai2Cur
	strb r1, [r3, #0x05] @ u->ai2Cur = su->ai2Cur

	ldrb r1, [r2, #0x09] @ r1 = su->aiFlag
	strb r1, [r0, #0x0A] @ u->aiFlag = su->aiFlag

	ldrh r1, [r2, #0x0A] @ r1 = su->aiConf
	strh r1, [r3, #0x00] @ u->aiConf = su->aiConf

	pop {r4}

	pop {r1}
	bx  r1

	.pool
	.align

	.macro SAVE_UNIT_FUNC Name, PackFunc, StructSize, Allegiance

		.align

		.global \Name
		.type   \Name , function

	\Name :
		@ arguments:
		@ - r0 = target address (SRAM)
		@ - r1 = target size

		push {r4-r6, lr}

		sub sp, #(\StructSize)

		mov r4, r0 @ r4 = target

		mov r0, r1
		mov r1, #(\StructSize)

		svc 6 @ Div

		mov r6, r0 @ r6 = max number of written units

		mov r5, #(\Allegiance + 1) @ r5 = unit id

	\Name\().lop :
		ldr r3, =GetUnit

		mov r0, r5 @ GetUnit arg r0 = id

		bl  BXR3

		cmp r0, #0
		beq \Name\().end

		mov r1, r0
		mov r0, sp

		bl \PackFunc

		ldr r3, =WriteAndVerifySramFast

		@ implied              @ WriteAndVerifySramFast arg r0 = src
		mov r1, r4             @ WriteAndVerifySramFast arg r1 = dst
		mov r2, #(\StructSize) @ WriteAndVerifySramFast arg r2 = size

		bl  BXR3

		add r4, #(\StructSize) @ target++
		add r5, #1             @ id++

		sub r6, #1
		bgt \Name\().lop

	\Name\().end :
		add sp, #(\StructSize)

		pop {r4-r6}

		pop {r0}
		bx r0

		.pool

	.endm @ SAVE_UNIT_FUNC

	.macro LOAD_UNIT_FUNC Name, UnpackFunc, StructSize, Allegiance

		.align

		.global \Name
		.type   \Name , function

	\Name :
		@ arguments:
		@ - r0 = source address (SRAM)
		@ - r1 = source size

		push {r4-r6, lr}

		sub sp, #(\StructSize)

		mov r4, r0 @ r4 = source

		mov r0, r1
		mov r1, #(\StructSize)

		svc 6 @ Div

		mov r6, r0 @ r6 = max number of read units

		mov r5, #(\Allegiance + 1) @ r5 = unit id

	\Name\().lop :
		ldr r3, =ReadSramFastAddr
		ldr r3, [r3]

		mov r0, r4             @ ReadSramFast arg r0 = src
		mov r1, sp             @ ReadSramFast arg r1 = dst
		mov r2, #(\StructSize) @ ReadSramFast arg r2 = size

		bl  BXR3

		ldr r3, =GetUnit

		mov r0, r5 @ GetUnit arg r0 = id

		bl  BXR3

		cmp r0, #0
		beq \Name\().end

		@ implied  @ Unpack arg r0 = target Unit
		mov r1, sp @ Unpack arg r1 = source GameSaveUnit

		bl \UnpackFunc

		add r4, #(\StructSize) @ source++
		add r5, #1             @ id++

		sub r6, #1
		bgt \Name\().lop

	\Name\().end :
		add sp, #(\StructSize)

		pop {r4-r6}

		pop {r0}
		bx r0

		.pool

	.endm @ LOAD_UNIT_FUNC

	@ Saved units
	SAVE_UNIT_FUNC ESU_SaveGameUnits, PackGameSaveUnit,   GameSaveUnit.size, 0x00
	LOAD_UNIT_FUNC ESU_LoadGameUnits, UnpackGameSaveUnit, GameSaveUnit.size, 0x00

	@ Suspend player units
	SAVE_UNIT_FUNC ESU_SavePlayerSuspendUnits, PackPlayerSuspendSaveUnit,   PlayerSuspendSaveUnit.size, 0x00
	LOAD_UNIT_FUNC ESU_LoadPlayerSuspendUnits, UnpackPlayerSuspendSaveUnit, PlayerSuspendSaveUnit.size, 0x00

	@ Suspend green units
	SAVE_UNIT_FUNC ESU_SaveGreenSuspendUnits, PackOtherSuspendSaveUnit,   OtherSuspendSaveUnit.size, 0x40
	LOAD_UNIT_FUNC ESU_LoadGreenSuspendUnits, UnpackOtherSuspendSaveUnit, OtherSuspendSaveUnit.size, 0x40

	@ Suspend red units
	SAVE_UNIT_FUNC ESU_SaveRedSuspendUnits, PackOtherSuspendSaveUnit,   OtherSuspendSaveUnit.size, 0x80
	LOAD_UNIT_FUNC ESU_LoadRedSuspendUnits, UnpackOtherSuspendSaveUnit, OtherSuspendSaveUnit.size, 0x80
