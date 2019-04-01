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

.set prForEachAdjacentUnit,      0x08024F70 @ arguments: r0 = x, r1 = y, r2 = function<void(UnitStruct*)>; returns: nothing
.set prAddTargetListEntry,       0x0804F8BC @ arguments: r0 = x, r1 = y, r2 = unit allegience byte, r3 = trap type; returns: nothing
.set prGetTargetListSize,        0x0804FD28 @ arguments: nothing; returns: r0 = current target list size

.set prScheduleRoutineCall,      0x080148E4 @ arguments: r0 = routine to call, r1 = argument, r2 = time (in frames) before call happens

.set prGetFacingDirectionId,     0x0807B9B8 @ arguments: r0 = xSource, r1 = ySource, r2 = xTarget, r3 = yTarget
.set prChangeActiveUnitFacing,   0x0801F50C @ arguments: r0 = xTarget, r1 = yTarget

.set prIDunnoReallyButIThinkItUpdatesStandingSprites, 0x080271A0

.set prMap_Fill,                 0x080197E4 @ arguments: r0 = rows start ptr, r1 = value; returns: nothing

.set prItem_GetIndex,            0x080174EC @ arguments: r0 = item short; returns: r0 = item index (= (item & 0xFF))

.set prUnit_GetStruct,           0x08019430 @ arguments: r0 = Unit Allegience Index; returns: r0 = Unit Struct pointer (0 if not found)
.set prUnit_ApplyMovement,       0x0801849C @ arguments: r0 = Unit Struct pointer
.set prUnit_CanCrossTerrain,     0x0801949C @ arguments: r0 = Unit Struct pointer, r1 = Terrain Index; returns: r0 = 0 if Unit cannot cross/stand on terrain
.set prUnit_GetEquippedWeapon,   0x08016B28 @ arguments: r0 = Unit Struct pointer; returns: r0 = Item Short
.set prUnit_GetItemCount,        0x080179D8 @ arguments: r0 = Unit Struct pointer; returns: r0 = Item Count
.set prUnit_CanUseAsStaff,       0x080167A4 @ arguments: r0 = Unit Struct pointer, r1 = Item Short; returns: r0 = 0 if cannot use

.set prUnit_GetDefense,          0x08019250 @ arguments: r0 = Unit Struct pointer; returns: r0 = Unit Computed Defense

.set prItem_GetMight,            0x080175DC @ arguments: r0 = Item Short; returns: r0 = Might
.set prItem_GetAttributes,       0x0801756C @ arguments: r0 = Item Short; returns: r0 = Attribute Word
.set prItem_GetWType,            0x08017548 @ arguments: r0 = Item Short; returns: r0 = WType
.set prItem_GetWRank,            0x080176B8 @ arguments: r0 = Item Short; returns: r0 = WRank
.set prItem_GetUseEffect,        0x0801773C @ arguments: r0 = Item Short; returns: r0 = Use Effect

.set prGetTextInBuffer,          0x0800A240 @ arguments: r0 = text id; returns: r0 = buffer in which the text now is

.set prBottomHelpDisplay_New,    0x08035708 @ arguments: r0 = parent 6C, r1 = pointer to text IN BUFFER

.set prTargetSelection_New,      0x0804FA3C @ arguments: r0 = pointer to Target Selection Definition

.set prMoveRange_ShowGfx,        0x0801DA98 @ arguments: r0 = type bitfield (&1 = Move Blue Squares, &2 = Range Red Squares, &4 = Range Green Squares, &16 = Range Blue Squares)
.set prMoveRange_HideGfx,        0x0801DACC @ none

.set prMOVEUNIT_NewForMapUnit,   0x08078464 @ arguments: r0 = pointer to Unit Struct; returns: r0 = new MOVEUNIT pointer
.set prMOVEUNIT_SetMovement,     0x08078790 @ arguments: r0 = pointer to MOVEUNIT, r1 = pointer to movement buffer
.set prMOVEUNIT_SetSprDirection, 0x08078694 @ arguments: r0 = pointer to MOVEUNIT, r1 = direction id (use prGetFacingDirectionId, or 0xB for idle)
.set prMOVEUNIT_DeleteAll,       0x080790A4 @ none

.set pr6C_New,                   0x08002C7C @ arguments: r0 = pointer to ROM 6C code, r1 = parent; returns: r0 = new 6C pointer (0 if no space available)
.set pr6C_NewBlocking,           0x08002CE0 @ same
.set pr6C_Delete,                0x08002D6C @ arguments: r0 = pointer to 6C to delete
.set pr6C_BreakLoop,             0x08002E94 @ arguments: r0 = pointer to 6C whose loop to break
.set pr6C_Find,                  0x08002E9C @ arguments: r0 = pointer to ROM 6C code; returns: r0 = 6C pointer of first match (0 if none found)
.set pr6C_GotoLabel,             0x08002F24 @ arguments: r0 = pointer to 6C, r1 = label index to go to
.set pr6C_GotoPointer,           0x08002F5C @ arguments: r0 = pointer to 6C, r1 = pointer to ROM 6C code to go to
.set pr6C_ForEach,               0x08002F98 @ arguments: r0 = pointer to ROM 6C code, r1 = function<void(6CStruct*)>
.set pr6C_BlockEachMarked,       0x08002FEC @ arguments: r0 = mark index
.set pr6C_UnblockEachMarked,     0x08003014 @ arguments: r0 = mark index
.set pr6C_DeleteEachMarked,      0x08003040 @ arguments: r0 = mark index
.set pr6C_DeleteEach,            0x08003078 @ arguments: r0 = pointer to ROM 6C code
.set pr6C_BreakEachLoop,         0x08003094 @ arguments: r0 = pointer to ROM 6C code

.set prCall_Future,              0x080148E4 @ arguments: r0 = routine to call, r1 = passed argument, r2 = time in frames to wait before call

.set prSaveData_GetSRAMLocation, 0x080A3064 @ arguments: r0 = Save Slot Index (0-2 for standard save, 3-4 for suspends, 5-6 unknown); returns: SRAM Location
.set prSaveData_SaveToSRAM,      0x080D184C @ arguments: r0 = Input Data Ptr, r1 = Output SRAM pointer, r2 = Size (bytes)

.set ppActiveUnit,               0x03004E50

.set pBattleUnitInstiagator,     0x0203A4EC
.set pBattleUnitTarget,          0x0203A56C

.set pChapterDataStruct,         0x0202BCF0
.set pActionStruct,              0x0203A958
@ .set ppSubjectUnit,              0x02033F3C @ I don't remeber where I found this?

.set pGenericBuffer,             0x02020188 @ Used while saving among other cases

.set pCurrentMapSize,            0x0202E4D4
.set ppUnitMapRows,              0x0202E4D8
.set ppTerrainMapRows,           0x0202E4DC
.set ppMoveMapRows,              0x0202E4E0
.set ppRangeMapRows,             0x0202E4E4
.set ppFogMapRows,               0x0202E4E8
