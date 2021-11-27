.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@r0 Unit
@r1 ItemID
FireAnime:
push {r4-r5,lr}
mov r4, r1	@itemid
mov r1, #0x1
neg r1, r1
blh 0x0802cb24   @SetupBattleStructForStaffUser	{U}
@blh 0x0802CA5C   @SetupBattleStructForStaffUser	{J}
ldr r5, =0x0203A4EC @BattleUnit@gBattleActor 戦闘アニメで右側.CopyUnit	{U}
@ldr r5, =0x0203A4E8 @BattleUnit@gBattleActor 戦闘アニメで右側.CopyUnit	{J}

mov r0 ,r5
add r0, #0x48
mov r1, r4   @ItemID  モーション用

strh r1, [r0, #0x0]   @BattleUnit@gBattleActor 戦闘アニメで右側.weapon
add r0, #0x2
strh r1, [r0, #0x0]   @BattleUnit@gBattleActor 戦闘アニメで右側.weaponBefore

ldr r0, =0x0203A608 @gpCurrentRound	{U}
@ldr r0, =0x0203A604 @gpCurrentRound	{J}
ldr r2, [r0, #0x0]

mov r0 ,r5
blh 0x0802d2c4   @BattleHitTerminate	{U}
@blh 0x0802D1FC   @BattleHitTerminate	{J}
blh 0x0802ca14   @BeginBattleAnimations	{U}
@blh 0x0802C94C   @BeginBattleAnimations	{J}
pop {r4-r5}
pop {r0}
bx r0

