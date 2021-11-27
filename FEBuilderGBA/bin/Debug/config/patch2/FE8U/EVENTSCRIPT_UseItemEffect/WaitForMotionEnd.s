
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


WaitForMotionEnd:
push {r4,r5,r6,lr}
mov r4 ,r0

@ 0x2Cで指定しているアニメーションProcsが終わるまで待ちます
@ arg r0 = target proc 
ldr r0, [r4, #0x2c]
blh 0x08002e9c	@Fin6C	{U}
@blh 0x08002DEC	@Fin6C	{J}
cmp r0 ,#0x0
bne Exit
	@ユニットが移動中のモーションが残ってしまうので消す
	blh 0x080790A4       @ClearMOVEUNITs	{U}
	@blh 0x0807B4B8       @ClearMOVEUNITs	{J}

	ldr  r6, =0x0203A4EC @BattleUnit@gBattleActor	{U}	戦闘アニメで右側.CopyUnit
	@ldr r6, =0x0203A4E8 @BattleUnit@gBattleActor	{J}	戦闘アニメで右側.CopyUnit
	ldrb r0, [r6, #0xb]
	blh 0x08019430       @GetUnitStruct	{U}
	@blh 0x08019108       @GetUnitStruct	{J}
	mov r5, r0	@Unit

	@回復やダメージの結果をRAMUnitに書き戻して確定させます
	mov r0, r5       @Arg1: Unit
	mov r1, r6       @Arg2: 戦闘アニメで右側.CopyUnit
	blh 0x0802c1ec   @SaveUnitFromBattle	{U}
	@blh 0x0802C134   @SaveUnitFromBattle	{J}

	@もしユニットのHPが0になってしまっているなら死亡させる
	mov r0, r5       @Arg1 Unit
	blh 0x08032750   @KillUnitIfNoHealth	{U}
	@blh 0x0803269C   @KillUnitIfNoHealth	{J}

	blh 0x080321C8   @UpdateMapAndUnit	{U}
	@blh 0x08032114   @UpdateMapAndUnit	{J}

	@マップアニメーションが終わったのでループを抜ける
	mov r0 ,r4
	blh 0x08002e94   @Break6CLoop	{U}
	@blh 0x08002DE4   @Break6CLoop	{J}
Exit:
pop {r4,r5,r6}
pop {r0}
bx r0
