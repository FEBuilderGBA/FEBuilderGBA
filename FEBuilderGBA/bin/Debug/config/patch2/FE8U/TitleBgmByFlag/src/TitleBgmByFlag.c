#include "global.h"

#include "bmsave.h"
#include "ev_triggercheck.h"
#include "opanim.h"
#include "soundwrapper.h"
#include "hardware.h"

#include "TitleBgmByFlag.h"

extern struct ProcCmd gProcScr_TitleScreen[]; //! FE8U: 0x08AA6A50
extern void Title_EndSkipFxListener(void); //! FE8U: 0x080C55B8

int GetTitleScreenBgm(void) {
    int i;

    for (i = 0; i < 3; i++) {
        const struct GameSaveBlock * saveBlock;
        const struct TitleScreenBgmLutEnt * it;

        if (!IsSaveValid(i)) {
            continue;
        }

        saveBlock = GetSaveReadAddr(i);

        ReadPermanentFlags_ret(saveBlock->permanentFlags, gGenericBuffer);

        for (it = gTitleScreenBgmFlagLut; it->flag != -1; it++) {
            if (!CheckPermanentFlagFrom(it->flag, gGenericBuffer)) {
                continue;
            }

            return it->songId;
        }
    }

    return 0x43; // default to vanilla
}

/* Hook; vanilla FE8U = 0x080C63D0 */
void Title_RestartProc(struct TitleScreenProc * proc) {
    EndAllProcChildren(proc);

    Title_EndSkipFxListener();

    gPaletteBuffer[PAL_BACKDROP_OFFSET] = 0;

    EnablePaletteSync();

    proc->mode = 0;

    SetDispEnable(0, 0, 0, 0, 0);

    // Vanilla starts song ID 0x43
    StartBgmExt(GetTitleScreenBgm(), 0, 0);

    return;
}

/* Hook; vanilla FE8U = 0x080C6424 */
void StartTitleScreen_WithMusic(ProcPtr parent) {
    struct TitleScreenProc * proc = Proc_StartBlocking(gProcScr_TitleScreen, parent);
    proc->mode = 0;

    // Vanilla doesn't fade out BGM and uses song ID 0x43
    Sound_FadeOutBGM(1);
    StartBgmExt(GetTitleScreenBgm(), 0, 0);

    return;
}

// unnecessary for our purposes; only declared to satisfy the function below since the struct is not currently in a proper header in decomp
struct SoundRoomProc;

/* Hook; vanilla FE8U = 0x080AFA64 */
void SoundRoomUi_RestartTitleMusic(struct SoundRoomProc * proc) {
    if (!MusicProc4Exists()) {
        // Vanilla uses song ID 0x43
        CallSomeSoundMaybe(GetTitleScreenBgm(), 0, 0xc0, 0x18, 0);
        Proc_Break(proc);
    }

    return;
}

