
.macro bllr
	.short 0xF800
.endm

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
	.set SpriteData8x8,			0x08590F44
	.set SpriteData16x16,		0x08590F4C
	.set ProcGENS,				0x0859A548
	.set ItemIconPalette,		0x085996F4
	.set AffinityWRankPalette,	0x08599714

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

	.set InventoryIconCount,	0x2C + 0x10
	.set AffinityFlag,			0x2C + 0x11
	.set AffinityX,				0x2C + 0x12
	.set AffinityY,				0x2C + 0x13
	.set AffinityTile,			0x2C + 0x14
	.set SkillIconCount,		0x2C + 0x16
	.set DisplayedSkillIndex,	0x2C + 0x17

	.set HoverFramecount,		0x2C + 0x18
	.set BattleStructFlag,		0x2C + 0x1A

	.set EquippedWeaponIndex,	0x2C + 0x1B
	.set DisplayedItemIndex,	0x2C + 0x1C
	.set WeaponSkillFlag,		0x2C + 0x1D

	.set CursorXOld,			0x2C + 0x20
	.set CursorYOld,			0x2C + 0x21
	.set CursorX,				0x2C + 0x22
	.set CursorY,				0x2C + 0x23

	.set WindowPosType,			0x2C + 0x24

	.set UnitFlag,				0x2C + 0x29
	.set RetractFlag,			0x2C + 0x2A
	.set WindowSideType,		0x2C + 0x2B
	.set Framecount,			0x2C + 0x2C

@ Unit data

	.set UnitCharacterData,		0x00
	.set UnitClassData,			0x04
	.set UnitLevel,				0x08
	.set UnitExperience,		0x09
	.set UnitAIFlags,			0x0A
	.set UnitDeploymentNumber,	0x0B @ fireemblem8u calls this `index`
	.set UnitState,				0x0C
	.set UnitXCoordinate,		0x10
	.set UnitYCoordinate,		0x11
	.set UnitMaxHP,				0x12
	.set UnitCurrentHP,			0x13
	.set UnitPower,				0x14 @ Strength/Magic
	.set UnitSkill,				0x15
	.set UnitSpeed,				0x16
	.set UnitDefense,			0x17
	.set UnitResistance,		0x18
	.set UnitLuck,				0x19
	.set UnitConstitutionBonus,	0x1A
	.set UnitRescue,			0x1B
	.set UnitBallistaIndex,		0x1C
	.set UnitMovementBonus,		0x1D
	.set UnitInventory,			0x1E
	.set UnitWeaponRanks,		0x28
	.set UnitStatus,			0x30
	.set UnitTorchBarrier,		0x31 @ I don't have a good name for both
	.set UnitSupports,			0x32
	.set UnitSupportBits,		0x39
	.set UnitUnknown3A,			0x3A
	.set UnitUnknown3B,			0x3B
	.set UnitMapSpriteHandle,	0x3C
	.set UnitAI3,				0x40
	.set UnitAI4,				0x41
	.set UnitAI1,				0x42
	.set UnitAI1Data,			0x43
	.set UnitAI2,				0x44
	.set UnitAI2Data,			0x45
	.set UnitUnknown46,			0x46
	.set UnitUnknown47,			0x47

@ Battle unit

	@ First 0x48 bytes are unit data
	.set BattleUnitWeapon,				0x48
	.set BattleUnitWeaponBefore,		0x4A
	.set BattleUnitWeaponAttributes,	0x4C
	.set BattleUnitWeaponType,			0x50
	.set BattleUnitWeaponSlotIndex,		0x51
	.set BattleUnitCanCounter,			0x52
	.set BattleUnitTriangleHitBonus,	0x53
	.set BattleUnitTriangleDamageBonus,	0x54
	.set BattleUnitTerrainID,			0x55
	.set BattleUnitTerrainDefense,		0x56
	.set BattleUnitTerrainAvoid,		0x57
	.set BattleUnitTerrainResistance,	0x58
	.set BattleUnitPad59,				0x59
	.set BattleUnitAttack,				0x5A
	.set BattleUnitDefense,				0x5C
	.set BattleUnitAttackSpeed,			0x5E
	.set BattleUnitHit,					0x60
	.set BattleUnitAvoid,				0x62
	.set BattleUnitEffectiveHit,		0x64
	.set BattleUnitCrit,				0x66
	.set BattleUnitDodge,				0x68
	.set BattleUnitEffectiveCrit,		0x6A
	.set BattleUnitLethality,			0x6C @ fireemblem8u calls this `SilencerRate`
	.set BattleUnitExperienceGain,		0x6E
	.set BattleUnitStatusOut,			0x6F
	.set BattleUnitLevelBefore,			0x70
	.set BattleUnitExperienceBefore,	0x71
	.set BattleUnitHPBefore,			0x72
	.set BattleUnitHPChange,			0x73
	.set BattleUnitPowerChange,			0x74
	.set BattleUnitSkillChange,			0x75
	.set BattleUnitSpeedChange,			0x76
	.set BattleUnitDefenseChange,		0x77
	.set BattleUnitResistanceChange,	0x78
	.set BattleUnitLuckChange,			0x79
	.set BattleUnitConstitutionChange,	0x7A
	.set BattleUnitWEXPMultiplier,		0x7B
	.set BattleUnitCanDealDamage,		0x7C @ fireemblem8u calls this `nonZeroDamage`
	.set BattleUnitWeaponBroke,			0x7D
	.set BattleUnitHasItemEffectTarget,	0x7E
	.set BattleUnitPad7F,				0x7F

@ Item data

	.set ItemDataNameID,				0x00
	.set ItemDataDescriptionID,			0x02
	.set ItemDataUseDescriptionID,		0x04
	.set ItemDataItemID,				0x06
	.set ItemDataType,					0x07
	.set ItemDataAttributes,			0x08
	.set ItemDataStatBonusesPointer,	0x0C
	.set ItemDataEffectivenessPointer,	0x10
	.set ItemDataUses,					0x14
	.set ItemDataMight,					0x15
	.set ItemDataHit,					0x16
	.set ItemDataWeight,				0x17
	.set ItemDataCrit,					0x18
	.set ItemDataRange,					0x19
	.set ItemDataCostPerUse,			0x1A
	.set ItemDataWeaponRank,			0x1C
	.set ItemDataIconID,				0x1D
	.set ItemDataUseEffectID,			0x1E
	.set ItemDataWeaponEffectID,		0x1F
	.set ItemDataWeaponEXP,				0x20

@ These are set by the skill system

	.set ItemDataDebuffType,			0x21
	.set ItemDataUnknown,				0x22
	.set ItemDataItemSkill,				0x23

@ Item attributes

	.set ItemAttributeWeapon,			(1 <<  0)
	.set ItemAttributeMagic,			(1 <<  1)
	.set ItemAttributeStaff,			(1 <<  2)
	.set ItemAttributeUnbreakable,		(1 <<  3)
	.set ItemAttributeUnsellable,		(1 <<  4)
	.set ItemAttributeBrave,			(1 <<  5)
	.set ItemAttributeMagicDamage,		(1 <<  6)
	.set ItemAttributeUncounterable,	(1 <<  7)
	.set ItemAttributeReverseTriangle,	(1 <<  8)
	.set ItemAttributeHammerne,			(1 <<  9)
	.set ItemAttributeLock3,			(1 << 10)
	.set ItemAttributeLock1,			(1 << 11)
	.set ItemAttributeLock2,			(1 << 12)
	.set ItemAttributeLock0,			(1 << 13)
	.set ItemAttributeNegateFlying,		(1 << 14)
	.set ItemAttributeNegateCrit,		(1 << 15)
	.set ItemAttributeUnusable,			(1 << 16)
	.set ItemAttributeNegateDefense,	(1 << 17)
	.set ItemAttributeLock4,			(1 << 18)
	.set ItemAttributeLock5,			(1 << 19)
	.set ItemAttributeLock6,			(1 << 20)
	.set ItemAttributeLock7,			(1 << 21)
