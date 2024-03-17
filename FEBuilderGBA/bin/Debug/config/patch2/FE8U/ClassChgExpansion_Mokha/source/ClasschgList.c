#include "global.h"
#include "bmunit.h"
#include "constants/classes.h"

#include "ClassChgExpansion.h"

static int RearrangeJidBuffer(u8 * in, int len_in, u8 * out, int len_out)
{
    int i, amt = 0;
    u8 jid, tmp_list[0x100];

    CpuFastFill16(0, tmp_list, sizeof(tmp_list));

    /* Re-arrange jid list to remove the same node */
    for (i = 0; i < len_in; i++)
    {
        jid = in[i];

        if (jid != 0)
            tmp_list[jid] = true;
    }

    if (len_out > len_in)
        len_out = len_in;

    for (i = 1, amt = 0; i < 0x100; i++)
    {
        if (amt >= len_out)
            break;

        if (tmp_list[i] != 0)
        {
            if (out)
                out[amt] = i;

            amt = amt + 1;
        }
    }

    return amt;
}

int GetClasschgList(struct Unit * unit, u16 item, u8 * out, int len)
{
    int i, ret, tmp_len, amt = 0;
    const GetClasschgListFunc_t * it;
    u8 tmp_list[0x10];
    u8 * cur;

    if (!UNIT_IS_VALID(unit))
        return 0;

    tmp_len = sizeof(tmp_list);

    // CpuFill16(0, tmp_list, tmp_len);
    for (i = 0; i < tmp_len; i++)
        tmp_list[i] = 0;

    for (it = gGetClasschgListFuncs, cur = tmp_list; *it; it++)
    {
        ret = (*it)(unit, item, cur, tmp_len);

        if (ret > tmp_len)
            ret = tmp_len;

        amt += ret;
        cur += ret;

        tmp_len -= ret;
        if (tmp_len <= 0)
            break;
    }

    return RearrangeJidBuffer(tmp_list, sizeof(tmp_list), out, len);
}

int GetClasschgListPadFunc(struct Unit * unit, u16 item, u8 * out, int len)
{
    return 0;
}
