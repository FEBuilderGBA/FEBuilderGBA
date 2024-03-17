#pragma once

#include "global.h"
#include "bmunit.h"
#include "proc.h"

#define UNIT_PROMOT_LIST_MAX 6

typedef int (* GetClasschgListFunc_t)(struct Unit * unit, u16 item, u8 * out, int len);
extern const GetClasschgListFunc_t gGetClasschgListFuncs[];

/* List expansion */
extern const u8 gPromoJidLutExpa[0x100][0x4];

struct ClassChgExpaMod {
    u8 jid_promo;
    u8 jid_old;
    u8 pid;
    u8 _pad_;
    u16 item;
    u16 flag;
};
extern struct ClassChgExpaMod const * const gpClassChgExpaMods;

int GetClasschgList(struct Unit * unit, u16 item, u8 * out, int length);

/* Trainee related */

struct TraineeDataRe {
    u8 jid;
    u8 level;
};
extern struct TraineeDataRe const * const gpTraineesRe;

struct ClassChgSelInfo {
    u8 jid;
    bool use_weapon;
    s16 banim_idx;
    u16 desc;
};

struct ProcClassChgDataRepo {
    PROC_HEADER;

    struct Unit * unit;
    int amt;
    struct ClassChgSelInfo infos[UNIT_PROMOT_LIST_MAX];
};
struct ProcClassChgDataRepo * GetClassChgSelInfo(void);

/* Utils */
u16 GetClassAnimWeapon(u8 jid);
