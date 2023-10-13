#include "global.h"

#include "TitleBgmByFlag.h"

const char gHackIdentifier[] = "EEB-BGM";

const struct TitleScreenBgmLutEnt gTitleScreenBgmFlagLut[] = {
    {
        .flag = 0xB4, // "Guide - Viewing Units"
        .songId = 6, // Grim Journey
    },
    {
        .flag = 0xE1, // "Guide - Save"
        .songId = 5, // Treasured Memories
    },
    {
        .flag = -1,
        .songId = 0,
    },
};

