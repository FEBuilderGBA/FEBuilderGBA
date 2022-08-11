.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.macro blh_ to, reg=r3
  push {\reg}
  ldr \reg, =\to
  mov lr, \reg
  pop {\reg}
  .short 0xf800
.endm

@Orignal 080B83A8	@{J}
push {r4,r5,r6,r7,lr}
mov r7, r10
mov r6, r9
mov r5, r8
push {r5,r6,r7}
sub sp, #0x4
mov r9, r0		@this procs
                @r9は、this procsを指すグローバル変数として利用する

@表示するかどうか
bl  GetIsShowData
mov r10, r0


@@アニメーションデータ
bl GetAnimationData	@ArenaDicStruct->Animation
mov r3, r9
@ldr r3, [r3, #0x34] @ ArenaDicStruct
@ldrh r0, [r3, #0x18] @ ArenaDicStruct->Info
str r0, [r3, #0x38]	@AnimationData


@@背景の設定
mov r0, #0x0
blh 0x08001acc   @SetupBackgrounds

mov r0, #0x0
mov r4, r9
strh r0, [r4, #0x2a]
strh r0, [r4, #0x2c]

@ステータスの表示を止めさせる
str r0, [r4, #0x40]
str r0, [r4, #0x44]

ldr r6, =0x02022CA8 @BG 0 wram buffer
mov r0 ,r6
mov r1, #0x0
blh 0x080011d0   @BG_Fill
ldr r0, =0x020234A8 @BG 1 wram buffer
mov r1, #0x0
blh 0x080011d0   @BG_Fill
ldr r0, =0x02023CA8 @BG 2 wram buffer
mov r1, #0x0
blh 0x080011d0   @BG_Fill
ldr r4, =0x03003020 @IORegisters@IORegisters.To 04000000 (DISPCNT / LCD Control) )
ldrb r1, [r4, #0x1]
mov r0, #0x2
neg r0 ,r0
and r0 ,r1
mov r1, #0x3
neg r1 ,r1
and r0 ,r1
sub r1, #0x2
and r0 ,r1
sub r1, #0x4
and r0 ,r1
sub r1, #0x8
and r0 ,r1
strb r0, [r4, #0x1]
ldrb r1, [r4, #0x0] @03003020 (IORegisters@IORegisters.To 04000000 (DISPCNT / LCD Control) )
mov r0, #0x8
neg r0 ,r0
and r0 ,r1
strb r0, [r4, #0x0]   @IORegisters@IORegisters.To 04000000 (DISPCNT / LCD Control)
blh 0x08001ed0   @SetDefaultColorEffects
blh 0x08003c50   @Font_ResetAllocation
blh 0x08003bc4   @Font_InitForUIDefault
ldrb r2, [r4, #0xc] @ pointer:0300302C (IORegisters@IORegisters.To 04000008 (BG0CNT / BG0 Control) )
mov r1, #0x4
neg r1 ,r1
mov r0 ,r1
and r0 ,r2
mov r3, #0x2
orr r0 ,r3
strb r0, [r4, #0xc]   @IORegisters@IORegisters.To 04000008 (BG0CNT / BG0 Control)
ldrb r2, [r4, #0x10] @ pointer:03003030 (IORegisters@IORegisters.To 0400000A (BG1CNT / BG1 Control) )
mov r0 ,r1
and r0 ,r2
orr r0 ,r3
strb r0, [r4, #0x10]   @IORegisters@IORegisters.To 0400000A (BG1CNT / BG1 Control)
ldrb r0, [r4, #0x14] @ pointer:03003034 (IORegisters@IORegisters.To 0400000C (BG2CNT / BG2 Control) )
and r1 ,r0
orr r1 ,r3
strb r1, [r4, #0x14]   @IORegisters@IORegisters.To 0400000C (BG2CNT / BG2 Control)
ldrb r0, [r4, #0x18] @ pointer:03003038 (IORegisters@IORegisters.To 0400000E (BG3CNT / BG3 Control) )
mov r1, #0x3
orr r0 ,r1
strb r0, [r4, #0x18]   @IORegisters@IORegisters.To 0400000E (BG3CNT / BG3 Control)
mov r0, #0x0
mov r1, #0x0
mov r2, #0x0
blh 0x08001448   @BG_SetPosition
mov r0, #0x1
mov r1, #0x0
mov r2, #0x0
blh 0x08001448   @BG_SetPosition
mov r0, #0x2
mov r1, #0x0
mov r2, #0x0
blh 0x08001448   @BG_SetPosition
mov r0, #0x3
mov r1, #0x0
mov r2, #0x0
blh 0x08001448   @BG_SetPosition
ldr r4, =0x08AB1C58 @特殊表示用の背景1@ZIMAGE_POINTER
mov r0, #0x3
blh 0x08000f3c   @GetBackgroundTileDataOffset
mov r1 ,r0
mov r5, #0xc0
lsl r5 ,r5 ,#0x13
add r1 ,r1, r5
mov r0 ,r4
blh 0x08013008   @UnLZ77Decompress
ldr r0, =0x08AB6768 @特殊表示用の背景1@PALETTE_POINTER
mov r2, #0x80
lsl r2 ,r2 ,#0x1
mov r1, #0xe0
blh 0x08000d68   @CopyToPaletteBuffer
ldr r0, =0x020244A8 @BG 3 wram buffer
ldr r1, =0x08AB62B4 @特殊表示用の背景1@HEADERTSA_POINTER
mov r2, #0xe0
lsl r2 ,r2 ,#0x7
blh 0x080dc0dc   @CallARM_FillTileRect
ldr r4, =0x08AB162C @CC画面の下のテキストボックス@ZIMAGE_POINTER
mov r0, #0x2
blh 0x08000f3c   @GetBackgroundTileDataOffset
mov r1 ,r0
add r1 ,r1, r5
mov r0 ,r4
blh 0x08013008   @UnLZ77Decompress
ldr r0, =0x085E0D94 @何かのパレット 137
mov r1, #0xc0
mov r2, #0x20
blh 0x08000d68   @CopyToPaletteBuffer
ldr r1, =0x08AB17A4 @FillTileRectされる何かのタイル 26
mov r2, #0xc0
lsl r2 ,r2 ,#0x7
ldr r0, =0x02023CA8 @BG 2 wram buffer
blh 0x080dc0dc   @CallARM_FillTileRect
mov r0, #0xf
blh 0x08001efc   @BG_EnableSyncByMask
mov r0 ,r6
mov r1, #0x0
blh 0x080011d0   @BG_Fill


ShowText3Start:
mov r4, #0x4a
add r4 ,r4, r6
add r6, #0x42
mov r4, #0x0

@テキスト1
mov r0,#0x0
bl ShowText3
add r6, #0x80
add r4, #0x8

@テキスト2
mov r0,#0x1
bl ShowText3
add r6, #0x80
add r4, #0x8

@テキスト3
mov r0,#0x2
bl ShowText3
add r6, #0x80
add r4, #0x8


@空撃ち
add r6, #0x80
add r4, #0x8

@達成率
bl ShowAchievementRate
add r6, #0x80
add r4, #0x8

@ページ数
bl ShowPageCount
add r6, #0x80
add r4, #0x8


mov r5, #0x0
mov r0, r9
blh 0x080b8c64   @Startopinfogaugedraw	上の英語フォント
mov r3, r9
str r0, [r3, #0x3c]
mov r0, #0x80
lsl r0 ,r0 ,#0x1
mov r1, #0x2
mov r2, #0x0
blh_ 0x08006710   @Dialogue_InitGfx
blh 0x0800687c   @Dialogue_InitFont
blh 0x0800814c   @Dialogue_ClearLines
blh 0x08006980   @EndDialogue


bl  GetTwoLineMessageInfo
mov r2, r0

mov r4, r9
@ldr r0, [r4, #0x34] @ ArenaDicStruct
@ldrh r2, [r0, #0x4] @ ArenaDicStruct->INFO 2行メッセージ

mov r0, #0x2
mov r1, #0xf
blh 0x08006934   @StartDialogueFromIndex
mov r0, #0x0
blh 0x08006a14   @Dialogue_SetDefaultTextColor
mov r0, #0x1
blh 0x080069ac   @SetDialogueFlag
mov r0, #0x2
blh 0x080069ac   @SetDialogueFlag
mov r0, #0x4
blh 0x080069ac   @SetDialogueFlag
mov r0, #0x8
blh 0x080069ac   @SetDialogueFlag
mov r0, #0x40
blh 0x080069ac   @SetDialogueFlag
mov r0, #0x4
blh 0x080069f4   @Dialogue_SetCharacterDisplayDelay

bl  GetUnitPalette
mov r1, r0  @ArenaDicStruct->Palette
sub r1, #0x1 @パレット-1の値を登録する必要があります

ldr r0, =0x02000000 @WRAM
ldr r3, [r4, #0x34]	@ArenaDicStruct
@mov r1, #0x12
@ldsb r1, [r3, r1]   @ArenaDicStruct->Palette
strh r1, [r0, #0x8]   @gAISFrontRight

mov r1, #0x82
lsl r1 ,r1 ,#0x1
strh r1, [r0, #0x2]
mov r1, #0x58
strh r1, [r0, #0x4]   @gAISBackLeft
ldrh r1, [r3, #0x0E]  @ArenaDicStruct->BattleAnimeID
sub r1, #0x01
strh r1, [r0, #0x6]
mov r1, #0x6
strh r1, [r0, #0xa]
ldrb r1, [r3, #0x11]  @ArenaDicStruct->敵味方色
strb r1, [r0, #0x1]
mov r4, #0x1
strh r4, [r0, #0xc]   @gAISBackRight
mov r1, #0xc0
lsl r1 ,r1 ,#0x1
strh r1, [r0, #0xe]
mov r1, #0x2
strh r1, [r0, #0x10]
ldr r1, =0x02000038
str r1, [r0, #0x1c]
ldr r1, =0x02002038
str r1, [r0, #0x24]
ldr r1, =0x02007838
str r1, [r0, #0x20]
ldr r1, =0x020078D8
str r1, [r0, #0x28]
ldr r1, =0x0200A2D8
str r1, [r0, #0x30]
ldrb r2, [r3, #0xD]   @ArenaDicStruct->攻撃魔法エフェクト
strh r2, [r1, #0x0]
mov r2, #0x0          @謎の4バイト 0固定
strh r2, [r1, #0x2]
strh r2, [r1, #0x4]
strh r2, [r1, #0x6]
strh r2, [r1, #0x8]
mov r2, #0xa0
lsl r2 ,r2 ,#0x2
strh r2, [r1, #0xe]
mov r3, #0xf
strh r3, [r1, #0x10]
sub r2, #0x80
strh r2, [r1, #0xa]
strh r3, [r1, #0xc]
strh r4, [r1, #0x12]
ldr r2, =0x020234A8 @BG 1 wram buffer
str r2, [r1, #0x14]
ldr r2, =0x0200A300
str r2, [r1, #0x18]
ldr r2, =0x0200C300
str r2, [r1, #0x1c]
ldr r2, =0x0200CB00
str r2, [r1, #0x20]
ldr r2, =0x080B82ED
str r2, [r1, #0x24]
blh 0x0805b7a4   @StartEkrUnitMainMini
ldr r4, =0x0201DB00
mov r0, r9
ldr r1, [r0, #0x34]   @ArenaDicStruct
ldrb r0, [r1, #0x12]  @ArenaDicStruct->床左 地形
add  r0, #0x01
strh r0, [r4, #0x0]
mov r0, #0xa
strh r0, [r4, #0x2]
mov r0, #0xe0
lsl r0 ,r0 ,#0x2
strh r0, [r4, #0x4]
ldrb r0, [r1, #0x12]  @ArenaDicStruct->床右 地形
add  r0, #0x01
strh r0, [r4, #0x6]
mov r0, #0xb
strh r0, [r4, #0x8]
mov r0, #0xf0
lsl r0 ,r0 ,#0x2
strh r0, [r4, #0xa]
strh r5, [r4, #0xc]
ldr r0, =0x0000FFFF
strh r0, [r4, #0xe]
ldr r0, =0x06010000 @OBJ_VRAM0_TEXT
str r0, [r4, #0x1c]
ldr r0, =0x0201DB28
str r0, [r4, #0x20]
mov r0 ,r4
blh 0x0805b80c   @StartAnime_EkrUnitMain
mov r3, #0x98
lsl r3 ,r3 ,#0x1
mov r0, #0x68
str r0,[sp, #0x0]
mov r0 ,r4
mov r1, #0xd0
mov r2, #0x68
blh_ 0x0805bbe4
ldr r0, =0x080B828D
blh 0x08001d28   @SetPrimaryHBlankHandler
add sp, #0x4
pop {r3,r4,r5}
mov r8, r3
mov r9, r4
mov r10, r5
pop {r4,r5,r6,r7}
pop {r0}
bx r0
.ltorg

ShowText3:
@r9 this
@r0 index
@r4 ?
@r6 ?
push {r5,r7,lr}
mov r7, r0
ldr r0, =0x0201FB28
add r5 ,r4, r0
mov r0 ,r5
mov r1, #0x8
blh 0x08003c8c   @Text_Init
mov r0 ,r5
blh 0x08003cf8   @TextVRAMClearer This function gets a space in VRAM and prepares it for the new text to be written to.


mov r0 ,r5
mov r1 ,#0x0     @r1はテキストの色
blh 0x08003d90   @Text_SetColorId
mov r0 ,r5
mov r1, #0x0
blh 0x08003d84   @Text_SetXCursor

mov r3, r9
ldr r3, [r3, #0x34] @ ArenaDicStruct
mov r0, r7
lsl r0, #0x1 @ *2
add r0, #0x6
ldrh r0, [r3, r0]  @ ArenaDicStruct->TextID[r7]
cmp r0,#0x0
beq ShowText3_Exit
blh 0x08009fa8   @GetStringFromIndex

mov r1 ,r0
mov r0 ,r5
blh 0x08003f28   @Text_AppendString

ldr r0, =0x0201FB28
add r0 ,r4, r0
mov r1, r6
blh 0x08003da0   @Text_Draw

ShowText3_Exit:
pop {r5,r7}
pop {r0}
bx r0


ShowAchievementRate:
@r9 this
@r4 ?
@r6 ?
push {r5,lr}
ldr r0, =0x0201FB28
add r5 ,r4, r0
mov r0 ,r5
mov r1, #0xC
blh 0x08003c8c   @Text_Init
mov r0 ,r5
blh 0x08003cf8   @TextVRAMClearer This function gets a space in VRAM and prepares it for the new text to be written to.

mov r0 ,r5
mov r1 ,#0x0
blh 0x08003d90   @Text_SetColorId

ldr r0, =0x535   @達成率
blh 0x08009fa8   @GetStringFromIndex

mov r1 ,r0
mov r0 ,r5
blh 0x08003f28   @Text_AppendString

ldr r0, =0x0201FB28
add r0 ,r4, r0
mov r1, r6
blh 0x08003da0   @Text_Draw


@パーセントを出す
mov r0 ,r5
mov r1, #0x39
blh 0x08003d84   @Text_SetXCursor

ldr r0, =0x748   @%
blh 0x08009fa8   @GetStringFromIndex

mov r1 ,r0
mov r0 ,r5
blh 0x08003f28   @Text_AppendString

ldr r0, =0x0201FB28
add r0 ,r4, r0
mov r1, r6
blh 0x08003da0   @Text_Draw


@comp*100/all=
mov r3, r9
ldr r3, [r3, #0x30] @ 親Procs ArenaDic
ldrh r0, [r3, #0x2E] @ArenaDic->ComplateCount
mov  r1, #100
mul  r0, r1

ldrh r1, [r3, #0x2C] @ArenaDic->AllCount

swi 6  @div
mov r2, r0

mov r1, #0x0  @TextColor
cmp r2, #100
blt ShowAchievementRate_Draw
    mov r1, #0x4  @100%だったら緑にする

ShowAchievementRate_Draw:
@r2 達成率の数字
@r1 数字の色

mov r0 ,r6
add r0, #0xc
blh 0x08004a90   @DrawUiNumber

pop {r5}
pop {r0}
bx r0
.ltorg


ShowPageCount:
@r9 this
@r4 ?
@r6 ?
push {r5,lr}
ldr r0, =0x0201FB28
add r5 ,r4, r0
mov r0 ,r5
mov r1, #0xC
blh 0x08003c8c   @Text_Init
mov r0 ,r5
blh 0x08003cf8   @TextVRAMClearer This function gets a space in VRAM and prepares it for the new text to be written to.

@@スラッシュの表示
mov r0 ,r5
mov r1 ,#0x3     @黄色
blh 0x08003d90   @Text_SetColorId
mov r0 ,r5
mov r1, #0x1A
blh 0x08003d84   @Text_SetXCursor

ldr r0, =0x4C9   @スラッシュ
blh 0x08009fa8   @GetStringFromIndex

mov r1 ,r0
mov r0 ,r5
blh 0x08003f28   @Text_AppendString

ldr r0, =0x0201FB28
add r0 ,r4, r0
mov r1, r6
blh 0x08003da0   @Text_Draw



@r2 現在のページの数字
@r1 数字の色
mov r3, r9
ldr r3, [r3, #0x30] @ 親Procs ArenaDic
ldrh r2, [r3, #0x2A] @ArenaDic->CurrentPage
add r2, #0x01 @ページ0は見た目が悪いので、+1して、1スタートにする

mov r1, #0x0 @色

mov r0, r6
add r0, #0x4
blh 0x08004a90   @DrawUiNumber


@r2 全ページの数字
@r1 数字の色
mov r3, r9
ldr r3, [r3, #0x30] @ 親Procs ArenaDic
ldrh r2, [r3, #0x2C] @ArenaDic->AllCount

mov r1, #0x3 @色

mov r0, r6
add r0, #0xE
blh 0x08004a90   @DrawUiNumber

pop {r5}
pop {r0}
bx r0
.ltorg

GetIsShowData:
@r9 this
push {lr}
mov r3, r9
ldr r0, [r3, #0x30] @ ParentProcs
mov r1, #0x29
ldrb r0, [r0, r1]	@thisProcs->isShowData
pop {r1}
bx r1
.ltorg


GetAnimationData:
@r9 this
@r10 show bool
push {lr}

mov r3, r10
cmp r3,#0x0
bne GetAnimationData_MoveData

@非表示
ldr  r3, ArenaDicConfig
ldr  r0, [r3, #0x04]	@ ArenaDicConfig->未撃破静止アニメーション
b    GetAnimationData_Exit

GetAnimationData_MoveData:
mov r3, r9
ldr r3, [r3, #0x34] @ ArenaDicStruct
ldrb r0, [r3, #0x13] @ ArenaDicStruct->Anime
cmp r0, #0x1
beq GetAnimationData_Anime2
cmp r0, #0x2
beq GetAnimationData_Anime3

GetAnimationData_Anime1:
ldr  r3, ArenaDicConfig
ldr  r0, [r3, #0x10]	@ ArenaDicConfig->アニメーション1
b    GetAnimationData_Exit

GetAnimationData_Anime2:
ldr  r3, ArenaDicConfig
ldr  r0, [r3, #0x14]	@ ArenaDicConfig->アニメーション2
b    GetAnimationData_Exit

GetAnimationData_Anime3:
ldr  r3, ArenaDicConfig
ldr  r0, [r3, #0x18]	@ ArenaDicConfig->アニメーション3
@b    GetAnimationData_Exit

GetAnimationData_Exit:
pop {r1}
bx r1


GetTwoLineMessageInfo:
@r9 this
@r10 show bool
push {lr}

mov r3, r10
cmp r3,#0x0
bne GetTwoLineMessageInfo_ShowText

@非表示
ldr  r3, ArenaDicConfig
ldrh r0, [r3, #0x02]	@ ArenaDicConfig->未撃破メッセージ
b    GetTwoLineMessageInfo_Exit

GetTwoLineMessageInfo_ShowText:
mov r3, r9
ldr r3, [r3, #0x34] @ ArenaDicStruct
ldrh r0, [r3, #0x04] @ ArenaDicStruct->Info

GetTwoLineMessageInfo_Exit:
pop {r1}
bx r1


GetUnitPalette:
@r9 this
@r10 show bool
push {lr}

mov r3, r10
cmp r3,#0x0
bne GetUnitPalette_ShowColor


@非表示
@未撃破パレットとして、真っ黒なパレットをロードする。
@ようするに、コンナの犯人風な感じ
ldr  r3, ArenaDicConfig
ldrb r0, [r3, #0x00]	@ ArenaDicConfig->未撃破パレット
b    GetUnitPalette_Exit

GetUnitPalette_ShowColor:
mov r3, r9
ldr r3, [r3, #0x34] @ ArenaDicStruct
mov r0, #0x10
ldsb r0, [r3, r0] @ ArenaDicStruct->Palette

GetUnitPalette_Exit:
pop {r1}
bx r1


.ltorg
DATA:
.equ	ArenaDicConfig,	DATA+0
