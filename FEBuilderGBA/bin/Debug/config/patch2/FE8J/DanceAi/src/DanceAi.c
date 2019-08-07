#include "gbafe.h"

static int DanceAiGetUnitDanceWeight(struct Unit* unit) {
	return unit->pow + unit->skl + unit->spd + unit->def + unit->res + unit->lck + unit->curHP;
}

void DanceAiTryDecide(int(*isUnitEnemy)(struct Unit*)) {
	if (!(UNIT_ATTRIBUTES(gActiveUnit) & (CA_DANCE | CA_PLAY)))
		return;

	AiFillMovementMapForUnit(gActiveUnit);

	int currentTargetWieght = -1;

	int targetIndex = -1;
	int xMovement   = -1;
	int yMovement   = -1;

	for (int iy = 0; iy < gMapSize.height; ++iy) {
		for (int ix = 0; ix < gMapSize.width; ++ix) {
			if (gMapMovement[iy][ix] > 120)
				continue; // Can't move there

			unsigned otherIndex = gMapUnit[iy][ix];

			if (!otherIndex)
				continue; // No unit there

			if (otherIndex == gActiveUnitId)
				continue; // We are there!

			struct Unit* unit = GetUnit(otherIndex);

			if (isUnitEnemy(unit))
				continue; // We don't want to dance for the enemy

			if (!(unit->state & US_UNSELECTABLE))
				continue; // We don't want to dance for someone that hasn't moved yet!

			if (UNIT_ATTRIBUTES(unit) & (CA_DANCE | CA_PLAY))
				continue; // not dancing for other dancers

			int weight = DanceAiGetUnitDanceWeight(unit);

			if (weight < currentTargetWieght)
				continue; // This unit is less interesting to dance for than the previous one

			struct Vector2 position;

			if (!GetAiSafestAccessibleAdjacentPosition(ix, iy, &position))
				continue; // Can't move close enough!!

			targetIndex = otherIndex;
			xMovement   = position.x;
			yMovement   = position.y;

			currentTargetWieght = weight;
		}
	}

	if (targetIndex >= 0) {
		SetAiActionParameters(
			xMovement, yMovement,
			AI_DECISION_DANCE,
			targetIndex, 0, 0, 0
		);
	}
}

int DanceAiDoAction(struct Proc* cpPerformProc) {
	gActiveUnit->xPos = gAiData.decision.xMovement;
	gActiveUnit->yPos = gAiData.decision.yMovement;

	gActionData.actionIndex = UNIT_ACTION_DANCE;
	gActionData.targetIndex = gAiData.decision.unitTargetIndex;

	// Do the dancing action
	ApplyUnitAction(cpPerformProc);

	// Finding last index in the ai unit queue
	unsigned lastIndex = 0;
	for (; gAiData.aiUnits[lastIndex]; ++lastIndex) {}

	// Queuing our refreshed unit
	gAiData.aiUnits[lastIndex]     = gAiData.decision.unitTargetIndex;
	gAiData.aiUnits[lastIndex + 1] = 0;

	return 1;
}
