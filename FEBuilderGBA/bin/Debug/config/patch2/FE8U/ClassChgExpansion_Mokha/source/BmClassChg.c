#include "global.h"
#include "bm.h"
#include "bmio.h"
#include "uiutils.h"
#include "hardware.h"
#include "ctc.h"
#include "prepscreen.h"
#include "uimenu.h"
#include "scene.h"
#include "fontgrp.h"
#include "ekrbattle.h"
#include "efxbattle.h"
#include "classdisplayfont.h"
#include "bmunit.h"
#include "bmitem.h"
#include "bmusemind.h"
#include "bmarch.h"
#include "classchg.h"
#include "sysutil.h"
#include "constants/characters.h"
#include "constants/classes.h"
#include "constants/items.h"

#include "ClassChgExpansion.h"

void sub_805AE14(void *);
u8 ClassChgMenuSelOnPressB(struct MenuProc * pmenu, struct MenuItemProc * pmitem);

/**
 * Stage1: handler
 */
u32 PromoHandler_SetupAndStartUI(struct ProcPromoHandler * proc)
{
    u8 jid_list[0x10];
    struct Unit * unit;
    int i, amt = 0;

    if (proc->bmtype == PROMO_HANDLER_TYPE_TRANINEE)
    {
        for (i = 0; i < 0x40; ++i)
        {
            const struct TraineeDataRe * it;
            unit = GetUnit(i);

            if (!UNIT_IS_VALID(unit))
                continue;

            if (unit->state & (US_BIT16 | US_DEAD))
                continue;

            for (it = gpTraineesRe; it->jid != 0; it++)
            {
                if (it->jid != UNIT_CLASS_ID(unit))
                    continue;

                if (unit->level >= it->level)
                {
                    amt = GetClasschgList(unit, unit->items[proc->item_slot], jid_list, sizeof(jid_list));
                    if (amt <= 0)
                        continue;

                    if (amt == 1)
                    {
                        proc->jid = jid_list[0];
                        proc->sel_en = false;
                    }

                    MakePromotionScreen(proc, UNIT_CHAR_ID(unit), TERRAIN_PLAINS);
                    return PROMO_HANDLER_STAT_IDLE;
                }
            }
        }
        return PROMO_HANDLER_STAT_END;
    }

    proc->sel_en = true;
    unit = GetUnitFromCharIdAndFaction(proc->pid, FACTION_BLUE);
    amt = GetClasschgList(unit, unit->items[proc->item_slot], jid_list, sizeof(jid_list));

    if (amt <= 0)
    {
        if (proc->bmtype == PROMO_HANDLER_TYPE_PREP)
        {
            BMapDispResume();
            RefreshBMapGraphics();
        }
        return PROMO_HANDLER_STAT_END;
    }

    if (amt == 1)
    {
        proc->jid = jid_list[0];
        proc->sel_en = false;
    }

    MakePromotionScreen(proc, proc->pid, TERRAIN_PLAINS);
    return PROMO_HANDLER_STAT_IDLE;
}

/**
 * Stage2: main
 */

/* LynJump! */
void PromoMain_HandleType(struct ProcPromoMain * proc)
{
    struct ProcPromoHandler * parent = proc->proc_parent;

    if (parent->sel_en == false)
    {
        proc->jid = parent->jid;
        Proc_Goto(proc, PROMOMAIN_LABEL_POST_SEL);
        return;
    }

    Proc_Goto(proc, PROMOMAIN_LABEL_SEL_EN);
}

/**
 * Stage3: sel
 */
const struct ProcCmd ProcScr_ClassChgSelInfoRepo[] =
{
    PROC_NAME("ClassChgSelInfoRepo"),
    PROC_BLOCK,
    PROC_END,
};

struct ProcClassChgDataRepo * GetClassChgSelInfo(void)
{
    return Proc_Find(ProcScr_ClassChgSelInfoRepo);
}

/* LynJump! */
void Make6C_PromotionMenuSelect(struct ProcPromoSel * proc)
{
    int i;
    u16 weapon;
    u8 jid_list[0x10];
    struct ProcPromoMain * pproc = proc->proc_parent;
    struct ProcPromoHandler * ppproc = pproc->proc_parent;
    struct ProcClassChgDataRepo * repo;
    struct Unit * unit;

    pproc->stat = PROMO_MAIN_STAT_2;
    proc->pid = pproc->pid;
    proc->u50 = 9;

    BG_Fill(gBG0TilemapBuffer, 0);
    BG_Fill(gBG1TilemapBuffer, 0);
    BG_Fill(gBG2TilemapBuffer, 0);

    ResetText();
    LoadUiFrameGraphics();
    LoadObjUIGfx();
    sub_80CD47C(0, -1, 0xFB * 2, 0x58, 6);
    ClassChgLoadUI();
    sub_80CD408(proc->u50, 0x8C * 2, 0x68);

    unit = GetUnitFromCharIdAndFaction(proc->pid, FACTION_BLUE);
    if (UNIT_IS_VALID(unit))
    {
        weapon = GetUnitEquippedWeapon(unit);
        repo = Proc_Start(ProcScr_ClassChgSelInfoRepo, proc);

        repo->unit = unit;
        repo->amt = GetClasschgList(unit, unit->items[ppproc->item_slot], jid_list, sizeof(jid_list));

        if (repo->amt > UNIT_PROMOT_LIST_MAX)
            repo->amt = UNIT_PROMOT_LIST_MAX;

        for (i = 0; i < repo->amt; i++)
        {
            repo->infos[i].jid = jid_list[i];
            repo->infos[i].use_weapon = LoadClassBattleSprite(&repo->infos[i].banim_idx, jid_list[i], weapon);
            repo->infos[i].desc = GetClassData(jid_list[i])->descTextId;
        }

        proc->weapon = weapon;
    }

    proc->stat = 1;
    proc->menu_index = 0;
    LoadClassReelFontPalette(proc, UNIT_CLASS_ID(unit));
    LoadClassNameInClassReelFont(proc);
    LoadObjUIGfx();

    proc->menu_proc = NewClassChgMenuSelect(proc);

    if (ppproc->bmtype == PROMO_HANDLER_TYPE_BM)
    {
        RestartMuralBackground();
        BG_EnableSyncByMask(0xF);
    }
}

/* LynJump! */
void sub_80CCF60(struct ProcPromoSel * proc)
{
    u16 tmp;
    struct ProcClassChgDataRepo * repo;
    repo = GetClassChgSelInfo();

    ResetTextFont();
    ResetText();
    BG_EnableSyncByMask(BG0_SYNC_BIT | BG1_SYNC_BIT | BG2_SYNC_BIT | BG3_SYNC_BIT);

    InitTalk(0x100, 2, 0);
    ChangeClassDescription(repo->infos[proc->menu_index].desc);
    SetTalkPrintDelay(-1);

    gLCDControlBuffer.bg0cnt.priority = 0;
    gLCDControlBuffer.bg1cnt.priority = 2;
    gLCDControlBuffer.bg2cnt.priority = 1;
    gLCDControlBuffer.bg3cnt.priority = 3;

    BG_EnableSyncByMask(BG0_SYNC_BIT | BG1_SYNC_BIT | BG2_SYNC_BIT | BG3_SYNC_BIT);

    tmp = REG_BG0CNT;
    tmp &= 0xFFFC;
    REG_BG0CNT = tmp + 1;
    tmp = REG_BG1CNT;
    tmp &= 0xFFFC;
    REG_BG1CNT = tmp + 1;
    tmp = REG_BG2CNT;
    tmp &= 0xFFFC;
    REG_BG2CNT = tmp + 1;
    tmp = REG_BG3CNT;
    tmp &= 0xFFFC;
    REG_BG3CNT = tmp + 1;
}

/**
 * Stage4: menu
 */

const struct MenuRect ClassChgMenuRectRe = {
    .x = 2,
    .y = 0,
    .w = 11,
    .h = 0
};

const struct MenuItemDef MenuItems_PromoSelRe[];
u32 ClassChgMenuSelOnInit(struct MenuProc * proc);
u32 ClassChgMenuSelOnEnd(struct MenuProc * proc);

const struct MenuDef MenuDef_PromoSelRe = {
    .rect = { 11, 2, 8, 0 },
    .menuItems = MenuItems_PromoSelRe,
    .onInit = (void *)ClassChgMenuSelOnInit,
    .onEnd = (void *)ClassChgMenuSelOnEnd,
    .onBPress = ClassChgMenuSelOnPressB,
};

/* LynJump! */
void ClassChgMenuExec(struct ProcClassChgMenuSel * proc)
{
    proc->unk4C = 0;
    ResetTextFont();
    ResetText();
    SetTextFontGlyphs(0);
    InitTextFont(&gFontClassChg, (void *)BG_VRAM + 0x1000, 0x1000 / 0x20, 5);
    SetTextFont(&gFontClassChg);
    proc->pmenu = StartMenuCore(
		&MenuDef_PromoSelRe,
		ClassChgMenuRectRe,
		2,
		0,
		0,
		0,
		proc);
}

u8 ClassChgReMenuItem_CheckEnable(const struct MenuItemDef * mitemdef, int number);
int ClassChgReMenuItem_OnTextDraw(struct MenuProc * menu, struct MenuItemProc * mitem);
u8 ClassChgReMenuItem_OnSelect(struct MenuProc * menu, struct MenuItemProc * mitem);
int ClassChgReMenuItem_OnChange(struct MenuProc * menu, struct MenuItemProc * mitem);

const struct MenuItemDef MenuItems_PromoSelRe[] = {
    {
        .helpMsgId = 0x6DC,     /* Discard items. Important[NL]items cannot be discarded. */
        .color = TEXT_COLOR_SYSTEM_WHITE,
        .overrideId = 0,
        .isAvailable = ClassChgReMenuItem_CheckEnable,
        .onDraw = ClassChgReMenuItem_OnTextDraw,
        .onSelected = ClassChgReMenuItem_OnSelect,
        .onSwitchIn = ClassChgReMenuItem_OnChange
    },
    {
        .helpMsgId = 0x6DC,     /* Discard items. Important[NL]items cannot be discarded. */
        .color = TEXT_COLOR_SYSTEM_WHITE,
        .overrideId = 1,
        .isAvailable = ClassChgReMenuItem_CheckEnable,
        .onDraw = ClassChgReMenuItem_OnTextDraw,
        .onSelected = ClassChgReMenuItem_OnSelect,
        .onSwitchIn = ClassChgReMenuItem_OnChange
    },
    {
        .helpMsgId = 0x6DC,     /* Discard items. Important[NL]items cannot be discarded. */
        .color = TEXT_COLOR_SYSTEM_WHITE,
        .overrideId = 2,
        .isAvailable = ClassChgReMenuItem_CheckEnable,
        .onDraw = ClassChgReMenuItem_OnTextDraw,
        .onSelected = ClassChgReMenuItem_OnSelect,
        .onSwitchIn = ClassChgReMenuItem_OnChange
    },
    {
        .helpMsgId = 0x6DC,     /* Discard items. Important[NL]items cannot be discarded. */
        .color = TEXT_COLOR_SYSTEM_WHITE,
        .overrideId = 3,
        .isAvailable = ClassChgReMenuItem_CheckEnable,
        .onDraw = ClassChgReMenuItem_OnTextDraw,
        .onSelected = ClassChgReMenuItem_OnSelect,
        .onSwitchIn = ClassChgReMenuItem_OnChange
    },
    {
        .helpMsgId = 0x6DC,     /* Discard items. Important[NL]items cannot be discarded. */
        .color = TEXT_COLOR_SYSTEM_WHITE,
        .overrideId = 4,
        .isAvailable = ClassChgReMenuItem_CheckEnable,
        .onDraw = ClassChgReMenuItem_OnTextDraw,
        .onSelected = ClassChgReMenuItem_OnSelect,
        .onSwitchIn = ClassChgReMenuItem_OnChange
    },
    {
        .helpMsgId = 0x6DC,     /* Discard items. Important[NL]items cannot be discarded. */
        .color = TEXT_COLOR_SYSTEM_WHITE,
        .overrideId = 5,
        .isAvailable = ClassChgReMenuItem_CheckEnable,
        .onDraw = ClassChgReMenuItem_OnTextDraw,
        .onSelected = ClassChgReMenuItem_OnSelect,
        .onSwitchIn = ClassChgReMenuItem_OnChange
    },
    {0}
};

u8 ClassChgReMenuItem_CheckEnable(const struct MenuItemDef * mitemdef, int number)
{
    struct ProcClassChgDataRepo * repo;
    repo = GetClassChgSelInfo();

    if (repo && number < repo->amt)
        return MENU_ENABLED;

    return MENU_NOTSHOWN;
}

int ClassChgReMenuItem_OnTextDraw(struct MenuProc * menu, struct MenuItemProc * mitem)
{
    struct ProcClassChgDataRepo * repo;
    repo = GetClassChgSelInfo();

    ClassChgMenuOnDrawCore(menu, mitem, GetStringFromIndex(GetClassData(repo->infos[mitem->itemNumber].jid)->nameTextId));
    return 0;
}

u8 ClassChgReMenuItem_OnSelect(struct MenuProc * menu, struct MenuItemProc * mitem)
{
    u8 jid;
    struct Unit * unit;
    struct ProcClassChgMenuSel * parent = menu->proc_parent;
    struct ProcPromoSel * gparent = parent->proc_parent;
    struct ProcPromoMain * ggparent = gparent->proc_parent;
    struct ProcClassChgDataRepo * repo = GetClassChgSelInfo();

    if (!repo || gparent->stat != 0)
        return 0;

    unit = repo->unit;
    jid = repo->infos[mitem->itemNumber].jid;
    ggparent->jid = jid;

    switch (jid) {
    case CLASS_RANGER:
    case CLASS_RANGER_F:
        if (unit->state & US_IN_BALLISTA)
            TryRemoveUnitFromBallista(unit);
        break;
    }

#if 0
    /* No I don't want sub confirm menu */

    InitTextFont(&gFontClassChgMenu, (void *)BG_VRAM + 0x1000, 0x80, 0x5);
    TileMap_FillRect(TILEMAP_LOCATED(gBG0TilemapBuffer, 9, 4), 0xA, 0x6, 0);
    BG_EnableSyncByMask(BG0_SYNC_BIT);
    StartMenuExt(&Menu_PromoSubConfirm, 2, 0, 0, 0, menu);
#else
    /* Directly promote unit! */
    EndAllProcChildren(ggparent);
    ClassChgLoadEfxTerrain();
    Proc_Goto(ggparent, PROMOMAIN_LABEL_POST_SEL);
#endif

    return MENU_ACT_SKIPCURSOR | MENU_ACT_END | MENU_ACT_SND6A;
}

int ClassChgReMenuItem_OnChange(struct MenuProc * menu, struct MenuItemProc * mitem)
{
    struct ProcClassChgMenuSel * parent = menu->proc_parent;
    struct ProcPromoSel * gparent = parent->proc_parent;
    struct ProcClassChgDataRepo * repo = GetClassChgSelInfo();

    gparent->stat = 1;
    gparent->menu_index = mitem->itemNumber;

    if (repo)
        ChangeClassDescription(repo->infos[mitem->itemNumber].desc);

    SetTalkPrintDelay(-1);
    return 0;
}

/**
 * Stage5: post confirm
 */

/* LynJump! */
void ExecUnitPromotion(struct Unit * unit, u8 classId, int itemIdx, s8 unk)
{
    if (itemIdx != -1)
        gBattleActor.weaponBefore = gBattleTarget.weaponBefore = unit->items[itemIdx];

    gBattleActor.weapon = GetClassAnimWeapon(classId);
    gBattleTarget.weapon = GetClassAnimWeapon(UNIT_CLASS_ID(unit));

    InitBattleUnitWithoutBonuses(&gBattleTarget, unit);

    ApplyUnitPromotion(unit, classId);

    InitBattleUnitWithoutBonuses(&gBattleActor, unit);

    GenerateBattleUnitStatGainsComparatively(&gBattleActor, &gBattleTarget.unit);

    SetBattleUnitTerrainBonusesAuto(&gBattleActor);
    SetBattleUnitTerrainBonusesAuto(&gBattleTarget);

    if (unk)
        unit->state |= US_HAS_MOVED;

    if (itemIdx != -1)
        UnitUpdateUsedItem(unit, itemIdx);

    gBattleHitArray[0].attributes = 0;
    gBattleHitArray[0].info = BATTLE_HIT_INFO_END;
    gBattleHitArray[0].hpChange = 0;

    gBattleStats.config = BATTLE_CONFIG_PROMOTION;

    return;
}

/**
 * Misc
 */

void LoadClassNameInClassReelFont(struct ProcPromoSel * proc)
{
    char str[0x20];
    int index;
    struct ProcClassChgDataRepo * repo = GetClassChgSelInfo();
    u8 jid = repo->infos[proc->menu_index].jid;
    u32 xOffs = 0x74;
    const struct ClassData *class = GetClassData(jid);
    GetStringFromIndexInBuffer(class->nameTextId, str);

    for (index = 0; index < 0x14 && str[index] != '\0'; index++)
    {
        struct ClassDisplayFont * font = GetClassDisplayFontInfo(str[index]);
        if (font)
        {
            if (font->a)
            {
                PutSpriteExt(4, xOffs - font->xBase - 2, font->yBase + 6, font->a, 0x81 << 7);
                xOffs += font->width - font->xBase;
            }
        } else
            xOffs += 4;

        if (xOffs > 0xF0)
            break;
    }

    if (proc->u44 < 0xff)
        proc->u44++;
}

void LoadBattleSpritesForBranchScreen(struct ProcPromoSel * proc)
{
    u8 b;
    struct ProcPromoSel * p2;
    struct ProcPromoSel * c2;
    struct Anim * anim1;
    struct Anim * anim2;
    struct Unit copied_unit;
    void * tmp;
    u16 sp58;
    struct ProcClassChgDataRepo * repo = GetClassChgSelInfo();

    anim1 = gUnknown_030053A0.anim1;
    anim2 = gUnknown_030053A0.anim2;

    p2 = (void *)gUnknown_0201FADC.proc14;
    c2 = (void *)gUnknown_0201FADC.proc18;

    tmp = &gUnknown_030053A0;

    if (proc->stat == 1)
    {
        u16 r4, r6;
        s16 i;
        struct Unit *unit;
        const struct BattleAnimDef * battle_anim_ptr;
        u32 battle_anim_id;
        u16 ret;

        if ((s16) p2->sprite[0] <= 0x117)
        {
            p2->sprite[0] += 12;
            c2->sprite[0] += 12;
            anim1->xPosition += 12;
            anim2->xPosition = anim1->xPosition;
        }
        else
            proc->stat = 2;

        if (proc->stat == 2)
        {
            EndEfxAnimeDrvProc();
            sub_805AA28(&gUnknown_030053A0);
            r4 = proc->pid - 1;
            r6 = repo->infos[proc->menu_index].jid;
            sp58 = 0xffff;
            unit = GetUnitFromCharId(proc->pid);
            copied_unit = *unit;
            copied_unit.pClassData = GetClassData(repo->infos[proc->menu_index].jid);
            battle_anim_ptr = copied_unit.pClassData->pBattleAnimDef;

            ret = GetBattleAnimationId(
                &copied_unit,
                battle_anim_ptr,
                GetClassAnimWeapon(r6),
                &battle_anim_id);

            /* If no anim for equipped weapon, try item anim */
            if (ret == 0xFFFF)
                ret = GetBattleAnimationId(
                    &copied_unit,
                    battle_anim_ptr,
                    0,
                    &battle_anim_id);

            /* If we did not find the anim, return */
            if (ret == 0xFFFF)
            {
                proc->stat = 0;
                return;
            }

            for (i = 0; i <= 6; i++)
            {
                if (gAnimCharaPalConfig[(s16)r4][i] == (s16) r6) {
                    sp58 = gAnimCharaPalIt[(s16)r4][i] - 1;
                    break;
                }
            }
            sub_80CD47C((s16) ret, (s16) sp58, (s16) (p2->sprite[0] + 0x28), 0x58, 6);
            sub_805AE14(&gUnknown_0201FADC);
            sub_80CD408(proc->u50, p2->sprite[0], p2->msg_desc[1]);
        }
        else
            goto D1AC;
    }
    ++proc; --proc;
    b = proc->stat;
    tmp = &gUnknown_030053A0;
    if (b == 2) {
        if ((s16) p2->sprite[0] > 0x82) {
            u16 off = 12;
            p2->sprite[0] -= off;
            c2->sprite[0] -= off;
            anim1->xPosition -= off;
            anim2->xPosition = anim1->xPosition;
        } else {
            proc->stat = 0;
        }
    }
D1AC:
    if ((u8) sub_805A96C(tmp)) {
        sub_805A990(tmp);
    }
    LoadClassNameInClassReelFont(proc);
    return;
}
