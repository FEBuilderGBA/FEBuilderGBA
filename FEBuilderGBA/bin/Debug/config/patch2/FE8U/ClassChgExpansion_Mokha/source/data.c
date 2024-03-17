#include "global.h"
#include "constants/characters.h"
#include "constants/classes.h"

#include "ClassChgExpansion.h"

const u8 gPromoJidLutExpa[0x100][0x4] = {
    [CLASS_JOURNEYMAN] = {
        CLASS_MAGE,
        CLASS_SHAMAN,
        CLASS_MONK,
    },

    [CLASS_RECRUIT] = {
        CLASS_CAVALIER_F,
        CLASS_ARMOR_KNIGHT_F,
        CLASS_PEGASUS_KNIGHT
    },

    [CLASS_PUPIL] = {
        CLASS_FIGHTER,
        CLASS_BRIGAND
    },

    [CLASS_MAGE] = {
        CLASS_DRUID,
    },

    [CLASS_MAGE_F] = {
        CLASS_VALKYRIE,
        CLASS_BISHOP_F,
        CLASS_DRUID_F,
        CLASS_SUMMONER_F
    },

    [CLASS_EIRIKA_LORD] = {
        CLASS_SWORDMASTER_F
    },
};

const char PromoJidLutExpaMagic[] = "PromoJidLutExpa"; /* for FEBuilder patch */

static const struct ClassChgExpaMod gClassChgExpaMods[] = {
    {
        .jid_promo = CLASS_RECRUIT_T1,
        .jid_old = CLASS_RECRUIT,
        .pid = CHARACTER_AMELIA,
        .item = 0,
        .flag = 0,
    },
    {
        .jid_promo = CLASS_JOURNEYMAN_T1,
        .jid_old = CLASS_JOURNEYMAN,
        .pid = CHARACTER_ROSS,
        .item = 0,
        .flag = 0,
    },
    {
        .jid_promo = CLASS_PUPIL_T1,
        .jid_old = CLASS_PUPIL,
        .pid = CHARACTER_EWAN,
        .item = 0,
        .flag = 0,
    },
    {0}
};

struct ClassChgExpaMod const * const gpClassChgExpaMods = gClassChgExpaMods;
const char ClassChgExpaModsMagic[] = "ClassChgExpaMod"; /* for FEBuilder patch */

static const struct TraineeDataRe gTraineesRe[] = {
    { CLASS_JOURNEYMAN, 10 },
    { CLASS_RECRUIT, 10 },
    { CLASS_PUPIL, 10 },
    {0}
};

struct TraineeDataRe const * const gpTraineesRe = gTraineesRe;
const char TraineesReMagic[] = "TraineesRee"; /* for FEBuilder patch */
