#include "FE-CLib\include\gbafe.h"

s8 AreAllegiancesAllied(int factionA, int factionB) {
	factionA &= 0xFF; factionB &= 0xFF;
	if (factionA  == factionB) return TRUE; // if same units, then they're obviously allied
	factionA = (factionA >> 6);
	factionB =  (factionB >> 6);
	if ((GetChapterDefinition(gChapterData.chapterIndex)->factionRelations) >> (factionA * 4 + factionB) & 1) return TRUE; else return FALSE;
}

void SwitchGameplayPhase() {
	switch(gChapterData.currentPhase) {
		case FACTION_BLUE:
		gChapterData.currentPhase = FACTION_RED;
		break;
		case FACTION_RED:
		gChapterData.currentPhase = FACTION_GREEN;
		break;
		case FACTION_GREEN:
		gChapterData.currentPhase = FACTION_PURPLE;
		break;
		case FACTION_PURPLE:
		gChapterData.currentPhase = FACTION_BLUE;
		if(gChapterData.turnNumber < 999) gChapterData.turnNumber++;
		ProcessSupportGains();
		break;
	}
}

int GetCurrentMapMusicIndex() {
	int PlayerPhaseMusicIndex, EnemyPhaseMusicIndex, OtherPhaseMusicIndex, NeutralPhaseMusicIndex; 
	if(CheckEventId(4)) PlayerPhaseMusicIndex = 6; else PlayerPhaseMusicIndex = 0;
	if(CheckEventId(4)) EnemyPhaseMusicIndex = 7; else EnemyPhaseMusicIndex = 1;
	if(CheckEventId(4)) OtherPhaseMusicIndex = 5; else OtherPhaseMusicIndex = 2;
	if(CheckEventId(4)) NeutralPhaseMusicIndex = 4; else NeutralPhaseMusicIndex = 3;
	switch(gChapterData.currentPhase) {
		case FACTION_RED:
			return GetChapterDefinition(gChapterData.chapterIndex)->mapSongIndices[EnemyPhaseMusicIndex];
		case FACTION_GREEN:
			return GetChapterDefinition(gChapterData.chapterIndex)->mapSongIndices[OtherPhaseMusicIndex];
		case FACTION_PURPLE:
			return GetChapterDefinition(gChapterData.chapterIndex)->mapSongIndices[NeutralPhaseMusicIndex];
		case FACTION_BLUE: 
			if(CheckEventId(4)) // Victory Music doesn't play if flag 4 is on?
				return GetChapterDefinition(gChapterData.chapterIndex)->mapSongIndices[PlayerPhaseMusicIndex];
			else { // Should normal player phase music be played or victory music?
				if(GetBattleMapType() == 2 || GetChapterDefinition(gChapterData.chapterIndex)->victorySongEnemyThreshold != 0) {
					if (CountUnitInFactionWithoutAttributes(FACTION_RED, US_UNAVAILABLE) < GetChapterDefinition(gChapterData.chapterIndex)->victorySongEnemyThreshold || CountUnitInFactionWithoutAttributes(FACTION_RED, US_UNAVAILABLE) == 0) return 0x10;
					
				}
				return GetChapterDefinition(gChapterData.chapterIndex)->mapSongIndices[PlayerPhaseMusicIndex];
			}
	}
}