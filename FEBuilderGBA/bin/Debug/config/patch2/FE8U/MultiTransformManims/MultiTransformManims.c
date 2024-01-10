#include "global.h"

#include "bmio.h"
#include "bmbattle.h"
#include "mu.h"
#include "bmitem.h"

#include "mapanim.h"

#include "constants/classes.h"
#include "constants/items.h"

struct MultiTransformManimEnt {
    /* 00 */ u8 weaponId;
    /* 01 */ u8 sourceClassId;
    /* 02 */ u8 displayClassId;
    /* 04 */ const u16 * palette;
};

extern const struct MultiTransformManimEnt gMultiTransformManimList[];
// const struct MultiTransformManimEnt gMultiTransformManimList[] = {
//     {
//         .weaponId = ITEM_DIVINESTONE,
//         .sourceClassId = CLASS_MANAKETE_MYRRH,
//         .displayClassId = CLASS_MANAKETE,
//         .palette = gUnknown_089A8F74, // Myrrh's vanilla palette; TODO: Name in decomp
//     },
//     {
//         .weaponId = ITEM_SWORD_RAPIER,
//         .sourceClassId = CLASS_EIRIKA_LORD,
//         .displayClassId = CLASS_BAEL,
//         .palette = gPal_MapSpriteArena,
//     },
//     {
//         .weaponId = ITEM_SWORD_RAPIER,
//         .sourceClassId = CLASS_EIRIKA_MASTER_LORD,
//         .displayClassId = CLASS_ELDER_BAEL,
//         .palette = gPal_MapSpriteArena,
//     },
//     {
//         .weaponId = ITEM_LANCE_STEEL,
//         .sourceClassId = CLASS_NONE,
//         .displayClassId = CLASS_DRACO_ZOMBIE,
//         .palette = gPal_MapSpriteArena,
//     },
//     {
//         .weaponId = UINT8_MAX,
//     },
// };

void MTM_TrySetSpecialClassSprite(int actorNum) {
    const struct MultiTransformManimEnt * it = gMultiTransformManimList;

    u16 item = GetItemIndex(gManimSt.actor[actorNum].bu->weaponBefore);

    for (; it->weaponId != UINT8_MAX; it++) {
        if (it->weaponId != item) {
            continue;
        }

        if ((it->sourceClassId != CLASS_NONE) && (it->sourceClassId != UNIT_CLASS_ID(gManimSt.actor[actorNum].unit))) {
            continue;
        }

        MU_SetPaletteId(gManimSt.actor[actorNum].mu, BM_OBJPAL_BANIM_SPECIALMU + actorNum);
        MU_SetSpecialSprite(gManimSt.actor[actorNum].mu, it->displayClassId, it->palette);
        return;
    }

    return;
}