
.macro SET_FUNC name, value
	.global \name
	.type   \name, %function
	.set    \name, \value
.endm

@ Routines

	SET_FUNC	FindProc,					(0x08002E9C+1)
	SET_FUNC	ClearProcOnCycle,			(0x08002E94+1)
	SET_FUNC	GotoProcLabel,				(0x08002F24+1)
	SET_FUNC	GetDeploymentSlot,			(0x08019430+1)
	SET_FUNC	WindowPosCheck,				(0x0808BBCC+1)
	SET_FUNC	WindowSideCheck,			(0x0808BBAC+1)
	SET_FUNC	TextAllocate,				(0x08003D84+1)
	SET_FUNC	FillTilemapRect,			(0x080D74A8+1)
	SET_FUNC	CopyTilemapRect,			(0x080D74B8+1)
	SET_FUNC	EnableBackgroundSyncByMask,	(0x08001FAC+1)
	SET_FUNC	DrawTilemap,				(0x080D74A0+1)
	SET_FUNC	TextBufferWriter,			(0x0800A240+1)
	SET_FUNC	GetStringTextCenteredPos,	(0x08003F90+1)
	SET_FUNC	TextSetParameters,			(0x08003E68+1)
	SET_FUNC	TextDraw,					(0x08003E70+1)
	SET_FUNC	TextClear,					(0x08003DC8+1)
	SET_FUNC	TextAppendString,			(0x08004004+1)
	SET_FUNC	GetPaletteByAllegiance,		(0x0808C2CC+1)
	SET_FUNC	GetPortraitIndex,			(0x080192F4+1)
	SET_FUNC	CreateMinimug,				(0x08005988+1)
	SET_FUNC	GetCurrentHP,				(0x08019150+1)
	SET_FUNC	GetMaxHP,					(0x08019190+1)
	SET_FUNC	GetStr,						(0x080191B0+1)
	SET_FUNC	GetSkl,						(0x080191D0+1)
	SET_FUNC	GetSpd,						(0x08019210+1)
	SET_FUNC	GetDef,						(0x08019250+1)
	SET_FUNC	GetRes,						(0x08019270+1)
	SET_FUNC	GetLuk,						(0x08019298+1)
	SET_FUNC	GetAffinity,				(0x080286BC+1)
	SET_FUNC	DrawStatus,					(0x0808C388+1)
	SET_FUNC	WriteNumberBuffer,			(0x0800391C+1)
	SET_FUNC	PushToSecondaryOAM,			(0x08002BB8+1)
	SET_FUNC	CopyToPaletteBuffer,		(0x08000DB8+1)
	SET_FUNC	DrawIcon,					(0x080036BC+1)
	SET_FUNC	ClearIconRegistry,			(0x08003584+1)
	SET_FUNC	RegisterIconOBJ,			(0x0800372C+1)
	SET_FUNC	RegisterTileGraphics,		(0x08002014+1)
	SET_FUNC	CanUnitUseStaff,			(0x080167A4+1)
	SET_FUNC	GetEquippedWeapon,			(0x08016B28+1)
	SET_FUNC	GetEquippedWeaponSlot,		(0x08016B58+1)
	SET_FUNC	SetupBattleStructUnitWeapon,(0x0802A400+1)
	SET_FUNC	GetROMItemStructPtr,		(0x080177B0+1)

