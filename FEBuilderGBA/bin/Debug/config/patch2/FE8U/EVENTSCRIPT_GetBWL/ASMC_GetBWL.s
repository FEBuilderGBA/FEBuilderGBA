@BWLを取得します
@Get BWL
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr ,r4 ,r5}

@ldr r4, =0x030004B0	@Slot0	{J}
ldr r4, =0x030004B8	@Slot0	{U}

ldr r0, [r4 , #0x4 * 1]	@Slot1

@blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}
cmp  r0,#0x00
beq  Exit            @取得できなかったら終了
mov  r5, r0

ldr  r0, [r5 , #0x0]	@RAMUnit->Unit
ldrb r0, [r0 , #0x4]	@RAMUnit->Unit->ID
@blh  0x080a9740   @BWL_GetEntry	@{J}
blh  0x080a4cfc   @BWL_GetEntry	@{U}

cmp  r0, #0x0
beq  Exit

ldr r1, [r4 , #0x4 * 2]	@Slot2
cmp r1, #0x1
beq GetWin
cmp r1, #0x2
beq GetLose
cmp r1, #0x3
beq GetDeadMapID
cmp r1, #0x4
beq IsDeadSkirmishes

@総戦闘回数を返す
GetBattle:
ldrh r2, [r0, #0xc]
lsl r2 ,r2 ,#0x12
lsr r0 ,r2 ,#0x14
b   Exit

@戦闘に勝利した数を返す
GetWin:
ldrb r3, [r0, #0xb]
ldrb r2, [r0, #0xc]
mov r1, #0x3
and r2 ,r1
lsl r2 ,r2 ,#0x8
orr r3 ,r2
mov r0 ,r3
b   Exit

@戦闘に敗北した数を返す
GetLose:
ldrb r0, [r0, #0x0]
b   Exit

@死亡したマップIDを取得
@もし、フリーマップで死亡していた場合は、NodeIDが代わりに返却されます
GetDeadMapID:
ldrb r1, [r0, #0x5]
lsl r1 ,r1 ,#0x1a
lsr r0 ,r1 ,#0x1a
b   Exit

@フリーマップで死亡しているか?
IsDeadSkirmishes:
ldrb r1, [r0, #0xe]
lsr r0 ,r1 ,#0x7
b   Exit

Exit:
str r0, [r4 , #0x4 * 0xC]	@SlotC

pop {r4,r5}
pop {r0}
bx r0
