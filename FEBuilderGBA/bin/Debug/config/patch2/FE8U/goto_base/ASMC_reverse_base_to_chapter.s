@Call 9783C	{J}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


push {lr}

@ターン数を記録したくないのでフラグ0x03を強制的にオフにする
mov r0,#0x3
@blh 0x080860bc	@フラグを下す関数	r0=下すフラグ:FLAG	{J}
blh 0x08083D94	@フラグを下す関数	r0=下すフラグ:FLAG	{U}


@拠点から移動戻ったので、フラグ0xA4をOFFにします
mov r0,#0xA4
@blh 0x080860bc	@フラグを下す関数	r0=下すフラグ:FLAG	{J}
blh 0x08083D94	@フラグを下す関数	r0=下すフラグ:FLAG	{U}


@フラグを8つ消費して格納されている章IDを取り出す
@ldrb r0, =0x03005248	@{J}
ldrb r0, =0x03005258	@{U}
ldrb r3,[r0]

@イベントエンジンを動かして、ベースマップに移動する
@ldr r0,=0x0202B6A8 - 0x80	@テキストバッファの下の方を少しかります	@{J}
ldr r0,=0x0202B6AC - 0x80	@テキストバッファの下の方を少しかります	@{U}
mov r1,#0x23         @MNC3
strb r1,[r0,#0x0]
mov r1,#0x2A
strb r1,[r0,#0x1]
mov  r1,r3
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
@blh 0x0800D340	@イベント命令を動作させる関数	r0=イベント命令ポインタ:POINTER_EVENT	r1=引数?1-3	{J}
blh 0x0800D07C	@イベント命令を動作させる関数	r0=イベント命令ポインタ:POINTER_EVENT	r1=引数?1-3	{U}

pop {r0}
bx r0

.ltorg
