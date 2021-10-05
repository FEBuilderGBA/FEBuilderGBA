@Hook 0B4458 {J}
@Hook 0AF838 {U}
@
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


push {r0}

bl ClearScreen

blh 0x08089078	@{J}  Procs  GAMECTRL CallASM  DeleteEach6C BG3HSlide
@blh 0x08086DBC	@{U}  Procs  GAMECTRL CallASM  DeleteEach6C BG3HSlide

bl CountTable
blh 0x08000c58	@{J} NextRN_N
@blh 0x08000C80	@{U} NextRN_N

bl ShowTableData

bl ReloadScreen


Exit:
pop {r0}
pop {r4,r5,r6}
pop {r1}
bx r1


@テーブルの総数を返します.
CountTable:
mov r0,#0x0    @counter

ldr r1,=0xffff

ldr r3,SoundRoomBG_Table
CountTable_Loop:
ldrh r2,[r3]
cmp r2,r1  @ check 0xffff term
beq CountTable_Exit

CountTable_Next:
add r0,#0x1
add r3,#0x2
b CountTable_Loop

CountTable_Exit:
bx lr


@テーブルから、データを取得します.
@r0,r1に、LoadBackgoundの引数として利用できる値を返します.
ShowTableData:
push {r5,lr}
SUB SP, #0x40

lsl r0,#1   @r0 * 2
ldr r3,SoundRoomBG_Table
add r3,r0

ldrb r0,[r3,#0x1]
cmp r0,#0x00
beq ShowTableData_BG
cmp r0,#0x01
beq ShowTableData_CG
cmp r0,#0x02
beq ShowTableData_BattleBG
b   ShowTableData_Exit

ShowTableData_BG:
mov r0,#0x1  @BG
ldrb r1,[r3, #0x0] @BGID
b ShowTableData_LoadBackgound

ShowTableData_CG:
ldrb r1,[r3, #0x0] @CGID
mov r0,#0x2  @CG

@大昔に作ったルーチンが足を引っ張ってるなあ..
@イベントエンジンから呼び出されることを前提に作っているので、スタックを使って合意うに辻褄を合わせます。
mov r5,sp
mov r3,#0x38
str r5,[r5,r3]
strb r1,[r5,#0x2]

ShowTableData_LoadBackgound:
blh 0x0800E9E0	@{J} LoadBackgound
@blh 0x0800E7D0	@{U} LoadBackgound
b ShowTableData_Exit

ShowTableData_BattleBG:
ldrb r0,[r3, #0x0] @BattleBGID
cmp  r0,#0x0
beq  ShowTableData_Exit

sub  r0,#0x1
bl   ShowBattleBG

ShowTableData_Exit:
ADD SP, #0x40
pop {r5}
pop {r0}
bx r0

ShowBattleBG:
push {r4,lr}
mov r4, r0

mov r0, #0x0
mov r1, #0x0
mov r2, #0x0
blh 0x08001448	@{J}	BG_SetPosition
@blh 0x0800148C	@{U} BG_SetPosition

mov r0, #0x1
mov r1, #0x0
mov r2, #0x0
blh 0x08001448	@{J}	BG_SetPosition
@blh 0x0800148C	@{U} BG_SetPosition

mov r0, #0x2
mov r1, #0x0
mov r2, #0x0
blh 0x08001448	@{J}	BG_SetPosition
@blh 0x0800148C	@{U} BG_SetPosition

mov r0, #0x3
mov r1, #0x0
mov r2, #0x0
blh 0x08001448	@{J}	BG_SetPosition
@blh 0x0800148C	@{U} BG_SetPosition

mov r0, r4
blh 0x08077F0C	@{J}	DrawBattleBG
@blh 0x08075ad8	@{U}	DrawBattleBG

pop {r4}
pop {r0}
bx r0


@背景を読込むとサウンドルームのリストが乱れるので、再描画を行います。
ReloadScreen:
push {lr}

@Random Modeの場合は再描画は不要!
ldrh r0, [r1, #0x2a]
cmp r0,#0x01   @ramdom mode?
beq ReloadScreen_Exit


ldrh r2, [r4, #0x2a]  @current pos?

ldr r1, =0x0000FFFC
mov r0, #0xff
and r2 ,r0
mov r0, #0x2
blh 0x08001448	@{J} BG_SetPosition
@blh 0x0800148C	@{U} BG_SetPosition

ReloadScreen_Exit:
pop {r0}
bx r0


@画面のちらつきを抑制する
ClearScreen:
push {lr}

@画面のちらつきを抑制するため、背景の描画に使われているBG3を強制的にOFFにします
@どうせ次の描画タイミングで自動的にreg syncされてBG3が有効になるので、気にする必要はありません。
ldr r3, =0x04000000  @REG_DISPCNT
ldr r0, [r3]
ldr r2, =0xFFFFF7FF  @not DISPLAY_BG3_ACTIVE
and r0, r2
str r0, [r3]

pop {r0}
bx r0


.ltorg
.align
SoundRoomBG_Table:
@POIN SoundRoomBG_Table
