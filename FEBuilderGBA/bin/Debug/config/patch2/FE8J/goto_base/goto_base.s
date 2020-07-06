.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4,lr}

mov r4,r0  @Procsの保存

mov r0,#0xA4
blh 0x080860d0   //フラグ状態確認関数 RET=結果BOOL r0=確認するフラグ:FLAG	{J}
cmp r0,#0x0
bne Term

@PrepMenuを終了させる
mov r0,r4
blh 0x08097520	@PrepMenu_PressStart_To_Battle	{J}

@ターン数を記録したくないのでフラグ0x03を強制的にオフにする
mov r0,#0x3
blh 0x080860bc	@フラグを下す関数	r0=下すフラグ:FLAG	{J}

@拠点から移動したことを記録したいので、フラグ0xA4をONにします
mov r0,#0xA4
blh 0x080860a8	@フラグを立てる関数 r0=立てるフラグ:FLAG	{J}

@フラグを8つ消費して現在の章を格納する
@FLAG A5-AC
ldr r3,=0x0202BCEC   @ChapterData {J}
ldrb r1,[r3,#0xE]

ldrb r0, =0x03005248
strb r1,[r0]

@イベントエンジンを動かして、ベースマップに移動する
ldr r0,=0x0202B6A8 - 0x80	@テキストバッファの下の方を少しかります
mov r1,#0x23         @MNC3
strb r1,[r0,#0x0]
mov r1,#0x2A
strb r1,[r0,#0x1]
ldr  r1,GotoChapter
strh r1,[r0,#0x2]
mov r1,#0x28         @NoFade+Term
strb r1,[r0,#0x4]
mov r1,#0x02
strb r1,[r0,#0x5]
mov r1,#0x07
strb r1,[r0,#0x6]
mov r1,#0x00
strb r1,[r0,#0x7]
mov r1,#0x20
strb r1,[r0,#0x8]
mov r1,#0x01
strb r1,[r0,#0x9]
mov r1,#0x00
strb r1,[r0,#0xA]
@mov r1,#0x00
strb r1,[r0,#0xB]

MOV r1, #0x1
blh 0x0800D340	@イベント命令を動作させる関数	r0=イベント命令ポインタ:POINTER_EVENT	r1=引数?1-3	{J}

Term:
pop {r4}
pop {r0}
bx r0

.ltorg
GotoChapter:

