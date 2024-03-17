#include "global.h"
#include "bmunit.h"
#include "bmitem.h"

#include "ClassChgExpansion.h"

s8 CheckFlag(int);

int GetClasschgListVanilla(struct Unit * unit, u16 item, u8 * out, int len)
{
    int i, amt = 0;
    u8 tmp_list[2];

    if (!UNIT_IS_VALID(unit))
        return 0;

    for (i = 0; i < 2; i++)
    {
        u8 jid_new = gPromoJidLut[UNIT_CLASS_ID(unit)][i];
        if (jid_new != 0)
            tmp_list[amt++] = jid_new;
    }

    if (out)
    {
        if (amt > len)
            amt = len;

        for (i = 0; i < amt; i++)
            out[i] = tmp_list[i];
    }

    return amt;
}

int GetClasschgListExpa(struct Unit * unit, u16 item, u8 * out, int len)
{
    int i, amt = 0;
    u8 jid, tmp_list[4];

    if (!UNIT_IS_VALID(unit))
        return 0;

    jid = UNIT_CLASS_ID(unit);

    for (i = 0; i < 4; i++)
    {
        u8 jid_new = gPromoJidLutExpa[jid][i];
        if (jid_new != 0)
            tmp_list[amt++] = jid_new;
    }

    if (out)
    {
        if (amt > len)
            amt = len;

        for (i = 0; i < amt; i++)
            out[i] = tmp_list[i];
    }

    return amt;
}

int GetClasschgListMod(struct Unit * unit, u16 item, u8 * out, int len)
{
    int i, amt = 0;
    u8 tmp_list[len];
    const struct ClassChgExpaMod * it;

    if (!UNIT_IS_VALID(unit))
        return 0;

    for (it = gpClassChgExpaMods; it->jid_promo != 0; it++)
    {
        if (amt >= len)
            break;

        if (UNIT_CLASS_ID(unit) == it->jid_old)
            if (it->pid == 0 || UNIT_CHAR_ID(unit) == it->pid)
                if (it->item == 0 || ITEM_INDEX(item) == ITEM_INDEX(it->item))
                    if (it->flag == 0 || CheckFlag(it->flag))
                    {
                        tmp_list[amt++] = it->jid_promo;
                    }
    }

    if (out)
    {
        if (amt > len)
            amt = len;

        for (i = 0; i < amt; i++)
            out[i] = tmp_list[i];
    }

    return amt;
}
