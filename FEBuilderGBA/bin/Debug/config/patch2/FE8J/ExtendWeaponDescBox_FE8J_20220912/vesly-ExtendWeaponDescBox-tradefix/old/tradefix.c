#include "proc.h"
#include "mu.h"

void LockGameLogic(void); // 08015361
void TradeMenu_ClearDisplay(ProcPtr proc); // 0802DB49

ProcPtr FindProc(struct ProcScr const * procscr); // 08002E9D

void SetFaceConfig(u32 const * config); // 08005545

extern struct ProcScr const ProcScr_MoveUnit[]; // 089A2C48

static u32 const face_config[] =
{
    0x6000, 6,
    0x7000, 7,
    0x0000, 0,
    0x0000, 0,
};

static void SetMuChrHot(struct MuProc * mu, int chr)
{
    if (mu != NULL)
    {
        mu->config->chr = chr;
        mu->sprite_anim->oam2 &= ~0x3FF;
        mu->sprite_anim->oam2 |= chr;
        mu->sprite_anim->need_sync_img_b = TRUE;
    }
}

void TradeMenu_LockGame_Override(ProcPtr proc)
{
    SetFaceConfig(face_config);
    SetMuChrHot(FindProc(ProcScr_MoveUnit), 572); // move
    LockGameLogic();
}

void TradeMenu_ClearDisplay_Override(ProcPtr proc)
{
    SetFaceConfig(NULL);
    SetMuChrHot(FindProc(ProcScr_MoveUnit), 896); // restore
    TradeMenu_ClearDisplay(proc);
}
