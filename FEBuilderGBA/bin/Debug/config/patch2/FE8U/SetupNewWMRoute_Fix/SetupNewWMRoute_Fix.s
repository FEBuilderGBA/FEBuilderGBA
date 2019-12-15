@大陸を周回させる改造をしたときに道が作成されないバグを回避するため、
@既にリストに入っている道は、リストに追加しないようにする
@
.thumb
@@@.org 0x080BC8BC    @FE8U
@r0  gSomeWMEventRelatedStruct !注意!:r0は戻り値を入れるまで書き換え禁止!
@r1  path array byte[0x20] and byte[0x21]=length
@r2  new path id

push {r6,r5,r4,lr}
mov  r6,#0x20   @const扱い
ldrb r5,[r1,r6] @道の個数を取得 r6=0x20
cmp  r5,#0x0    @ゲームオーバーになった時配列が0クリアされた状態で呼び出されることがある
beq  NotFound

mov  r3,#0x00

Loop:           @ 空きを探す
ldrb r4,[r1, r3]
cmp  r4,r2
beq  TrueExit   @既にある

add  r3,#0x01
cmp  r3,r5
blt  Loop

cmp  r5,r6      @0x20個以未満?
blt  NotFound

mov  r0, #0x1   @リストは満杯 戻り値1を返す
b    Exit


NotFound:
strb r2, [r1, r5]     @道を追加
add  r5, #0x01
strb r5, [r1, r6]     @個数の更新 r6=0x20

bl 0x080bca0c         @FE8U nazo function たぶん道を画面に出す関数?
                      @引数r0が必要。 だから、これを呼ぶまでr0は保持しないといけない

TrueExit:
mov  r0, #0x0   @正常終了は0を返す.

Exit:
pop {r6,r5,r4}
pop {pc}
