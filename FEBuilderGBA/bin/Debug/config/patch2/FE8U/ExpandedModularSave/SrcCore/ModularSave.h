#ifndef MODULAR_SAVE_H
#define MODULAR_SAVE_H

#include "gbafe.h"

struct SaveBlockDecl {
	/* 00 */ u16 offset;
	/* 02 */ u16 type;
};

struct SaveChunkDecl {
	/* 00 */ u16 offset;
	/* 02 */ u16 size;

	/* 04 */ void (*save)(void* target, unsigned size);
	/* 08 */ void (*load)(void* source, unsigned size);

	/* 0C */ u16 identifier;
};

extern const u8 gMS_ChapterStateChunkId;
extern const u8 gMS_BWLChunkId;
extern const u8 gMS_PermanentEidsChunkId;
extern const u8 gMS_ClaimFlagsChunkId;
extern const u8 gMS_WMDataChunkId;

extern const struct SaveBlockDecl gSaveBlockDecl[];
extern const u16 gSaveBlockTypeSizeLookup[];

extern const struct SaveChunkDecl gGameSaveChunks[];
extern const struct SaveChunkDecl gSuspendSaveChunks[];

void* MS_GetSaveAddressBySlot(unsigned slot);
const struct SaveChunkDecl* MS_FindGameSaveChunk(unsigned chunkId);
const struct SaveChunkDecl* MS_FindSuspendSaveChunk(unsigned chunkId);
void MS_LoadChapterStateFromGameSave(unsigned slot, struct ChapterState* target);
u32 MS_GetClaimFlagsFromGameSave(unsigned slot);

// TODO: add to libgbafe

void SaveUnit(struct Unit* unit, void* target) __attribute__((long_call));
void LoadSavedUnit(void* source, struct Unit* unit) __attribute__((long_call));

void SaveWMStuff(void*, void*) __attribute__((long_call));
void LoadWMStuff(void*, void*) __attribute__((long_call));

extern u8 gSomeWMEventRelatedStruct;

void StoreRNStateToActionStruct(void) __attribute__((long_call));
void LoadRNStateFromActionStruct(void) __attribute__((long_call));

// Those aren't consistent:
// the packing function works with a buffer
// But the unpacking functions loads from SRAM directly
void PackUnitStructForSuspend(struct Unit* unit, void* target) __attribute__((long_call));
void UnpackUnitStructFromSuspend(void* source, struct Unit* unit) __attribute__((long_call));

void Set0203EDB4(u32 value) __attribute__((long_call));

#endif // MODULAR_SAVE_H
