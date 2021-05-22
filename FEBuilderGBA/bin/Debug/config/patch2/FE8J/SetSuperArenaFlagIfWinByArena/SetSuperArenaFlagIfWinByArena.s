.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@条件を満たして満たしていれば、SuperArenaフラグを設定する
push {lr ,r4 ,r5, r6}

mov r6 ,r0	@this procs

@壊すコードの再送
blh 0x08015394|1	@SubSkipThread2	@{J}
@blh 0x08015370|1	@SubSkipThread2	@{U}


ldr r4, =0x03004DF0 @Current	@{J}
@ldr r4, =0x03004E50 @Current	@{U}

ldr r4, [r4]		@RAMUnit = [Current]

@既にSuper Arenaだったら抜ける
CHECK_SUPER_ARENA:
ldr r0, [r4, #0xc]	@RAMUnit->Status
lsr r0 ,r0 ,#0x11
mov r1, #0x7
and r0 ,r1
cmp r0, #0x4
bhi Exit @既にSuper Arena


ldr r5, CONDTABLE

CHECKLV:
ldrb r2,  [r5, #0x0]	@Level
cmp  r2, #0x0
beq  CHECK_PROMOTED

ldrb r1, [r4 , #0x8]	@RAMUnit->LV
cmp  r1, r2
blt  Exit

CHECK_PROMOTED:
ldrb r2,  [r5, #0x1]	@Promoted
cmp  r2, #0x1
bne  CHECK_BWL_WIN

ldr r1, [r4, #0x0] @RAMUnit->Unit
ldr r0, [r4, #0x4] @RAMUnit->Class
ldr r2, [r1, #0x28]
ldr r0, [r0, #0x28]
orr r2 ,r0

mov r0, #0x80
lsl r0 ,r0 ,#0x1
and r2 ,r0
cmp r2 ,#0x0
beq Exit


CHECK_BWL_WIN:
ldrh r2,  [r5, #0x2]	@BWL
cmp  r2, #0x0
beq  CHECK_MONEY

ldr  r0, [r4 , #0x0]	@RAMUnit->Unit
ldrb r0, [r0 , #0x4]	@RAMUnit->Unit->ID
blh  0x080a9740   @BWL_GetEntry	@{J}
@blh  0x080a4cfc   @BWL_GetEntry	@{U}

cmp  r0, #0x0
beq  Exit

@Get BWL Win
ldrb r3, [r0, #0xb]
ldrb r2, [r0, #0xc]
mov r1, #0x3
and r2 ,r1
lsl r2 ,r2 ,#0x8
orr r3 ,r2

ldrh r2,  [r5, #0x2]	@BWL
cmp  r3, r2
blt  Exit


CHECK_MONEY:
ldr  r2,  [r5, #0x4]	@MONEY
cmp  r2, #0x0
beq  CHECK_FLAG

ldr  r0, =0x0202BCEC	@gChapterData	@{J}
@ldr  r0, =0x0202BCF0	@gChapterData	@{U}
ldr  r0, [r0, #0x8]		@gChapterData->Gold
cmp  r0, r2
blt  Exit


CHECK_FLAG:
ldrh r0,  [r5, #0x8]	@Flag
cmp  r0, #0x0
beq  Found

blh  0x080860d0	@CheckFlag	@{J}
@blh  0x08083DA8	@CheckFlag	@{U}
cmp  r0, #0x0
beq  Exit

Found:

@SuerArenaのフラグを設定します.
ldr  r0,  [r4, #0xc] @RAMUnit->Status
mov  r1 , #0xE
lsl  r1 , r1 ,#0x10
orr  r0 , r1

str  r0, [r4, #0xc] @RAMUnit->Status | SuperArena1 | SuperArena2

RunEvent:
ldr r0,  [r5, #0xC]	@Event
cmp r0,  #0x1
ble Exit
mov r1, r6  @this procs

blh 0x0800d340   @イベント命令を動作させる関数	{J}
@blh 0x0800d07c   @イベント命令を動作させる関数	{U}

Exit:
pop {r4,r5,r6}
pop {r0}
bx r0

.ltorg
CONDTABLE:
@LV			byte
@上級職		byte
@戦績		short

@ゴールド	word	4

@フラグ条件	sort	8
@イベント	word	C
