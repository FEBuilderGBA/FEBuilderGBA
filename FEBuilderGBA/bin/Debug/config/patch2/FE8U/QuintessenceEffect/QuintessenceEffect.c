#include "global.h"

#include "hardware.h"
#include "sysutil.h"
#include "mapanim.h"
#include "bmlib.h"

struct QuintessenceFxProc
{
    /* 00 */ PROC_HEADER;
    /* 29 */ STRUCT_PAD(0x29, 0x4C);

    /* 4C */ s16 timer;

    /* 4E */ STRUCT_PAD(0x4E, 0x58);

    /* 58 */ int bg2_offset;
};

extern u16 Pal_QuintessenceFx[];
// TODO: We could import the palette from FE7, but this is close enough
#define Pal_QuintessenceFx Pal_GameOverText1

// Defined in EA
extern u8 Tsa_QuintessenceFx[];

extern void TryLockParentProc(ProcPtr);   // FE8U: 0x080854E4
extern void TryUnlockParentProc(ProcPtr); // FE8U: 0x080854F0

// Translation between FE7J and FE8U
#define TmFill BG_Fill
#define InitScanlineEffect InitScanline
#define SetBgOffset BG_SetPosition
#define SetOnHBlankA SetPrimaryHBlankHandler
#define EnableBgSync BG_EnableSyncByMask
#define TmApplyTsa_thm CallARM_FillTileRect
#define fe7j_sub_8013BA0 GetPalFadeStClkEnd1
#define fe7j_sub_8013BAC GetPalFadeStClkEnd2
#define fe7j_sub_8013BBC GetPalFadeStClkEnd3

// =========================================
// Various other functions
// =========================================

// TODO: Nonmatching, but this seems to be close enough
// https://decomp.me/scratch/EoXm2

void fe7j_sub_80781C8(u16 * buf, s16 phase, s16 amplitude, s16 frequency, s16 param_5, s16 param_6, s16 param_7)
{
    int i;

    buf++;

    for (i = 1; i < DISPLAY_HEIGHT; i += 2)
    {
        *buf = ((SIN((i * frequency + phase)) * amplitude * param_7 * ABS(i - param_6)) >> 0x14) + param_5;
        buf += 2;
    }
}

u16 * fe7j_sub_8077CEC(int buf_id, int scanline)
{
    return gManimScanlineBufs[buf_id] + scanline;
}

void QuintessenceFx_OnHBlank(void)
{
    u16 vcount = REG_VCOUNT;

    if (vcount >= DISPLAY_HEIGHT)
    {
        gManimActiveScanlineBuf = gManimScanlineBufs[0];
        vcount = 0;
    }
    else
    {
        vcount++;
    }

    if ((vcount & 1) != 0)
    {
        REG_BG2HOFS = gManimActiveScanlineBuf[DISPLAY_HEIGHT + vcount] + gLCDControlBuffer.bgoffset[BG_2].x;
        REG_BG2VOFS = gManimActiveScanlineBuf[vcount] + gLCDControlBuffer.bgoffset[BG_2].y;
    }
}

// =========================================
// Event shims as ASMCs
// =========================================

struct Proc08C01654
{
    /* 00 */ PROC_HEADER;
    /* 29 */ STRUCT_PAD(0x29, 0x30);
    /* 30 */ int unk_30;
    /* 34 */ int unk_34;
    /* 38 */ int unk_38;
    /* 3C */ int unk_3c;
    /* 40 */ int unk_40;
    /* 44 */ int unk_44;
    /* 48 */ int unk_48;
    /* 4C */ int unk_4c;
    /* 50 */ int unk_50;
};

void fe7j_sub_80120D8(struct Proc08C01654 * proc)
{
    proc->unk_38 = 0;
    return;
}

void fe7j_sub_80120E0(struct Proc08C01654 * proc)
{
    proc->unk_38 += proc->unk_34;

    if (proc->unk_38 > 0xff)
    {
        Proc_Break(proc);
        proc->unk_38 = 0x100;
    }

    // clang-format off
    WriteFadedPaletteFromArchive(
        (proc->unk_3c * (0x100 - proc->unk_38) + proc->unk_38 * proc->unk_48) / 0x100,
        (proc->unk_40 * (0x100 - proc->unk_38) + proc->unk_38 * proc->unk_4c) / 0x100,
        (proc->unk_44 * (0x100 - proc->unk_38) + proc->unk_38 * proc->unk_50) / 0x100,
        proc->unk_30
    );
    // clang-format on

    return;
}

// clang-format off

const struct ProcCmd ProcScr_FE7J_08C01654[] = {
    PROC_YIELD,
    PROC_CALL(fe7j_sub_80120D8),
    PROC_REPEAT(fe7j_sub_80120E0),
    PROC_END,
};

// clang-format on

void fe7j_sub_8012150(int a, int b, int c, int d, int e, ProcPtr parent)
{
    struct Proc08C01654 * proc = Proc_StartBlocking(ProcScr_FE7J_08C01654, parent);

    proc->unk_34 = (b & 0xff) == 0x80 ? 0x100 : b & 0xff;
    proc->unk_3c = fe7j_sub_8013BA0();
    proc->unk_40 = fe7j_sub_8013BAC();
    proc->unk_44 = fe7j_sub_8013BBC();
    proc->unk_30 = a;
    proc->unk_48 = c;
    proc->unk_4c = d;
    proc->unk_50 = e;

    return;
}

void FE7J_EventE5_Sim_Type_A(ProcPtr proc)
{
    fe7j_sub_8012150(-1, 0x20, 0x08021A0 & 0x3FF, (0x08021A0 >> 10) & 0x3FF, (0x08021A0 >> 0x14) & 0x3FF, proc);
    return;
}

void FE7J_EventE5_Sim_Type_B(ProcPtr proc)
{
    fe7j_sub_8012150(-1, 0x20, 0x1004100 & 0x3FF, (0x1004100 >> 10) & 0x3FF, (0x1004100 >> 0x14) & 0x3FF, proc);
    return;
}

void fe7j_sub_80121FC(void)
{
    WriteFadedPaletteFromArchive(0x100, 0x100, 0x100, -1);
    return;
}

// =========================================
// Core Quintessence Effect proc functions
// =========================================

void QuintessenceFx_ParallelWorker(struct QuintessenceFxProc * proc)
{
    proc->bg2_offset++;

    fe7j_sub_80781C8(fe7j_sub_8077CEC(1, 0), proc->bg2_offset, 3, 2, 0, 60, 16);
    fe7j_sub_80781C8(fe7j_sub_8077CEC(1, 160), proc->bg2_offset, 2, 4, 0, 60, 16);

    SwapScanlineBufs();
}

void QuintFxBg2_Init(struct QuintessenceFxProc * proc)
{
    proc->bg2_offset = 0;
}

void QuintFxBg2_Loop(struct QuintessenceFxProc * proc)
{
    proc->bg2_offset++;
    SetBgOffset(BG_2, proc->bg2_offset >> 2, proc->bg2_offset >> 1);
}

// clang-format off

const struct ProcCmd ProcScr_QuintessenceFxBg2Scroll[] = {
    PROC_CALL(QuintFxBg2_Init),
    PROC_REPEAT(QuintFxBg2_Loop),

    PROC_END,
};

// clang-format on

void QuintessenceFx_Init_Main(struct QuintessenceFxProc * proc)
{
    gLCDControlBuffer.bldcnt.effect = 1;

    gLCDControlBuffer.blendCoeffA = 0;
    gLCDControlBuffer.blendCoeffB = 16;
    gLCDControlBuffer.blendY = 0;

    SetBlendTargetA(0, 0, 1, 0, 0);
    SetBlendTargetB(0, 0, 0, 1, 0);

    ApplyPalette(Pal_QuintessenceFx, 5);
    Decompress(Img_ChapterIntroFog, (void *)0x06004000);
    TmApplyTsa_thm(gBG2TilemapBuffer, (void* )0x085A647C, 0x5200);

    EnableBgSync(BG2_SYNC_BIT | BG3_SYNC_BIT);
    SetBgOffset(BG_2, 0, 0);

    proc->timer = 0;
    proc->bg2_offset = 0;

    InitScanlineEffect();

    SetOnHBlankA(QuintessenceFx_OnHBlank);
    StartParallelWorker(QuintessenceFx_ParallelWorker, proc);

    Proc_Start(ProcScr_QuintessenceFxBg2Scroll, PROC_TREE_VSYNC);
}

void QuintessenceFx_Loop_A(struct QuintessenceFxProc * proc)
{
    int bld_amt = proc->timer++ >> 1;

    gLCDControlBuffer.bldcnt.effect = 1;

    gLCDControlBuffer.blendCoeffA = bld_amt;
    gLCDControlBuffer.blendCoeffB = 16 - bld_amt;
    gLCDControlBuffer.blendY = 0;

    if (bld_amt == 16)
    {
        Proc_Break(proc);
    }
}

void QuintessenceFx_ResetBlend(struct QuintessenceFxProc * proc)
{
    gLCDControlBuffer.bldcnt.effect = 1;

    gLCDControlBuffer.blendCoeffA = 16;
    gLCDControlBuffer.blendCoeffB = 0;
    gLCDControlBuffer.blendY = 0;

    SetBlendTargetA(0, 0, 1, 0, 0);
    SetBlendTargetB(0, 0, 0, 1, 1);

    proc->timer = 0;
}

void QuintessenceFx_Loop_B(struct QuintessenceFxProc * proc)
{
    int bld_amt = proc->timer++ >> 2;

    gLCDControlBuffer.bldcnt.effect = 1;

    gLCDControlBuffer.blendCoeffA = 16 - bld_amt;
    gLCDControlBuffer.blendCoeffB = bld_amt;
    gLCDControlBuffer.blendY = 0;

    if (bld_amt == 10)
    {
        Proc_Break(proc);
    }
}

void QuintessenceFx_Loop_C(struct QuintessenceFxProc * proc)
{
    int bld_amt = proc->timer++ >> 2;

    gLCDControlBuffer.bldcnt.effect = 1;

    gLCDControlBuffer.blendCoeffA = 16 - bld_amt;
    gLCDControlBuffer.blendCoeffB = bld_amt;
    gLCDControlBuffer.blendY = 0;

    if (bld_amt == 16)
    {
        Proc_Break(proc);
    }
}

void QuintessenceFx_OnEnd(void)
{
    Proc_End(Proc_Find(ProcScr_QuintessenceFxBg2Scroll));

    SetOnHBlankA(NULL);

    SetBgOffset(BG_2, 0, 0);
    TmFill(gBG2TilemapBuffer, 0);

    EnableBgSync(BG2_SYNC_BIT);

    gLCDControlBuffer.bg0cnt.priority = 0;
    gLCDControlBuffer.bg1cnt.priority = 1;
    gLCDControlBuffer.bg2cnt.priority = 2;
    gLCDControlBuffer.bg2cnt.priority = 3;
}

// clang-format off

const struct ProcCmd ProcScr_QuintessenceFx[] = {
    PROC_SET_END_CB(QuintessenceFx_OnEnd),

    PROC_CALL(TryLockParentProc),
    PROC_CALL(QuintessenceFx_Init_Main),
    PROC_REPEAT(QuintessenceFx_Loop_A),
    PROC_CALL(TryUnlockParentProc),

    PROC_BLOCK,

PROC_LABEL(0),
    PROC_CALL(TryLockParentProc),
    PROC_CALL(QuintessenceFx_ResetBlend),
    PROC_REPEAT(QuintessenceFx_Loop_B),
    PROC_CALL(TryUnlockParentProc),

    PROC_BLOCK,

PROC_LABEL(1),
    PROC_CALL(TryLockParentProc),
    PROC_CALL(QuintessenceFx_ResetBlend),
    PROC_REPEAT(QuintessenceFx_Loop_C),
    PROC_CALL(TryUnlockParentProc),

    PROC_BLOCK,

    PROC_END,
};

// clang-format on

void StartQuintessenceStealEffect(struct Proc * parent)
{
    Proc_Start(ProcScr_QuintessenceFx, parent);
}

void QuintessenceFx_Goto_B(void)
{
    Proc_Goto(Proc_Find(ProcScr_QuintessenceFx), 0);
}

void QuintessenceFx_Goto_C(void)
{
    Proc_Goto(Proc_Find(ProcScr_QuintessenceFx), 1);
}

void EndQuintessenceStealEffect(void)
{
    Proc_End(Proc_Find(ProcScr_QuintessenceFx));
}
