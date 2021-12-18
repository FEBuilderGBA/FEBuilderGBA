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

@for FE8J
@.set MapAddInRange, 0x0801a798		@{J}
@.set gMapSize, 0x0202E4D0			@{J}
@.set gMapMovement , 0x0202E4DC		@{J}
@.set gMapUnit, 0x0202E4D4			@{J}
@.set gMapMovement2, 0x0202E4EC		@{J}
@.set GetBallistaItemAt, 0x08037A24	@{J}
@.set GetWeaponRangeMask, 0x08016E7C	@{J}
@.set gSubjectMap, 0x03004950		@{J}
@.set GetUnitRangeMask, 0x08016F90	@{J}
@.set GetItemMaxRange, 0x0801742c	@{J}
@.set GetItemMinRange, 0x08017414	@{J}
@.set CanUnitUseItem, 0x0802881c		@{J}
@.set GetUnitMagBy2Range, 0x08018730	@{J}

@for FE8U
.set MapAddInRange, 0x0801aabc		@{U}
.set gMapSize, 0x0202E4D4			@{U}
.set gMapMovement , 0x0202E4E0		@{U}
.set gMapUnit, 0x0202E4D8			@{U}
.set gMapMovement2, 0x0202E4F0		@{U}
.set GetBallistaItemAt, 0x0803798C	@{U}
.set GetWeaponRangeMask, 0x080170d4	@{U}
.set gSubjectMap, 0x030049B0		@{U}
.set GetUnitRangeMask, 0x080171e8	@{U}
.set GetItemMaxRange, 0x08017684	@{U}
.set GetItemMinRange, 0x0801766C	@{U}
.set CanUnitUseItem, 0x08028870		@{U}
.set GetUnitMagBy2Range, 0x08018a1c	@{U}

@for FE7J
@.set MapAddInRange, 0x0801a6b4		@{J}
@.set gMapSize, 0x0202E3D4			@{J}
@.set gMapMovement , 0x0202E3E0		@{J}
@.set gMapUnit, 0x0202E3D8			@{J}
@.set gMapMovement2, 0x0202E3F0		@{J}
@.set GetBallistaItemAt, 0x08034ba0	@{J}
@.set GetWeaponRangeMask, 0x08017208	@{J}
@.set gSubjectMap, 0x03004110		@{J}
@.set GetUnitRangeMask, 0x08017310	@{J}
@.set GetItemMaxRange, 0x0801778c	@{J}
@.set GetItemMinRange, 0x08017774	@{J}
@.set CanUnitUseItem, 0x08027158		@{J}
@.set GetUnitMagBy2Range, 0x080188A4	@{J}

@for FE7U
@.set MapAddInRange, 0x0801a2d4		@{U}
@.set gMapSize, 0x0202E3D8			@{U}
@.set gMapMovement , 0x0202E3E4		@{U}
@.set gMapUnit, 0x0202E3DC			@{U}
@.set gMapMovement2, 0x0202E3F4		@{U}
@.set GetBallistaItemAt, 0x080346c8	@{U}
@.set GetWeaponRangeMask, 0x08016DB4	@{U}
@.set gSubjectMap, 0x030041F0		@{U}
@.set GetUnitRangeMask, 0x08016ebc	@{U}
@.set GetItemMaxRange, 0x08017384	@{U}
@.set GetItemMinRange, 0x0801736c	@{U}
@.set CanUnitUseItem, 0x08026cd0		@{U}
@.set GetUnitMagBy2Range, 0x080184B4	@{U}

