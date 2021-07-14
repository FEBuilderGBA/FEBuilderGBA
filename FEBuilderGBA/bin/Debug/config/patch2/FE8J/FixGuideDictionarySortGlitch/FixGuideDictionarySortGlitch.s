@辞書でソート2を利用すると、表示が乱れるバグを修正する
@辞書で、右側の項目が多い内容を表示した後に、セレクトキーを押して、項目をソートすると、左側の項目の表示名が乱れることがあります。
@このパッチは、その問題を修正します。
@
@これは、gGenericBufferを使いまわしているからです。
@辞書では、gGenericBufferをいくつかの領域に区切り、データを書き込んでいます。
@ただ、右側の件数が多い辞書の項目を表示すると、データの突き抜けが発生します。
@その結果、辞書の左側の項目名のバッファが最初に破壊されます。
@そこにはText_Initで初期化されたデータがあり、これが壊れることで表示が乱れます。
@
@壊れてしまうものはしかたないので、ソートする時に再度初期化することで修正します。
@
@Text_Initで初期化すると、それを連続して繰り返すと別のメモリ溢れがおきるので、TextParamsだけをコピーします。
@辞書では同一フォントだけを利用しているので、たぶんこれで大丈夫です。

@HOOK	080D3E7C	@{J}
@HOOK	080CF180	@{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


@r2には、gGenericBuffer が入っているので保護しないといけない
push {r2}

@左側項目を描画する変数を再初期化する
ldr r0, [r2, #0x0] @gGenericBuffer
add r0, #0x7c
mov r1, #0x9
bl  ReInitText

pop {r2}


@壊すコードの再送
ldr r0, [r2, #0x0] @gGenericBuffer
add r0, #0x29
mov r1, #0x0
strb r1, [r0, #0x0]

@戻す
ldr r3, =0x080D3E84|1	@{J}
@ldr r3, =0x080CF188|1	@{U}
bx r3

ReInitText:
push {r4,lr}
ldr r2, =0x02028E70	@TextParams	@{J}	@{U}
ldr r3, [r2, #0x0]

ldrh r2, [r3, #0x12]
mov r4, #0x0
strh r2, [r0, #0x0]
strb r1, [r0, #0x4]
strb r4, [r0, #0x6]

mov r2, #0x1
strb r2, [r0, #0x5]
strb r4, [r0, #0x7]

pop {r4}
pop {r0}
bx r0
