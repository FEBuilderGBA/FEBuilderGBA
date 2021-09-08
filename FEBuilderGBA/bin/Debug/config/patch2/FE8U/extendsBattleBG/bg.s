@ORG 0x57ED0	@背景	@{J}

.thumb
@r0   tile index
@r1   pointer index
push {lr}

ldr  r3, Table
lsl  r1,r1,#0x3 @ *8
add  r1, #0x04
ldr  r1, [r3 , r1]

cmp  r1, #0x0      @未初期化リストが設定されていたら index: 0x00 のリストを使う
bne  Found
ldr  r1, [r3 , #0x4]

Found:
ldrb  r0, [r1, r0] @GetTileID

pop {r1}
bx r1

.align
.ltorg
Table:
