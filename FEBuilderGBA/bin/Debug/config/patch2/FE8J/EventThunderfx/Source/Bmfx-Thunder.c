#include "gbafe.h"

extern struct BmBgxConf BmBgfxConf_EventThunder[];

struct ProcEventThunderfx {
    PROC_HEADER;

    /* 3C */ int x, y;
};

void EventThunderfx_Init(struct ProcEventThunderfx * proc)
{
    gLCDControlBuffer.bg0cnt.priority = 0;
    gLCDControlBuffer.bg1cnt.priority = 1;
    gLCDControlBuffer.bg2cnt.priority = 0;
    gLCDControlBuffer.bg3cnt.priority = 3;

    SetBlendAlpha(0x10, 0x10);
    SetBlendTargetA(0, 0, 1, 0, 0);
    SetBlendTargetB(0, 0, 0, 1, 1);

    StartBmBgfx(
        BmBgfxConf_EventThunder,
        BG_2,
        proc->x,
        proc->y,
        0,
        0x2000,
        0xF,
        NULL, proc
    );
}

void EventThunderfx_End(struct ProcEventThunderfx * proc)
{
    SetBlendConfig(BLEND_EFFECT_NONE, 0, 0x10, 0);
    InitBmBgLayers();
}

const struct ProcCmd ProcScr_EventThunderfx[] = {
    PROC_YIELD,
    PROC_CALL(EventThunderfx_Init),
    PROC_WHILE(CheckBmBgfxDone),
    PROC_CALL(EventThunderfx_End),
    PROC_END,
};

void StartBmThunderfx(int x, int y, ProcPtr parent)
{
    struct ProcEventThunderfx * proc;
    if (parent)
        proc = Proc_StartBlocking(ProcScr_EventThunderfx, parent);
    else
        proc = Proc_Start(ProcScr_EventThunderfx, PROC_TREE_3);

    proc->x = x - 0x18;
    proc->y = y - 0x78;
}
