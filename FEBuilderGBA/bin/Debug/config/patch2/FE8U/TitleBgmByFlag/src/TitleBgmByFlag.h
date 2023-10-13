#ifndef TITLE_BGM_BY_FLAG_H
#define TITLE_BGM_BY_FLAG_H

struct TitleScreenBgmLutEnt {
    int flag;
    int songId;
};

extern const struct TitleScreenBgmLutEnt gTitleScreenBgmFlagLut[];
extern const char gHackIdentifier[];

#endif // TITLE_BGM_BY_FLAG_H
