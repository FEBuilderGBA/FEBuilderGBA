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

Main:
push {r4,r5,r6,r7,lr}
mov r7, r0  @this proc
            @面倒なのでr7をグローバル変数みたいに使います。 this procsとして。

bl Init_ArenaDic

ldr r3, =0x02024CC0 @KeyStatusBuffer@KeyStatusBuffer.FirstTickDelay )
ldrh r2, [r3, #0x6] @KeyStatusBuffer@KeyStatusBuffer.TickPresses  1 For Press|Tick&Pressed, 0 Otherwise

mov r1, #0x1
and r1 ,r2
bne Main_PressAButton

mov r1, #0x2
and r1 ,r2
bne Main_PressBButton

mov r1, #0x10
and r1 ,r2
bne Main_PressRightButton

mov r1, #0x20
and r1 ,r2
bne Main_PressLeftButton

mov r1, #0x40
and r1 ,r2
bne Main_PressUpButton

mov r1, #0x80
and r1 ,r2
bne Main_PressDownButton

lsr r2, #0x8
mov r1, #0x01
and r1 ,r2
bne Main_PressRButton

b   Main_Exit

Main_PressBButton:
bl  Delete_opinfo

mov r0 ,r7
blh 0x08002de4   @Break6CLoop	{J}
b   Main_Exit

b Main_Exit

Main_PressLeftButton:
mov r0,#1
bl PageDown
b Main_ReloadInfo

Main_PressDownButton:
mov r0,#10
bl PageDown
b Main_ReloadInfo

Main_PressRightButton:
mov r0,#1
bl PageUp
b Main_ReloadInfo

Main_PressUpButton:
mov r0,#10
bl PageUp
b Main_ReloadInfo

Main_PressRButton:
bl PageFindHideData
b Main_ReloadInfo

Main_ReloadInfo:
Main_PressAButton:
bl Reload_opinfo

Main_Exit:
pop {r4,r5,r6,r7}
pop {r0}
bx r0


PageDown:	@r0=downPage
push {lr}
ldrh r1, [r7, #0x2A]	@thisProcs->CurrentPage
cmp r0, r1
bgt PageDown_Correction

sub r0, r1, r0
b   PageDown_store

PageDown_Correction:
@下限を超えたので最大値-1に戻す
ldrh r0, [r7, #0x2C]	@thisProcs->AllCount
cmp r0,#0x0        @異常値のチェック
beq PageDown_store

sub r0, #0x1
@b   PageDown_store

PageDown_store:
strh r0, [r7, #0x2A]	@thisProcs->CurrentPage

pop {r0}
bx r0


PageUp:	@r0=upPage
push {lr}
ldrh r1, [r7, #0x2A]	@thisProcs->CurrentPage
add r0, r1

ldrh r2, [r7, #0x2C]	@thisProcs->AllCount
cmp r0, r2
blt PageUp_store

@上限を超えたので0に戻す
mov r0, #0x0

PageUp_store:
strh r0, [r7, #0x2A]	@thisProcs->CurrentPage

pop {r0}
bx r0


Update_IsShowData:
push {lr}
bl Get_ArenaDicCurrentPage	@ArenaDicStruct[this->CurrentPage]
ldrb r1, [r0, #0xC]
cmp r1, #0x0
bne Update_IsShowData_ShowAlways

Update_IsShowData_CheckBit:
ldrh r0, [r7, #0x2A]	@thisProcs->CurrentPage
bl   CheckBit
b    Update_IsShowData_Store

Update_IsShowData_ShowAlways:
mov r0, #0x1

Update_IsShowData_Store:
mov  r1, #0x29
strb r0, [r7, r1]	@thisProcs->isShowData
pop {r0}
bx r0


PageFindHideData:
push {lr}

@既に全部埋まっていないか?
ldrh r3, [r7, #0x2C]	@thisProcs->AllCount
ldrh r1, [r7, #0x2E]	@thisProcs->Complate
cmp  r3, r1
beq  PageFindHideData_Exit

ldrh r0, [r7, #0x2A]	@thisProcs->CurrentPage
add  r0, #0x1
cmp  r0, r3
blt  PageFindHideData_Find

mov  r0, #0x0
PageFindHideData_Find:
bl   FindHideData
strh r0, [r7, #0x2A]	@thisProcs->CurrentPage

PageFindHideData_Exit:
pop {r0}
bx r0





MyDelete6C:
push {lr}
blh 0x08002dec   @Find6C	@{J}
cmp r0, #0x0
beq MyDelete6C_Exit
blh 0x08002CBC   @Delete6C	@{J}

MyDelete6C_Exit:
pop {r0}
bx r0

.ltorg

@キーを押しっぱなしにすると、Procsがつきるのかクラッシュしてしまうので少し手加減する
IsCooldown:
push {lr}

ldr r0, opinfoDrawBattleAnimeCustomProcs
blh 0x08002dec   @Find6C	@{J}
cmp r0, #0x0
beq IsCooldown_Exit

ldr r1, [r0, #0x38]	@アニメーションデータの初期化
cmp r1, #0x0
beq IsCooldown_TrueReturn

mov r0, #0x0
b   IsCooldown_Exit

IsCooldown_TrueReturn:
mov r0, #0x1

IsCooldown_Exit:
pop {r1}
bx r1


Reload_opinfo:
push {r4,lr}

bl IsCooldown
cmp r0, #0x1
beq Reload_opinfo_Abort

bl  Delete_opinfo

bl Update_IsShowData

bl Get_ArenaDicCurrentPage	@ArenaDicStruct[this->CurrentPage]
mov r4, r0

ldr r0, opinfoDrawBattleAnimeCustomProcs
mov r1 ,r7
blh 0x08002bcc   @New6C	@{J}
cmp r0, #0x0
beq Reload_opinfo_Abort

str r7, [r0, #0x30]	@親procsとして自分を登録
str r4, [r0, #0x34]	@表示するアニメーションデータ
mov r1, #0x0
str r1, [r0, #0x38]	@アニメーションデータの初期化
str r1, [r0, #0x3c]	@フレームの初期化

blh 0x0805b764   @NewEfxAnimeDrvProc

Reload_opinfo_Abort:
pop {r4}
pop {r0}
bx r0


Delete_opinfo:
push {lr}

ldr r0, opinfoDrawBattleAnimeCustomProcs
bl MyDelete6C

ldr r0, =0x85E3FDC @ekrAnimeDrvProc	@{J}
bl MyDelete6C

pop {r0}
bx r0

Get_ArenaDicCurrentPage:
push {lr}

ldrh r0, [r7, #0x2A]	@thisProcs->CurrentPage

mov  r1, #0x14	@sizeof(ArenaDicStruct)
mul  r0, r1

ldr r1, ArenaDicStruct
add r0, r1			@ArenaDicStruct[this->CurrentPage]

pop {r1}
bx r1


Init_ArenaDic:
push {r4,r5,r6,lr}

ldrh r0, [r7, #0x2C]	@thisProcs->AllCount
cmp  r0, #0x0
bne  Init_ArenaDic_Exit	@既に初期化済みなら何もしない

@初期化する
mov r5, #0x0	@達成数を求める
mov r6, #0x0	@件数を求める

ldr r4, ArenaDicStruct
Init_ArenaDic_Loop:
ldr r0, [r4]
cmp r0, #0x0
beq Init_ArenaDic_Break

Init_ArenaDic_CheckFlag:
ldrb r0, [r4,#0x0C]	@ArenaDicStruct->ShowAlways
cmp r0, #0x0
bne Init_IncrementAchievement

mov r0, r6
bl  CheckBit
cmp r0 ,#0x0
beq Init_ArenaDic_Next

Init_IncrementAchievement:
add r5, #0x1	@達成数++

Init_ArenaDic_Next:
add r4, #0x14	@sizeof(ArenaDicStruct)
add r6, #0x1	@件数++
b   Init_ArenaDic_Loop

Init_ArenaDic_Break:

@総数
strh r6, [r7, #0x2C]	@thisProcs->AllCount

@達成数
strh r5, [r7, #0x2E]	@thisProcs->ComplateCount

Init_ArenaDic_Reload:
bl AutoFocus
bl Reload_opinfo

Init_ArenaDic_Exit:
pop {r4,r5,r6}
pop {r0}
bx r0


AutoFocus:
push {r4,r5,r6,lr}

ldr r3, =0x0203A568	@gBattleTarget
ldrb r1, [r3,#0xB]  @gBattleTarget->所属
cmp r1, #0x80
blt AutoFocus_Exit

ldr r3, =0x0203E108	@BattleAnime1
ldrh r1, [r3]   @直前に戦った相手の戦闘アニメ
                @このデータがあれば、CurrentPageとして差し込む
cmp  r1, #0x0
beq  AutoFocus_Exit
add  r1, #0x1
mov  r5, r1     @AnimationID

mov r6, #0x0   @Counter

ldr r4, ArenaDicStruct
AutoFocus_Loop:
ldr r0, [r4]
cmp r0, #0x0
beq AutoFocus_Exit

ldrh r0, [r4,#0x0E]	@ArenaDicStruct->BattleAnime
cmp  r0, r5
bne  AutoFocus_Next
strh r6, [r7, #0x2A]	@thisProcs->CurrentPage
b    AutoFocus_Exit

AutoFocus_Next:
add r4, #0x14	@sizeof(ArenaDicStruct)
add r6, #0x1	@件数++
b   AutoFocus_Loop

AutoFocus_Exit:
pop {r4,r5,r6}
pop {r0}
bx r0


@r0の地点より最初に見つけた非公開データを返します.
FindHideData:
push {lr, r4, r5, r6}

mov r5, r0 @counter
mov r6, r0 @keep

ldr r4, ArenaDicStruct
mov r1, #0x14	@sizeof(ArenaDicStruct)
mul r0, r1
add r4, r0

FindHideData_Loop:
ldr r0, [r4]
cmp r0, #0x0
beq FindHideData_Break

FindHideData_CheckFlag:
ldrb r0, [r4,#0xC]	@ArenaDicStruct->ShowAlways
cmp r0, #0x0
bne FindHideData_Next

mov r0, r5
bl  CheckBit
cmp r0 ,#0x0
bne FindHideData_Next

@最初に見つかった非公開データ
mov r0, r5
b   FindHideData_Exit

FindHideData_Next:
add r4, #0x14	@sizeof(ArenaDicStruct)
add r5, #0x1	@件数++
b   FindHideData_Loop

FindHideData_Break:

mov r0, #0x0
cmp r6, #0x0
beq FindHideData_Exit

@先頭から再スキャン
bl  FindHideData

FindHideData_Exit:
pop {r4, r5, r6}
pop {r1}
bx r1



CheckBit:
push {lr}
@convert to bitflag
asr r2 ,r0 ,#0x5
lsl r2 ,r2 ,#0x2

mov r1, #0x1f
and r0 ,r1

mov r1 ,#0x01
lsl r1 ,r0

ldr r3, ArenaDicConfig
ldr r3, [r3, #0x1C]	@ArenaDicConfig->RAM

ldr r0, [r3, r2]
and r0 ,r1
cmp r0, #0x0
beq CheckBit_Exit

mov r0, #0x1
CheckBit_Exit:
pop {r1}
bx r1


.ltorg
DATA:
.equ	opinfoDrawBattleAnimeCustomProcs,	DATA+0
.equ	ArenaDicStruct,	DATA+4
.equ	ArenaDicConfig,	DATA+8
