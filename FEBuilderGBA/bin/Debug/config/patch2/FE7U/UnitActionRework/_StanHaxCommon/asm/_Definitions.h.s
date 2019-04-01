.thumb

.macro _blh to, reg=r3
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm

.macro _blr reg
	mov lr, \reg
	.short 0xF800
.endm

@ (rd != rox)) MUST be true
.macro _MakePair rd, rs1, rs2, rox=r3
	lsl \rox, \rs1, #16 @ clearing top 16 bits of part 1
	lsl \rd,  \rs2, #16 @ clearing top 16 bits of part 2
	lsr \rox,       #16 @ shifting back part 1
	orr \rd, \rox       @ OR
.endm

.macro _GetPairFirst rd, rs
	lsl \rd, \rs, #16 @ clearing second part of pair
	asr \rd, \rd, #16 @ shifting back
.endm

.macro _GetPairSecond rd, rs
	asr \rd, \rs, #16 @ shifting second part of pair (erasing first part in the process)
.endm

@ NOTE: not sure if working atm
.macro _MakeSign rd, rs, rox=r3
	neg \rox, \rs
    asr \rox, \rox, #31
    asr \rd,  \rs,  #31
    sub \rd,  \rox
.endm

.set prForEachAdjacentUnit,      0x08023A74 @ arguments: r0 = x, r1 = y, r2 = function<void(UnitStruct*)>; returns: nothing
.set prAddTargetListEntry,       0x0804ACFC @ arguments: r0 = x, r1 = y, r2 = unit allegience byte, r3 = trap type; returns: nothing
.set prGetTargetListSize,        0x0804b174 @ arguments: nothing; returns: r0 = current target list size

.set prScheduleRoutineCall,      0x08014B50 @ arguments: r0 = routine to call, r1 = argument, r2 = time (in frames) before call happens

.set prGetFacingDirectionId,     0x0806F478 @ arguments: r0 = xSource, r1 = ySource, r2 = xTarget, r3 = yTarget
.set prChangeActiveUnitFacing,   0x0801EC10 @ arguments: r0 = xTarget, r1 = yTarget

.set prIDunnoReallyButIThinkItUpdatesStandingSprites, 0x08025724

.set prMap_Fill,                 0x080190AC @ arguments: r0 = rows start ptr, r1 = value; returns: nothing

.set prItem_GetIndex,            0x080171B4 @ arguments: r0 = item short; returns: r0 = item index (= (item & 0xFF))

.set prUnit_GetStruct,           0x08018D0C @ arguments: r0 = Unit Allegience Index; returns: r0 = Unit Struct pointer (0 if not found)
.set prUnit_ApplyMovement,       0x08017F28 @ arguments: r0 = Unit Struct pointer
.set prUnit_CanCrossTerrain,     0x08018D68 @ arguments: r0 = Unit Struct pointer, r1 = Terrain Index; returns: r0 = 0 if Unit cannot cross/stand on terrain
.set prUnit_GetEquippedWeapon,   0x08016764 @ arguments: r0 = Unit Struct pointer; returns: r0 = Item Short
.set prUnit_GetItemCount,        0x080176DA @ arguments: r0 = Unit Struct pointer; returns: r0 = Item Count
.set prUnit_CanUseAsStaff,       0x080163D2 @ arguments: r0 = Unit Struct pointer, r1 = Item Short; returns: r0 = 0 if cannot use

.set prUnit_GetDefense,          0x08018B70 @ arguments: r0 = Unit Struct pointer; returns: r0 = Unit Computed Defense

.set prItem_GetMight,            0x080172E0 @ arguments: r0 = Item Short; returns: r0 = Might	//!
.set prItem_GetAttributes,       0x0801722C @ arguments: r0 = Item Short; returns: r0 = Attribute Word
.set prItem_GetWType,            0x080170FC @ arguments: r0 = Item Short; returns: r0 = WType
.set prItem_GetWRank,            0x080173B8 @ arguments: r0 = Item Short; returns: r0 = WRank
.set prItem_GetUseEffect,        0x0801722C @ arguments: r0 = Item Short; returns: r0 = Use Effect

.set prGetTextInBuffer,          0x08012C60 @ arguments: r0 = text id; returns: r0 = buffer in which the text now is

.set prBottomHelpDisplay_New,    0x08032560 @ arguments: r0 = parent 6C, r1 = pointer to text IN BUFFER

.set prTargetSelection_New,      0x0804AE88 @ arguments: r0 = pointer to Target Selection Definition

.set prMoveRange_ShowGfx,        0x0801D2A0 @ arguments: r0 = type bitfield (&1 = Move Blue Squares, &2 = Range Red Squares, &4 = Range Green Squares, &16 = Range Blue Squares)
.set prMoveRange_HideGfx,        0x0801D2D4 @ none

.set prMOVEUNIT_NewForMapUnit,   0x0806BAF0 @ arguments: r0 = pointer to Unit Struct; returns: r0 = new MOVEUNIT pointe	r
.set prMOVEUNIT_SetMovement,     0x0806C104 @ arguments: r0 = pointer to MOVEUNIT, r1 = pointer to movement buffer
.set prMOVEUNIT_SetSprDirection, 0x0806BF4C @ arguments: r0 = pointer to MOVEUNIT, r1 = direction id (use prGetFacingDirectionId, or 0xB for idle)
.set prMOVEUNIT_DeleteAll,       0x0806CCB8 @ none

.set pr6C_New,                   0x08004494 @ arguments: r0 = pointer to ROM 6C code, r1 = parent; returns: r0 = new 6C pointer (0 if no space available)
.set pr6C_NewBlocking,           0x080044F8 @ same
.set pr6C_Delete,                0x08004584 @ arguments: r0 = pointer to 6C to delete
.set pr6C_BreakLoop,             0x080046A0 @ arguments: r0 = pointer to 6C whose loop to break
.set pr6C_Find,                  0x080046A8 @ arguments: r0 = pointer to ROM 6C code; returns: r0 = 6C pointer of first match (0 if none found)
.set pr6C_GotoLabel,             0x08004720 @ arguments: r0 = pointer to 6C, r1 = label index to go to
.set pr6C_GotoPointer,           0x08004758 @ arguments: r0 = pointer to 6C, r1 = pointer to ROM 6C code to go to
.set pr6C_ForEach,               0x08004794 @ arguments: r0 = pointer to ROM 6C code, r1 = function<void(6CStruct*)>
.set pr6C_BlockEachMarked,       0x080047E8 @ arguments: r0 = mark index
.set pr6C_UnblockEachMarked,     0x0800480C @ arguments: r0 = mark index
.set pr6C_DeleteEachMarked,      0x08004834 @ arguments: r0 = mark index
.set pr6C_DeleteEach,            0x0800486C @ arguments: r0 = pointer to ROM 6C code
.set pr6C_BreakEachLoop,         0x08004888 @ arguments: r0 = pointer to ROM 6C code

.set prCall_Future,              0x08014B50 @ arguments: r0 = routine to call, r1 = passed argument, r2 = time in frames to wait before call

.set prSaveData_GetSRAMLocation, 0x0800721C @ arguments: r0 = Save Slot Index (0-2 for standard save, 3-4 for suspends, 5-6 unknown); returns: SRAM Location
.set prSaveData_SaveToSRAM,      0x080BFBD8 @ arguments: r0 = Input Data Ptr, r1 = Output SRAM pointer, r2 = Size (bytes)

.set ppActiveUnit,               0x03004690

.set pBattleUnitInstiagator,     0x0203A3F0
.set pBattleUnitTarget,          0x0203A470

.set pChapterDataStruct,         0x0202BBF8
.set pActionStruct,              0x0203A85C
@ .set ppSubjectUnit,              0x02033E40 @ I don't remeber where I found this?

.set pGenericBuffer,             0x02020140 @ Used while saving among other cases

.set pCurrentMapSize,            0x0202E3D8
.set ppUnitMapRows,              0x0202E3DC
.set ppTerrainMapRows,           0x0202E3E0
.set ppMoveMapRows,              0x0202E3E4
.set ppRangeMapRows,             0x0202E3E8
.set ppFogMapRows,               0x0202E3EC
