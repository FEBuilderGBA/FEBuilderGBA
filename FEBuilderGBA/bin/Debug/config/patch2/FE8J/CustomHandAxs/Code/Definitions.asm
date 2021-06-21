@ Definitions

@ Functions
.global New6C
.type   New6C, function
@.set    New6C, 0x08002C7D	@{U}
.set    New6C, 0x08002bcd	@{J}

.global Break6CLoop
.type   Break6CLoop, function
@.set    Break6CLoop, 0x08002E95	@{U}
.set    Break6CLoop, 0x08002de5	@{J}

.global Delete6C
.type   Delete6C, function
@.set    Delete6C, 0x08002D6D	@{U}
.set    Delete6C, 0x08002cbd	@{J}

.global StoreSpellTilesOBJ,
.type   StoreSpellTilesOBJ, function
@.set    StoreSpellTilesOBJ, 0x080557D9	@{U}
.set    StoreSpellTilesOBJ, 0x08056775	@{J}

.global StoreSpellTilesBG,
.type   StoreSpellTilesBG, function
@.set    StoreSpellTilesBG, 0x0805581D	@{U}
.set    StoreSpellTilesBG, 0x080567b9	@{J}

.global StoreSpellPaletteOBJ,
.type   StoreSpellPaletteOBJ, function
@.set    StoreSpellPaletteOBJ, 0x08055801	@{U}
.set    StoreSpellPaletteOBJ, 0x0805679d	@{J}

.global StoreSpellPaletteBG,
.type   StoreSpellPaletteBG, function
@.set    StoreSpellPaletteBG, 0x08055845	@{U}
.set    StoreSpellPaletteBG, 0x080567e1	@{J}

.global GetAISSubjectID
.type   GetAISSubjectID, function
@.set    GetAISSubjectID, 0x0805A16D	@{U}
.set    GetAISSubjectID, 0x0805af11	@{J}

.global getTargetAIS_void_int_a1
.type   getTargetAIS_void_int_a1, function
@.set    getTargetAIS_void_int_a1, 0x0805A2B5	@{U}
.set    getTargetAIS_void_int_a1, 0x0805b059	@{J}

.global MoveBattleCameraOnto
.type   MoveBattleCameraOnto, function
@.set    MoveBattleCameraOnto, 0x080533D1	@{U}
.set    MoveBattleCameraOnto, 0x080540c1	@{J}

.global ThisMakesTheHPInSpellAnimGoAway
.type   ThisMakesTheHPInSpellAnimGoAway, function
@.set    ThisMakesTheHPInSpellAnimGoAway, 0x08055279	@{U}
.set    ThisMakesTheHPInSpellAnimGoAway, 0x08056221	@{J}

.global StartEfxCriticalEffect
.type   StartEfxCriticalEffect, function
@.set    StartEfxCriticalEffect, 0x0806C71D	@{U}
.set    StartEfxCriticalEffect, 0x0806EA41	@{J}

.global SetSomethingSpellFxToFalse
.type   SetSomethingSpellFxToFalse, function
@.set    SetSomethingSpellFxToFalse, 0x0805516D	@{U}
.set    SetSomethingSpellFxToFalse, 0x08056115	@{J}

.global AIS_Free
.type   AIS_Free, function
@.set    AIS_Free, 0x08005005	@{U}
.set    AIS_Free, 0x08004f0d	@{J}

.global PrepAIS
.type   PrepAIS, function
@.set    PrepAIS, 0x08055555	@{U}
.set    PrepAIS, 0x080564f1	@{J}

.global SomeSFERoutine
.type   SomeSFERoutine, function
@.set    SomeSFERoutine, 0x080729A5	@{U}
.set    SomeSFERoutine, 0x08074e81	@{J}


@ RAM locations
.global palette_buffer
@.set    palette_buffer, 0x020228A8	@{U}
.set    palette_buffer, 0x020228A8	@{J}

.global gSomeSubAnim6CCounter
@.set    gSomeSubAnim6CCounter, 0x0201774C	@{U}
.set    gSomeSubAnim6CCounter, 0x0201774C	@{J}

.global AnimDistance
@.set    AnimDistance, 0x0203E120	@{U}
.set    AnimDistance, 0x0203E11C	@{J}

.global Procs_efxTeonoSE
@.set    Procs_efxTeonoSE, 0x085D50D8	@{U}
.set    Procs_efxTeonoSE, 0x085FF308	@{J}
