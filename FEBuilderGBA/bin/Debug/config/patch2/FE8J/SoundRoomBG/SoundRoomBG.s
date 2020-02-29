@Hook B4458 {J}
@Hook AF838 {U}
@
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


push {r0}

blh 0x08089078	@{J}  Procs  GAMECTRL CallASM  DeleteEach6C BG3HSlide
@blh 0x08086DBC	@{U}  Procs  GAMECTRL CallASM  DeleteEach6C BG3HSlide

bl ClearScreen

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

ldr r3,SoundRoomBG_Table
CountTable_Loop:
ldrb r2,[r3]
cmp r2,#0xFF
bne CountTable_Next

ldrb r2,[r3,#0x1]
cmp r2,#0xFF
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

ldrb r1,[r3]
cmp r1,#0xff
beq ShowTableData_CG

ShowTableData_BG:
mov r0,#0x1  @BG
b ShowTableData_Exit

ShowTableData_CG:
ldrb r1,[r3,#0x01]
mov r0,#0x2  @CG

@大昔に作ったルーチンが足を引っ張ってるなあ..
@イベントエンジンから呼び出されることを前提に作っているので、スタックを使って合意うに辻褄を合わせます。
mov r5,sp
mov r3,#0x38
str r5,[r5,r3]
strb r1,[r5,#0x2]

ShowTableData_Exit:
blh 0x0800E9E0	@{J} LoadBackgound
@blh 0x0800E7D0	@{U} LoadBackgound

ADD SP, #0x40
pop {r5}
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

@画面のちらつきを抑制するため、画像がロードされるパレット領域を0クリアする.
ldr r0, =0x05000100	@Palette BGP8
mov r1, #0x0
mov r2, #0xA0 @0x20 * 8palette
blh 0x080d6968	@{J} memset
@blh 0x080D1C6C	@{U} memset

ldr r0, =0x020229A8	@Palette Buffer BGP8
mov r1, #0x0
mov r2, #0xA0 @0x20 * 8palette
blh 0x080d6968	@{J} memset
@blh 0x080D1C6C	@{U} memset

pop {r0}
bx r0


.ltorg
.align
SoundRoomBG_Table:
@POIN SoundRoomBG_Table
