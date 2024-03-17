#include "global.h"
#include "bmunit.h"
#include "bmitem.h"
#include "constants/items.h"

u16 GetClassAnimWeapon(u8 jid)
{
    int i, type = ITYPE_ITEM;
    int wexp = 0;
    const struct ClassData * class = GetClassData(jid);

    const u8 weapons[] = {
        [ITYPE_SWORD] = ITEM_SWORD_IRON,
        [ITYPE_LANCE] = ITEM_LANCE_IRON,
        [ITYPE_AXE]   = ITEM_AXE_IRON,
        [ITYPE_BOW]   = ITEM_BOW_IRON,
        [ITYPE_STAFF] = ITEM_STAFF_HEAL,
        [ITYPE_ANIMA] = ITEM_ANIMA_FIRE,
        [ITYPE_LIGHT] = ITEM_LIGHT_LIGHTNING,
        [ITYPE_DARK]  = ITEM_DARK_FLUX,
        [ITYPE_ITEM]  = ITEM_NONE,
    };

    if (class)
    {
        for (i = 0; i < 8; i++)
        {
            if (wexp < class->baseRanks[i])
            {
                wexp = class->baseRanks[i];
                type = i;
            }
        }
    }

    if (weapons[type] == ITEM_NONE)
        return 0;

    return MakeNewItem(weapons[type]);
}
