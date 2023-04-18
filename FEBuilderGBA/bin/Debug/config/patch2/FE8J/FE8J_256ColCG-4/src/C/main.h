#ifndef MAIN_H
#define MAIN_H
#include "gbafe.h"

// Vanilla struct.
struct BGData {
  const void* gfx;
  const void* tsa;
  const u16* pal;
};

void CGC_LoadMultiPalBG(struct BGData* bgData, u32 colCount);

#endif // MAIN_H