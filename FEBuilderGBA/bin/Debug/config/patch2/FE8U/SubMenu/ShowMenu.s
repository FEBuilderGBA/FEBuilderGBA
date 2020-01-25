.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4,lr}
mov r4, r0
@blh 0x0804F610   @ClearBG0BG1	{J}
blh 0x0804E884   @ClearBG0BG1	{U}
@ldr r0, =0x02023CA8 @(BG 2 wram buffer )	{J}
ldr r0, =0x02023CA8 @(BG 2 wram buffer )	{U}
mov r1, #0x0
@blh 0x080011d0   @BG_Fill	{J}
blh 0x08001220   @BG_Fill	{U}
mov r0, #0x4
@blh 0x08001efc   @BG_EnableSyncByMask	{J}
@blh 0x08003c50   @Font_ResetAllocation	{J}
blh 0x08001FAC   @BG_EnableSyncByMask	{U}
blh 0x08003D20   @Font_ResetAllocation	{U}
mov r0, r4
@ldr r2, =0x0202BCAC @(BattleMapState@gGameState.boolMainLoopEnded )	{J}
ldr r2, =0x0202BCB0 @(BattleMapState@gGameState.boolMainLoopEnded )	{U}
mov r3, #0x1c
ldsh r1, [r2, r3] @(BattleMapState@gGameState._unk1C )
mov r3, #0xc
ldsh r2, [r2, r3] @(BattleMapState@gGameState.cameraRealPos )
sub r1 ,r1, r2
mov r2, #0x1
mov r3, #0x16
@blh 0x080503c0,r4  @NewMenu_AndDoSomethingCommands	{J}
@blh 0x0801d730,r4  @HideMoveRangeGraphics	{J}
blh 0x0804F64C,r4  @NewMenu_AndDoSomethingCommands	{U}
blh 0x0801DACC,r4  @HideMoveRangeGraphics	{U}
pop {r4}
pop {r1}
bx r1

.ltorg
.align
