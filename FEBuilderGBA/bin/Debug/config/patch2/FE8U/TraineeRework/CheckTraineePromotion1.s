@ORG D1440	@{J}
@ORG CC724	@{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@
@なぜかspにデータを保存しているので、それを使わないように、強制します
@テーブル選択部

@r4  table + add address(index*16)
@r3  index

ldr  r2, Table
lsl  r4, r3, #0x4   @index*16
str  r3, [sp, #0xc] @バニラではスタック変数に保存してる。
                    @無駄だとは思うけど、構造を変えたくないので利用します.
add  r5 ,r2, r4

ldrh r0,[r5, #0x4]  @CheckFlag
cmp  r0,#0x0
beq  CheckUnit

@blh  0x080860d0	@CheckFlag	@{J}
blh  0x08083DA8	@CheckFlag	@{U}
cmp  r0, #0x0
beq  Exit       @NotFound
                @ユニットがいないことにして、このテーブルの内容を握りつぶします.

CheckUnit:
ldrb r0, [r5, #0x0]
@blh 0x08017fb0   @GetUnitByCharId	@{J}
blh 0x0801829c   @GetUnitByCharId	@{U}

Exit:
mov r2 ,r0
ldr r3,[sp, #0xc]   @先ほど保存したr3の復帰

@ldr r0, =0x080D1452|1	@{J}
ldr r0, =0x080CC736|1	@{U}
bx  r0

.align
.ltorg
Table:
@Table
