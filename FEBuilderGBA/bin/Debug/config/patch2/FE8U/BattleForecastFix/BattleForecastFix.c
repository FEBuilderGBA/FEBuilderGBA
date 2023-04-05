#include "gbafe.h"

extern bool MapEventEngineExists();
extern const ProcInstruction gProc_859B630;

u8 BattleForecast_OnBPress(void) {
    if (MapEventEngineExists() == 1) {
        return 0;
    }

    //Reset tile allocation to prevent the bug
    Text_ResetTileAllocation();

    ProcStart(&gProc_859B630, ROOT_PROC_3);

    return ME_DISABLE | ME_END | ME_PLAY_BOOP;
}
