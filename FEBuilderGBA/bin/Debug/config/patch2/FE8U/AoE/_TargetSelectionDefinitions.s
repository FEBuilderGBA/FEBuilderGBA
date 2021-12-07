.thumb
@Staff AI asm macros

.macro _blh to, reg=r3
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm

.macro _bldr reg, dest
	ldr \reg, =\dest
	mov lr, \reg
	.short 0xF800
.endm

.macro _blr reg
	mov lr, \reg
	.short 0xF800
.endm


@----------------------------------------------------------
@Relevant Ram Offsets
	.set ChapterDataStruct,            0x0202BCF0 		@{U}
@	.set ChapterDataStruct,            0x0202BCEC 		@{J}
	.set CurrentMapSize,               0x0202E4D4 		@{U}
@	.set CurrentMapSize,               0x0202E4D0 		@{J}
	.set UnitMapRows,                  0x0202E4D8 		@{U}
@	.set UnitMapRows,                  0x0202E4D4 		@{J}
	.set MoveCostMapRows,              0x0202E4E0 		@{U}
@	.set MoveCostMapRows,              0x0202E4DC 		@{J}
	.set RangeMapRows,                 0x0202E4E4 		@{U}
@	.set RangeMapRows,                 0x0202E4E0 		@{J}
	.set FogMapRows,                   0x0202E4E8		@{U}
@	.set FogMapRows,                   0x0202E4E4		@{J}
	.set ActionStruct,                 0x0203A958 		@{U}
@	.set ActionStruct,                 0x0203A954 		@{J}
	.set TargeterXY,                   0x0203DDE8 		@{U}
@	.set TargeterXY,                   0x0203DDE4 		@{J}
	.set TargetList,                   0x0203DDEC		@{U}
@	.set TargetList,                   0x0203DDE8		@{J}
	.set TargetNum,                    0x0203E0EC 		@{U}
@	.set TargetNum,                    0x0203E0E8 		@{J}
	.set SelectedUnit,                 0x02033F3C 		@{U}
@	.set SelectedUnit,                 0x02033F38 		@{J}
	.set ActiveUnit,                   0x03004E50 		@{U}
@	.set ActiveUnit,                   0x03004DF0 		@{J}

@----------------------------------------------------------
@List of Relevant Routines

	@Item & Unit Related routines
	.set DecrementItemUses,            0x08016AEC	@{U}
@	.set DecrementItemUses,            0x08016894	@{J}
		@arguments: r0= item/uses short

	.set Unit_GetEquippedWeapon,       0x08016B28	@{U}
@	.set Unit_GetEquippedWeapon,       0x080168D0	@{J}
		@ arguments: r0 = Unit Struct pointer;
		@ returns: r0 = Item Short
	.set Item_GetUsesLeft,             0x08017584	@{U}
@	.set Item_GetUsesLeft,             0x0801732C	@{J}
		@arguments: r0 = item/uses short
	.set Unit_ReorderItems,            0x08017984	@{U}
@	.set Unit_ReorderItems,            0x0801772C	@{J}
		@arguments: r0 = ram unit pointer
		@remove spaces in unit's inventory caused 
		@by things like stolen and broken items
	.set Unit_GetItemCount,        0x080179D8 @ arguments: r0 = Unit Struct pointer; returns: r0 = Item Count	@{U}
@	.set Unit_GetItemCount,        0x08017780 @ arguments: r0 = Unit Struct pointer; returns: r0 = Item Count	@{J}
		@arguments: r0= ram unit pointer
	.set GetUnit,                  0x08019430 @ arguments: r0 = Unit Allegience Index; returns: r0 = Unit Struct pointer (0 if not found)	@{U}
@	.set GetUnit,                  0x08019108 @ arguments: r0 = Unit Allegience Index; returns: r0 = Unit Struct pointer (0 if not found)	@{J}

	.set Unit_GetAid,                  0x080189B8	@{U}
@	.set Unit_GetAid,                  0x080186CC	@{J}
	.set Unit_GetHalfMag,              0x08018A1C	@{U}
@	.set Unit_GetHalfMag,              0x08018730	@{J}
	.set Unit_GetCurHP,                0x08019150	@{U}
@	.set Unit_GetCurHP,                0x08018E64	@{J}
	.set Unit_GetMaxHP,                0x08019190	@{U}
@	.set Unit_GetMaxHP,                0x08018EA4	@{J}
	.set Unit_GetStr,                  0x080191B0	@{U}
@	.set Unit_GetStr,                  0x08018EC4	@{J}
	.set Unit_GetMag,                  0x080191B0	@{U}
@	.set Unit_GetMag,                  0x08018EC4	@{J}
	.set Unit_GetSkl,                  0x080191D0	@{U}
@	.set Unit_GetSkl,                  0x08018EE4	@{J}
	.set Unit_GetSpd,                  0x08019210	@{U}
@	.set Unit_GetSpd,                  0x08018F24	@{J}
	.set Unit_GetDef,                  0x08019250	@{U}
@	.set Unit_GetDef,                  0x08018F64	@{J}
	.set Unit_GetRes,                  0x08019270	@{U}
@	.set Unit_GetRes,                  0x08018F84	@{J}
	.set Unit_GetLuck,                 0x08019298	@{U}
@	.set Unit_GetLuck,                 0x08018FAC	@{J}
	.set Unit_CanCrossTerrain,         0x0801949C	@{U}
@	.set Unit_CanCrossTerrain,         0x08019174	@{J}
		@ arguments: r0 = Unit Struct pointer, r1 = Terrain Index;
		@ returns: r0 = 0 if Unit cannot cross/stand on terrain
	.set Unit_GetRangeMap,             0x080171E8	@{U}
@	.set Unit_GetRangeMap,             0x08016F90	@{J}
		@ arguments: r0 = Unit Struct pointer, r1 = Item Slot Index (-1 for all);
		@ returns: r0 = range mask
	.set Unit_CanUseItem,              0x08028870	@{U}
@	.set Unit_CanUseItem,              0x0802881C	@{J}
		@ arguments: r0 = Unit Struct pointer, r1 = Item Short;
		@ returns = 1 if unit can use item, 0 otherwise
	.set StaffHitRate,                 0x0802CCDC 	@{U}
@	.set StaffHitRate,                 0x0802CC14 	@{J}

	@Range and Move Cost Maps Routines
	.set FillMap,                      0x080197E4	@{U}
@	.set FillMap,                      0x080194BC	@{J}
		@r0 = row pointer; r1 = value
	.set AddRange,                     0x0801AABC	@{U}
@	.set AddRange,                     0x0801A798	@{J}
		@build targeting range in range map
		@r0 = x; r1 = y; r2 = range; r3 = value
	.set CheckUnitsInRange,            0x08024EAC	@{U}
@	.set CheckUnitsInRange,            0x08024E5C	@{J}
	.set CheckTilesInRange,            0x08024F18	@{U}
@	.set CheckTilesInRange,            0x08024EC8	@{J}
	.set CheckAdjacentUnits,           0x08024F70	@{U}
@	.set CheckAdjacentUnits,           0x08024F20	@{J}
	.set ShowRangeSquares,             0x0801DA98	@{U}
@	.set ShowRangeSquares,             0x0801D6FC	@{J}
	.set HideRangeSquares,             0x0801DACC	@{U}
@	.set HideRangeSquares,             0x0801D730	@{J}
		@arguments: none; returns: nothing
	
	@Target List Related Routines
	.set RefreshTargetList,            0x0804F8A4	@{U}
@	.set RefreshTargetList,            0x08050618	@{J}
		@r0 = x; r1 = y;
	.set AddTargetListEntry,           0x0804F8BC	@{U}
@	.set AddTargetListEntry,           0x08050630	@{J}
		@arguments: r0 = x, r1 = y, 
		@r2 = unit allegience byte, r3 = trap type; 
		@returns: nothing
	.set GetTargetListSize,            0x0804FD28	@{U}
@	.set GetTargetListSize,            0x08050A9C	@{J}
	.set GetTargetListEntry,           0x0804FD34	@{U}
@	.set GetTargetListEntry,           0x08050AA8	@{J}
	@6c stuff; most of these are taken from stan's notes
	.set NewTargetSelection,           0x0804FA3C	@{U}
@	.set NewTargetSelection,           0x080507B0	@{J}
	.set NewTargetSelectv2,            0x0804FAA4	@{U}
@	.set NewTargetSelectv2,            0x08050818	@{J}

	.set New6C,                        0x08002C7C @ arguments: r0 = pointer to ROM 6C code, r1 = parent; returns: r0 = new 6C pointer (0 if no space available)	@{U}
@	.set New6C,                        0x08002BCC @ arguments: r0 = pointer to ROM 6C code, r1 = parent; returns: r0 = new 6C pointer (0 if no space available)	@{J}

	.set New6CBlocking,                0x08002CE0 @ same	@{U}
@	.set New6CBlocking,                0x08002C30 @ same	@{J}
	.set End6C,                        0x08002D6C	@{U}
@	.set End6C,                        0x08002CBC	@{J}
		@ arguments: r0 = pointer to 6C to delete

	.set Break6CLoop,              0x08002E94	@{U}
@	.set Break6CLoop,              0x08002DE4	@{J}
		@ arguments: r0 = pointer to 6C whose loop to break
	.set Find6C,                       0x08002E9C	@{U}
@	.set Find6C,                       0x08002DEC	@{J}
		@ arguments: r0 = pointer to ROM 6C code; returns: r0 = 6C pointer of first match (0 if none found)
	.set Goto6CLabel,                  0x08002F24	@{U}
@	.set Goto6CLabel,                  0x08002E74	@{J}
		@ arguments: r0 = pointer to 6C, r1 = label index to go to
	.set Goto6CPointer,                0x08002F5C	@{U}
@	.set Goto6CPointer,                0x08002EAC	@{J}
		@ arguments: r0 = pointer to 6C, r1 = pointer to ROM 6C code to go to
	.set ForEach6C,                    0x08002F98	@{U}
@	.set ForEach6C,                    0x08002EE8	@{J}
		@ arguments: r0 = pointer to ROM 6C code, r1 = function<void(6CStruct*)>
	.set BlockEach6CMarked,            0x08002FEC	@{U}
@	.set BlockEach6CMarked,            0x08002F3C	@{J}
		@ arguments: r0 = mark index
	.set UnblockEach6CMarked,          0x08003014	@{U}
@	.set UnblockEach6CMarked,          0x08002F64	@{J}
		@ arguments: r0 = mark index
	.set DeleteEach6CMarked,           0x08003040	@{U}
@	.set DeleteEach6CMarked,           0x08002F90	@{J}
		@ arguments: r0 = mark index
	.set DeleteEach6C,                 0x08003078	@{U}
@	.set DeleteEach6C,                 0x08002FC8	@{J}
		@ arguments: r0 = pointer to ROM 6C code
	.set BreakEach6CLoop,              0x08003094	@{U}
@	.set BreakEach6CLoop,              0x08002FE4	@{J}
		@ arguments: r0 = pointer to ROM 6C code

	.set LockGameLogic,            0x08015360	@{U}
@	.set LockGameLogic,            0x08015384	@{J}
	.set UnlockGameLogic,          0x08015370	@{U}
@	.set UnlockGameLogic,          0x08015394	@{J}

	.set GetTextBuffer,                0x0800A240	@{U}
@	.set GetTextBuffer,                0x08009FA8	@{J}
	.set SetBottomHelpText,            0x08035708	@{U}
@	.set SetBottomHelpText,            0x08035610	@{J}
	
	@Trap Related Routines
	.set FindTrapAt,                   0x0802E1F0	@{U}
@	.set FindTrapAt,                   0x0802E128	@{J}
	.set FindTrapTypeAt,               0x0802E24C	@{U}
@	.set FindTrapTypeAt,               0x0802E184	@{J}
	.set CreateTrap,                   0x0802E2B8	@{U}
@	.set CreateTrap,                   0x0802E1F0	@{J}
	.set CreateLightRune,              0x0802EA58	@{U}
@	.set CreateLightRune,              0x0802E990	@{J}
	.set CreateBallista,               0x08037A04	@{U}
@	.set CreateBallista,               0x08037A9C	@{J}
	.set FindBallistaAt,               0x0803798C	@{U}
@	.set FindBallistaAt,               0x08037A24	@{J}
		@ arguments: r0 = x, r1 = y;
		@ returns: ballista item at (x, y) (0 if none)
	
	@Other
	.set Font_ResetAllocation,     0x08003D20	@{U}
@	.set Font_ResetAllocation,     0x08003C50	@{J}
		@frees space used by text and range squares?
		@arguments: none; returns: nothing

	.set PlaySoundEffect,              0x080D01FC	@{U}
@	.set PlaySoundEffect,              0x080D4EF4	@{J}
		@arguments: r0= sound id

	.set ConfirmStaffUse,              0x0802951C	@{U}
@	.set ConfirmStaffUse,              0x080294C4	@{J}
		@writes action 0x3 (using a staff) to ActionStruct
		@also removes range squares and clears BG2
		@arguments: none
	.set CanChestOpen,                 0x080831AC	@{U}
@	.set CanChestOpen,                 0x080854E4	@{J}
		@check if chest can be opened
		@arguments: r0 = x, r1 = y
		@returns true(1) or false(0)
	.set FadingChestOpen,              0x080831C8	@{U}
@	.set FadingChestOpen,              0x08085500	@{J}
		@if the tile at the given area is an openable chest,
		@ perform fading tile change
		@arguments: r0 = x, r1 = y
	.set CanDoorOpen,                  0x080831F0	@{U}
@	.set CanDoorOpen,                  0x08085528	@{J}
		@check if door can be opened
		@arguments: r0 = x, r1 = y
		@returns true(1) or false(0)
	.set FadingDoorOpen,               0x0808320C	@{U}
@	.set FadingDoorOpen,               0x08085544	@{J}
		@if the tile at the given area is an openable door,
		@ perform fading tile change
		@arguments: r0 = x, r1 = y


@ I call "pairs" 32 bit values that hold two 16 bit parts, suitable for being stored in only one register

@ (rd != rox) MUST be true
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

@ unsigned variant

.macro _MakeUPair rd, rs1, rs2
	lsl \rd, \rs2, #16
	orr \rd, \rs1
.endm

.macro _GetUPairFirst rd, rs
	lsl \rd, \rs, #16 @ clearing second part of pair
	lsr \rd, \rd, #16 @ shifting back
.endm

.macro _GetUPairSecond rd, rs
	lsr \rd, \rs, #16 @ shifting second part of pair (erasing first part in the process)
.endm

.set ppRangeMapRows,           0x0202E4E4	@{U}
@.set ppRangeMapRows,           0x0202E4E0	@{J}

.set Map_Fill,                 0x080197E4 @ arguments: r0 = rows start ptr, r1 = value; returns: nothing	@{U}
@.set Map_Fill,                 0x080194BC @ arguments: r0 = rows start ptr, r1 = value; returns: nothing	@{J}

.set MoveRange_HideGfx,        0x0801DACC @ none	@{U}
@.set MoveRange_HideGfx,        0x0801D730 @ none	@{J}

.set BottomHelpDisplay_New,    0x08035708 @ arguments: r0 = parent 6C, r1 = pointer to text IN BUFFER	@{U}
@.set BottomHelpDisplay_New,    0x08035610 @ arguments: r0 = parent 6C, r1 = pointer to text IN BUFFER	@{J}

.set BottomHelpDisplay_EndAll, 0x08035748 @ none	@{U}
@.set BottomHelpDisplay_EndAll, 0x08035848 @ none	@{J}

.set pActionStruct,            0x0203A958	@{U}
@.set pActionStruct,            0x0203A954	@{J}

.set pBG0TileMap,              0x02022CA8	@{U}
@.set pBG0TileMap,              0x02022CA8	@{J}

.set ppMoveMapRows,            0x0202E4E0	@{U}
@.set ppMoveMapRows,            0x0202E4DC	@{J}

.set TargetSelection_New,      0x0804FA3C @ arguments: r0 = pointer to Target Selection Definition	@{U}
@.set TargetSelection_New,      0x080507B0 @ arguments: r0 = pointer to Target Selection Definition	@{J}

.set p6C_GBToUnitMenu,         0x0859B600	@{U}
@.set p6C_GBToUnitMenu,         0x085C3AE0	@{J}

.set ppActiveUnit,             0x03004E50 @ Active Unit	@{U}
@.set ppActiveUnit,             0x03004DF0 @ Active Unit	@{J}

.set pGameDataStruct,          0x0202BCB0	@{U}
@.set pGameDataStruct,          0x0202BCAC	@{J}

.set HandlePPCursorMovement,   0x0801C8AC @ none?	@{U}
@.set HandlePPCursorMovement,   0x0801C514 @ none?	@{J}

.set pKeyStatusBuffer,         0x02024CC0	@{U}	@{J}

.set TCS_New,                  0x0800927C @ arguments: r0 = ROM source, r1 = OAM Index?	@{U}
@.set TCS_New,                  0x0800916C @ arguments: r0 = ROM source, r1 = OAM Index?	@{J}

.set TCS_SetAnim,              0x08009518 @ arguments: r0 = TCS, r1 = Index	@{U}
@.set TCS_SetAnim,              0x08009408 @ arguments: r0 = TCS, r1 = Index	@{J}

.set TCS_Update,               0x080092BC @ arguments: r0 = TCS, r1 = Display X, r2 = Display Y	@{U}
@.set TCS_Update,               0x080091AC @ arguments: r0 = TCS, r1 = Display X, r2 = Display Y	@{J}

.set TCS_Free,                 0x080092A4 @ arguments: r0 = TCS	@{U}
@.set TCS_Free,                 0x08009194 @ arguments: r0 = TCS	@{J}

.set pChapterDataStruct,       0x0202BCF0	@{U}
@.set pChapterDataStruct,        0x0202BCEC	@{J}

.set EventEngine, 0x800D07C	@{U}
@.set EventEngine, 0x800D340	@{J}

.set MemorySlot,0x30004B8	@{U}
@.set MemorySlot,0x30004B0	@{J}
