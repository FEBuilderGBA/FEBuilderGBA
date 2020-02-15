@簡単に作れる離脱イベント
@@簡易離脱イベント 全員が離脱するとクリア[離脱セリフ:xxxx]
@@簡易離脱イベント 全員が離脱するとクリア[最後のユニット:][拒否セリフ:][離脱セリフ:xxxx]
@@簡易離脱イベント FE9みたいに特定ユニットが離脱するとクリア[最後のユニット:][離脱セリフ:xxxx]
@@sval1 種類				Type
@@sval2 離脱セリフの構造体	EscapeText struct
@@sval3 最後のユニット		The last unit id
@@sval4 拒否セリフ			Denied serif
@@
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

@Main
push {r4,lr}
@ldr	r4, =0x030004B0	@MemorySlot		{J}
ldr	r4, =0x030004B8	@MemorySlot	{U}

ldrb r0,[r4,#0x01 * 0x4]	@Slot1
cmp r0,#0x00
beq ModeAllEscape
cmp r0,#0x01
beq ModeSpecificUnitLeavesLast
cmp r0,#0x02
beq ModeLikeFE9
b Main_Exit

@離脱イベント 全員が離脱するとクリア
ModeAllEscape:

ldr r0,[r4,#0x02 * 0x4]	@Slot2 離脱セリフの構造体
bl ShowEscapeText

bl GetUnitsLeft @残り人数の取得
cmp r0 ,#0x01
ble Main_SetWinFlag

bl Escape @現在のユニットを離脱させる
b Main_Exit


@離脱イベント 全員が離脱するとクリア
ModeSpecificUnitLeavesLast:

bl GetUnitsLeft
cmp r0 ,#0x01
ble ModeAllEscape @残り1人以下なら、普通に離脱ルーチン

ldrb r0,[r4,#0x03 * 0x4]	@Slot3 最後のユニット
bl IsTargetUnit
cmp r0,#0x00
beq ModeAllEscape @目的のユニットではないので、普通に離脱

ldrh r0,[r4,#0x04 * 0x4]	@Slot4 拒否セリフ
bl ShowText
b Main_Exit


@離脱イベント FE9みたいに指定したユニットが離脱するとクリア
ModeLikeFE9:
ldrb r0,[r4,#0x03 * 0x4]	@Slot3 最後のユニット
bl IsTargetUnit
cmp r0,#0x00
beq ModeAllEscape @目的のユニットではないので、普通に離脱

b Main_SetWinFlag

@勝利フラグを設定
Main_SetWinFlag:
mov r0,#0x3
@blh 0x080860a8   @フラグを立てる関数 r0=立てるフラグ:FLAG	{J}
blh 0x08083d80   @フラグを立てる関数 r0=立てるフラグ:FLAG	{U}
@blh 0x080855B8   @CallEndEvent	{J}
blh 0x08083280   @CallEndEvent	{U}

@b Main_Exit


Main_Exit:
pop {r4}
pop {r0}
bx r0


@セリフを表示する
@この関数は、メモリスロットに書き込むので注意してください。
@r0 textid
ShowText:
push {lr}
cmp r0,#0x00
beq ShowText_end
@blh 0x0800D524	@CallEvent イベント "会話0xFFFF"	{J}
blh 0x0800D260	@CallEvent イベント "会話0xFFFF"	{U}

ShowText_end:
pop {r0}
bx r0

@離脱セリフの表示
@r0 EscapeText struct
ShowEscapeText:
push {lr}
mov r3,r0 @EscapeText struct
cmp r3,#0x00
beq ShowEscapeText_end

@ldr r2,=0x03004DF0 @(操作キャラのワークメモリへのポインタ )	{J}
ldr r2,=0x3004E50	@(操作キャラのワークメモリへのポインタ )	{U}
ldr r2,[r2]
ldr r0,[r2,#0x00]
cmp r0,#0x00
beq ShowEscapeText_end

ldrb r0,[r0,#0x04] @unit->id
cmp r0,#0x45
bgt ShowEscapeText_end

lsl  r0,#0x1  @ *2
add  r0,r0,r3    @ EscapeText[unit->id]
ldrh r0,[r0]
bl ShowText

ShowEscapeText_end:
pop {r0}
bx r0

@離脱処理の実行
Escape:
push {r4,lr}
@ldr r4,=0x03004DF0 @(操作キャラのワークメモリへのポインタ )	{J}
ldr r4,=0x3004E50	@(操作キャラのワークメモリへのポインタ )	{U}
ldr r4,[r4]

@自軍以外なら終了
ldrb	r0, [r4, #0xB]
lsr	r0, r0, #6
bne	Escape_end

ldrb	r1, [r4, #0xC]
lsl	r1, r1, #27
bpl	Escape_run		@担いでないならジャンプ

@担いでいるユニットを離脱させる
ldrb	r0, [r4, #0x1B]
@blh     0x08019108	@GetUnitStruct	{J}
blh     0x08019430	@GetUnitStruct	{U}

cmp  r0, #0x00
beq  Escape_run  @データが取得できなかったので無視
mov  r3, r0
ldrb r0, [r3, #0xC]
mov  r1, #0x8 @undeployed(0x08)
orr  r0, r1
strb r0, [r3, #0xC]

@自分の離脱
Escape_run:
mov  r0, #0
strb r0, [r4, #0x13]

ldr r0,[r4,#0xC]
mov r1,#0x08 @undeployed(0x08)
orr r0,r1
str r0,[r4,#0xC]

Escape_end:
pop {r4}
pop {r0}
bx r0


@残ったプレイヤーユニットの数を取得
@RET ユニット数
GetUnitsLeft:
push {r4,lr}
@ldr r2, =0x0202BE48	@gUnitArray	{J}
@ldr r3, =0x0202BE48+(0x32*0x48)	@gUnitArray	{J}
ldr r2, =0x0202BE4C	@gUnitArray	{U}
ldr r3, =0x0202BE4C+(0x32*0x48)	@gUnitArray	{U}
mov  r4,#0x0

GetUnitsLeft_loop:
ldr  r0, [r2]
cmp  r0, #0
beq  GetUnitsLeft_next

ldr  r1, [r2, #0xC]
mov  r0, #0x4	@Dead 戦死
and  r0, r1
bne  GetUnitsLeft_next

lsl  r0, r1, #0xF	@Escape 離脱
bmi  GetUnitsLeft_next

mov  r0, #0x20	@Rescued 被救出
and  r0, r1
bne  GetUnitsLeft_next

mov  r0, #0x8   @undeployed(0x08)
and  r1, r0
bne  GetUnitsLeft_next

add  r4,r4,#0x01

GetUnitsLeft_next:
add  r2, #0x48
cmp  r2, r3
blt	GetUnitsLeft_loop

GetUnitsLeft_break:
mov  r0, r4

GetUnitsLeft_end:
pop {r4}
pop {r1}
bx r1


@操作中のユニットは、r0で指定されたUnitIDかどうか判定する
@r0 unit id
@RET bool
IsTargetUnit:
push {r4,lr}
mov r4, r0

@ldr r3,=0x03004DF0 @(操作キャラのワークメモリへのポインタ )	{J}
ldr r3,=0x3004E50	@(操作キャラのワークメモリへのポインタ )	{U}
ldr r3,[r3]
ldr r1,[r3,#0x00]
ldrb r2,[r1,#0x4] @unit->id

cmp r4,r2
beq IsTargetUnit_return_true

ldr	r2, [r3, #0xC]
lsl	r2, r2, #27
bpl	IsTargetUnit_return_false		@担いでいない

@担いでいるユニットIDの取得
ldrb	r0, [r3, #0x1B]
@blh	0x08019108	@GetUnitStruct	{J}
blh		0x08019430	@GetUnitStruct	{U}

cmp  r0, #0x00
beq  IsTargetUnit_return_false  @データが取得できなかったので無視

ldr r1,[r0,#0x00]
ldrb r2,[r1,#0x4] @unit->id
cmp r4,r2
beq IsTargetUnit_return_true    @担いでいるユニットは、探していたユニット

IsTargetUnit_return_false:
mov r0, #0x00
b   IsTargetUnit_Exit

IsTargetUnit_return_true:
mov r0, #0x01

IsTargetUnit_Exit:
pop {r4}
pop {r1}
bx r1
