#include "ModularSave.h"

// This contains Suspend functions suitables for MS save chunks
// In case there's no vanilla function that fit the bill
// If a Save chunk function is also used in a Suspend chunk, it will be in MSaFuncs.c

void MSu_SaveActionState(void* target, unsigned size) {
	StoreRNStateToActionStruct();
	WriteAndVerifySramFast(&gActionData, target, size);
}

void MSu_LoadActionState(void* source, unsigned size) {
	gpReadSramFast(source, &gActionData, size);
	LoadRNStateFromActionStruct();
}

void MSu_SavePlayerUnits(void* target, unsigned size) {
	for (unsigned i = 0; i < 51; ++i) {
		PackUnitStructForSuspend(GetUnit(i+1), gGenericBuffer);
		WriteAndVerifySramFast(gGenericBuffer, target + 0x34*i, 0x34);
	}
}

void MSu_LoadPlayerUnits(void* source, unsigned size) {
	for (unsigned i = 0; i < 51; ++i)
		UnpackUnitStructFromSuspend(source + 0x34*i, GetUnit(i+1));
}

void MSu_SaveOtherUnits(void* target, unsigned size) {
	// Save reds
	for (unsigned i = 0; i < 50; ++i) {
		PackUnitStructForSuspend(GetUnit(0x81+i), gGenericBuffer);
		WriteAndVerifySramFast(gGenericBuffer, target + 0x34*i, 0x34);
	}

	// Save greens
	for (unsigned i = 0; i < 10; ++i) {
		PackUnitStructForSuspend(GetUnit(0x41+i), gGenericBuffer);
		WriteAndVerifySramFast(gGenericBuffer, target + 0x34*(50+i), 0x34);
	}
}

void MSu_LoadOtherUnits(void* source, unsigned size) {
	// Load reds
	for (unsigned i = 0; i < 50; ++i)
		UnpackUnitStructFromSuspend(source + 0x34*i, GetUnit(0x81+i));

	// Load greens
	for (unsigned i = 0; i < 10; ++i)
		UnpackUnitStructFromSuspend(source + 0x34*(50+i), GetUnit(0x41+i));
}

void MSu_SaveMenuRelated(void* target, unsigned size) {
	static const void(*PackMenuRelatedSaveStruct)(void*) = (void(*)(void*))(0x804F714+1);

	u8 buf[0x10];

	PackMenuRelatedSaveStruct(buf);
	WriteAndVerifySramFast(buf, target, size);
}

void MSu_LoadMenuRelated(void* source, unsigned size) {
	static const void(*UnpackMenuRelatedSaveStruct)(void*) = (void(*)(void*))(0x804F754+1);

	u8 buf[0x10];

	gpReadSramFast(source, buf, size);
	UnpackMenuRelatedSaveStruct(buf);
}

void MSu_SaveDungeonState(void* target, unsigned size) {
	static const void(*PackIdk)(void*) = (void(*)(void*))(0x8037E08+1);

	u8 buf[0xC];

	PackIdk(buf);
	WriteAndVerifySramFast(buf, target, size);
}

void MSu_LoadDungeonState(void* source, unsigned size) {
	static const void(*UnpackIdk)(void*) = (void(*)(void*))(0x8037E30+1);

	u8 buf[0xC];

	gpReadSramFast(source, buf, size);
	UnpackIdk(buf);
}

void MSu_SaveEventCounter(void* target, unsigned size) {
	unsigned counter = GetEventCounter();
	WriteAndVerifySramFast(&counter, target, size);
}

void MSu_LoadEventCounter(void* source, unsigned size) {
	unsigned counter;

	gpReadSramFast(source, &counter, size);
	SetEventCounter(counter);
}

void MSu_LoadClaimFlagsFromParentSave(void* source, unsigned size) {
	// This doesn't load anything from the actual block
	Set0203EDB4(MS_GetClaimFlagsFromGameSave(gChapterData.saveSlotIndex));
}
