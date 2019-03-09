#include "gbafe.h"

int CanUnitBeOnPosition(Unit* unit, int x, int y) {
	if (x < 0 || y < 0)
		return 0; // position out of bounds
	
	if (x >= gMapSize.x || y >= gMapSize.y)
		return 0; // position out of bounds
	
	if (gMapUnit[y][x])
		return 0; // a unit is occupying this position
	
	if (gMapHidden[y][x] & 1)
		return 0; // a hidden unit is occupying this position
	
	return CanUnitCrossTerrain(unit, gMapTerrain[y][x]);
}
