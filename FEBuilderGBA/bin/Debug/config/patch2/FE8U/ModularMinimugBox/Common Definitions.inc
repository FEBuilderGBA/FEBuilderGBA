
@ RAM Stuff

	.set BG0Buffer,				0x02022CA8
	.set BG1Buffer,				0x020234A8

	.set WindowBuffer,			0x02003D2C
	.set WindowBufferBG1,		0x0200422C
	.set NumberBuffer,			0x02028E44
	.set GameDataStruct,		0x0202BCB0
	.set UnitMap,				0x0202E4D8
	.set BattleBufAttacker,		0x0203A4EC

@ ROM Stuff

	.set WindowSideTable,		0x08A01828
	.set ProcTI,				0x08A018AC
	.set ProcGENS,				0x0859A548

@ Proc State

	.set NameTextStructStart,	0x2C + 0x00
	.set NameTextStructCursor,	0x2C + 0x02
	.set NameTextStructColor,	0x2C + 0x03
	.set NameTextStructWidth,	0x2C + 0x04
	.set NameTextStructFlag,	0x2C + 0x05
	.set NameTextStructBuffer,	0x2C + 0x06
	.set NameTextStructUnknown,	0x2C + 0x07
	.set AltTextStructStart,	0x2C + 0x08
	.set AltTextStructCursor,	0x2C + 0x0A
	.set AltTextStructColor,	0x2C + 0x0B
	.set AltTextStructWidth,	0x2C + 0x0C
	.set AltTextStructFlag,		0x2C + 0x0D
	.set AltTextStructBuffer,	0x2C + 0x0E
	.set AltTextStructUnknown,	0x2C + 0x0F

	.set OAMCount,				0x2C + 0x10
	.set AffinityFlag,			0x2C + 0x11
	.set AffinityX,				0x2C + 0x12
	.set AffinityY,				0x2C + 0x13
	.set AffinityTile,			0x2C + 0x14
	.set OAMCount2,				0x2C + 0x16
	.set DisplayedIndex,		0x2C + 0x17

	.set HoverFramecount,		0x2C + 0x18
	.set BattleStructFlag,		0x2C + 0x1A

	.set CursorXOld,			0x2C + 0x20
	.set CursorYOld,			0x2C + 0x21
	.set CursorX,				0x2C + 0x22
	.set CursorY,				0x2C + 0x23

	.set WindowPosType,			0x2C + 0x24

	.set UnitFlag,				0x2C + 0x29
	.set RetractFlag,			0x2C + 0x2A
	.set WindowSideType,		0x2C + 0x2B
	.set Framecount,			0x2C + 0x2C










