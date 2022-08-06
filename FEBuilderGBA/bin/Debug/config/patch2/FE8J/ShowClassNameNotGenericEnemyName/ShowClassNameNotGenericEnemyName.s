@闘技場などで、対戦相手と戦う時は、戦闘画面の左上の名前を対戦相手ではなく、クラス名を表示する
@When fighting an opponent in the arena, etc., display the class name in the upper left corner of the battle screen instead of the opponent's name.
@
@ShowClassNameNotGenericEnemyName

.thumb
@FE8J 08052C70
@FE8U 08051F78


ldr r0, =0x0203E184 @BATTLE_ANIME	@{J}
@ldr r0, =0x0203E188 @BATTLE_ANIME	@{U}

ldr r1, [r0, #0x0]  @BATTLE_ANIME->CurrentUnit
ldr  r0, [r1, #0x0]  @CurrentUnit->Unit

ldrb r2, [r0, #0x4]  @CurrentUnit->Unit->ID
cmp  r2, #0xFD       @対戦相手(Enemy)
bne  ShowName

ShowClassName:
ldr  r0, [r1, #0x4]  @CurrentUnit->Class

ShowName:
ldrh r0, [r0, #0x0]  @->Name

ldr r3, =0x08052C78|1	@{J}
@ldr r3, =0x08051F80|1	@{U}
bx r3
