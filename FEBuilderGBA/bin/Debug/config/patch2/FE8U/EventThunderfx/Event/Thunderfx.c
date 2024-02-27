#include "gbafe.h"

void StartBmThunderfx(int x, int y, ProcPtr parent);

static void CallEventThunderfxForUnitSlot2(struct EventEngineProc * proc)
{
    struct Unit * unit = GetUnitStructFromEventParameter(CHAR_EVT_SLOT2);
    if (unit)
    {
        int x, y;
        x = unit->xPos * 16 - gBmSt.camera.x;
        y = unit->yPos * 16 - gBmSt.camera.y;

        StartBmThunderfx(x, y, proc);
    }
}

const EventScr EventScr_CallThunderfx[] = {
    STARTFADE
    EvtColorFadeSetup(0x0, 0x20, 8, 128, 128, 128) // ENOSUPP in EAstdlib
    STAL(30)
    SOUN(0x11A)
    ASMC(CallEventThunderfxForUnitSlot2)
    STAL(30)
    EvtColorFadeSetup(0x0, 0x20, 4, 256, 256, 256) // ENOSUPP in EAstdlib
    ENDA
};

static void CallEventThunderfxAtPosition(struct EventEngineProc * proc)
{
    int x, y;

    x = gEventSlots[3] * 16 - gBmSt.camera.x;
    y = gEventSlots[4] * 16 - gBmSt.camera.y;

    StartBmThunderfx(x, y, proc);
}

const EventScr EventScr_CallThunderfxAtPosition[] = {
    STARTFADE
    EvtColorFadeSetup(0x0, 0x20, 8, 128, 128, 128) // ENOSUPP in EAstdlib
    STAL(30)
    SOUN(0x11A)
    ASMC(CallEventThunderfxAtPosition)
    STAL(30)
    EvtColorFadeSetup(0x0, 0x20, 4, 256, 256, 256) // ENOSUPP in EAstdlib
    ENDA
};
