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

.macro _MakeSign rd, rs, rox=r3
	neg \rox, \rs
    asr \rox, \rox, #31
    asr \rd,  \rs,  #31
    sub \rd,  \rox
.endm

.set prForEachAdjacentUnit,      0x08024F20 @ arguments: r0 = x, r1 = y, r2 = function<void(UnitStruct*)>; returns: nothing
.set prAddTargetListEntry,       0x08050630 @ arguments: r0 = x, r1 = y, r2 = unit allegience byte, r3 = trap type; returns: nothing
.set prGetTargetListSize,        0x08050A9C @ arguments: nothing; returns: r0 = current target list size

.set prScheduleRoutineCall,      0x0801490C @ arguments: r0 = routine to call, r1 = argument, r2 = time (in frames) before call happens

.set prGetFacingDirectionId,     0x0807DD00 @ arguments: r0 = xSource, r1 = ySource, r2 = xTarget, r3 = yTarget

.set prIDunnoReallyButIThinkItUpdatesStandingSprites, 0x08027144

.set prMap_Fill,                 0x080194BC @ arguments: r0 = rows start ptr, r1 = value; returns: nothing

.set prItem_GetIndex,            0x08017294 @ arguments: r0 = item short; returns: r0 = item index (= (item & 0xFF))

.set prUnit_GetStruct,           0x08019108 @ arguments: r0 = Unit Allegience Index; returns: r0 = Unit Struct pointer (0 if not found)
.set prUnit_ApplyMovement,       0x080181B0 @ arguments: r0 = Unit Struct pointer
.set prUnit_CanCrossTerrain,     0x08019174 @ arguments: r0 = Unit Struct pointer, r1 = Terrain Index; returns: r0 = 0 if Unit cannot cross/stand on terrain

.set prTargetSelection_New,      0x080507B0 @ arguments: r0 = pointer to Target Selection Definition

.set prMOVEUNIT_NewForMapUnit,   0x0807A888 @ arguments: r0 = pointer to Unit Struct; returns: r0 = new MOVEUNIT pointer
.set prMOVEUNIT_SetMovement,     0x0807ABB4 @ arguments: r0 = pointer to MOVEUNIT, r1 = pointer to movement buffer
.set prMOVEUNIT_SetSprDirection, 0x0807AAB8 @ arguments: r0 = pointer to MOVEUNIT, r1 = direction id (use prGetFacingDirectionId, or 0xB for idle)
.set prMOVEUNIT_DeleteAll,       0x0807B4B8 @ none

.set pr6C_New,                   0x08002BCC @ arguments: r0 = pointer to ROM 6C code, r1 = parent; returns: r0 = new 6C pointer (0 if no space available)
.set pr6C_NewBlocking,           0x08002C30 @ same
.set pr6C_Delete,                0x08002CBC @ arguments: r0 = pointer to 6C to delete
.set pr6C_BreakLoop,             0x08002DE4 @ arguments: r0 = pointer to 6C whose loop to break
.set pr6C_Find,                  0x08002DEC @ arguments: r0 = pointer to ROM 6C code; returns: r0 = 6C pointer of first match (0 if none found)
.set pr6C_GotoLabel,             0x08002E74 @ arguments: r0 = pointer to 6C, r1 = label index to go to
.set pr6C_GotoPointer,           0x08002EAC @ arguments: r0 = pointer to 6C, r1 = pointer to ROM 6C code to go to
.set pr6C_ForEach,               0x08002EE8 @ arguments: r0 = pointer to ROM 6C code, r1 = function<void(6CStruct*)>
.set pr6C_BlockEachMarked,       0x08002F3C @ arguments: r0 = mark index
.set pr6C_UnblockEachMarked,     0x08002F64 @ arguments: r0 = mark index
.set pr6C_DeleteEachMarked,      0x08002F90 @ arguments: r0 = mark index
.set pr6C_DeleteEach,            0x08002FC8 @ arguments: r0 = pointer to ROM 6C code
.set pr6C_BreakEachLoop,         0x08002FE4 @ arguments: r0 = pointer to ROM 6C code

.set ppActiveUnit,               0x03004DF0
.set pActionStruct,              0x0203A954
@ .set ppSubjectUnit,              0x02033F38

.set pCurrentMapSize,            0x0202E4D0
.set ppUnitMapRows,              0x0202E4D4
.set ppTerrainMapRows,           0x0202E4D8
.set ppRangeMapRows,             0x0202E4E0
