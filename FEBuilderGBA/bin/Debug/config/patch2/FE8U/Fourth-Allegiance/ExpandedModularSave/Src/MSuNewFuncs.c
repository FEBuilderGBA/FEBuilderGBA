#include "ModularSave.h"

void MSu_SavePurpleUnits(void* target, unsigned size) {
	// Save purples
	for (unsigned i = 0; i < 20; ++i) {
		PackUnitStructForSuspend(GetUnit(0xC1+i), gGenericBuffer);
		WriteAndVerifySramFast(gGenericBuffer, target + 0x34*i, 0x34);
	}
}

void MSu_LoadPurpleUnits(void* source, unsigned size) {
	// Load purples
	for (unsigned i = 0; i < 20; ++i)
		UnpackUnitStructFromSuspend(source + 0x34*i, GetUnit(0xC1+i));
}
