@HOOK FE8J	15FDC	{J}
@HOOK FE8U	15FD0	{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


cmp r0,#0x0
beq FindBGMObject

@フラグ0x04 がONなので何もしない
ExitFlag0x04ON:
mov r7,#0x6
mov r6,#0x7
mov r4,#0x6
b   ExitContinueVanilla

FindBGMObject:
@ldr r0, =0x0203A610	@TrapData	@{J}
ldr r0, =0x0203A614	@TrapData	@{U}
ldr r3,=0x40*8
add r3,r0				@Term

Loop:
ldrb r1,[r0,#0x2] @Trap->Type
cmp  r1,#0xEE			@TrapType 0xEE
beq  Found
add  r0, #0x8

cmp  r0,r3
blt  Loop

@見つからないので、通常のBGMを再生する
ExitNotFound:
mov r7,#0x0
mov r6,#0x1
mov r4,#0x2

ExitContinueVanilla:
@ldr r3,=0x0801600E|1	@{J}
ldr r3,=0x08016002|1	@{U}
bx  r3

Found:
@ldr r5, =0x0202BCEC	@ChapterData	@{J}
ldr r5, =0x0202BCF0	@ChapterData	@{U}
ldrb r1, [r5, #0xf] @ ChapterData@ステージの領域.フェイズ 0=自軍,0x40=友軍,0x80=敵軍
cmp  r1, #0x80
bne  CheckNPC

PlayEnemyBGM:
ldrh r0, [r0,#0x6]	@CurrentTrap->0x6EnenmyBGM
b    ExitWithFoundBGM

CheckNPC:
cmp  r1, #0x40
bne  PlayerTurn

@
PlayNPCBGM:
ldrh r0, [r0,#0x0]	@CurrentTrap->0x0NPCBGM
b    ExitWithFoundBGM

@Player BGMと同じものを再生します
PlayPlayerBGM:
ldrh r0, [r0,#0x4]	@CurrentTrap->0x4PlayerBGM
ExitWithFoundBGM:
cmp  r0, #0x0
beq  ExitNotFound   @見つけたBGM IDだが0が設定されている
                    @これでは再生できないので、通常のBGMを再生します

@ldr  r3,=0x80160D0|1	@{J}
ldr  r3,=0x80160c4|1	@{U}
bx   r3

PlayerTurn:
ldrb  r1, [r0, #0x3]
cmp   r1, #0x1
beq   PlayPlayerBGM  @状況有利のBGMを再生しないのであれば、これで終わり

CheckVictoryBGM:
mov  r4, r0 @blコールがあるので、みつけたTrapObjectを壊されないように保存しておく.
ldrb r0, [r5, #0xE]	@ChapterData@ステージの領域.マップID
@blh  0x08034520		@GetROMChapterStruct	@{J}
blh  0x08034618		@GetROMChapterStruct	@{U}

add  r0, #0x86
ldrb r0, [r0]		@勝利BGM再生する敵兵の数の設定を取得
mov  r6, r0
cmp  r6, #0x1
bge  CheckEnenmyCount
mov  r6, #0x1      @残念ながら勝利BGMを再生する条件は、最低でも1です。

CheckEnenmyCount:
ldr r1, =0x0001000C
mov r0, #0x80
@blh 0x08024d00   @Count_Faction_With_Criteria	{J}
blh 0x08024d50   @Count_Faction_With_Criteria	{U}
cmp  r0, r6
ble  PlayVictoryBGM

mov  r0, r4
b    PlayPlayerBGM

PlayVictoryBGM:
@ldr  r3,=0x080160B8|1	@{J}
ldr  r3,=0x080160AC|1	@{U}
bx   r3
