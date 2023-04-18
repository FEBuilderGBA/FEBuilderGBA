#include <stdio.h>
#include "main.h"

// Set 256-col or 224-col BG.
void CGC_LoadMultiPalBG(struct BGData* bgData, u32 colCount) {
  // Init LCDIO stuff.
  SetBgPosition(0, 0, 0);
  SetBgPosition(1, 0, 0);
  SetBgPosition(2, 0, 0);
  SetBgPosition(3, 0, 0);
  SetBgMapDataOffset(0, 0xE000);
  SetBgMapDataOffset(1, 0xE800);
  SetBgMapDataOffset(2, 0xF000);
  SetBgMapDataOffset(3, 0xF800);
  SetBgTileDataOffset(0, 0);
  SetBgTileDataOffset(1, 0);
  SetBgTileDataOffset(2, 0);
  SetBgTileDataOffset(3, 0);
  gLCDIOBuffer.bgControl[3].colorMode = 1;                // 256-col mode.
  gLCDIOBuffer.blendControl.target2_backdrop = false;     // Prevents weird blending into trans colour effect.
  
  // Clear screen entries.
  CpuFill16(0x0, gBg0MapBuffer, 0x1800);
  CpuFastFill(0, (void*)0x6000000, 0x20);       // Empty tile.
  
  // Init gfx.
  Decompress(bgData->gfx, (void*)0x6004000);
  for (int i = 0; i < 640; i++)
    gBg3MapBuffer[i] = i+256;
  
  // Leave paletteslot 2 and 3 empty for text and chatbubble if 224-col BG.
  if (colCount == 224) {
    CopyToPaletteBuffer(bgData->pal, 0, 0x40);
    CopyToPaletteBuffer(bgData->pal+0x20, 0x80, 0x180);
  }
  else
    CopyToPaletteBuffer(bgData->pal, 0, 0x200);
  
  EnableBgSyncByMask(0xF);
  EnablePaletteSync();
}