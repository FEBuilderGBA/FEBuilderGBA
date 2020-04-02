
#include "gbafe.h"

// Portrait drawn on backgrounds now blink

// The functionnaly is already in the game, only unused
// This makes it used.

// TODO: update reference and add to CLib
static const struct ProcInstruction* spProc_BgFaceBlinkProc = (const void*)(0x08591204);
static void(*DisplayBgFaceCore)(u16* bgOut, unsigned portrait, unsigned tileId, unsigned palId) = (const void*)(0x08005CA4+1);

void NuDisplayBgFace(struct Proc* proc, u16* bgOut, unsigned portrait, unsigned tileId, unsigned palId)
{
	EndEachProc(spProc_BgFaceBlinkProc);
	memset(bgOut + TILEMAP_INDEX(0, -1), 0, 0x40);
	DisplayBgFaceCore(bgOut, portrait, tileId, palId);

	if (proc)
	{
		const struct PortraitData* data = GetPortraitData(portrait);

		if (!data->pPortraitGraphics)
			return;

		struct FaceBlinkProc* blink = START_PROC(spProc_BgFaceBlinkProc, proc);

		blink->output = ShouldPortraitBeSmol(portrait) ? bgOut + TILEMAP_INDEX(0, -1) : bgOut;
		blink->tileId = tileId;
		blink->paletteId = palId;
		blink->portraitId = portrait;
		blink->blinkControl = data->blinkBehaviorKind;
	}
}
