@ORG 0xD14FC		@{J}
@ORG 0xCC7E0		@{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@
@なぜかspにデータを保存しているので、それを使わないように、強制します
@条件にマッチしたデータを拾う部分

mov r6, #0x0
add r3 ,r3, #0x1

ldr  r2, Table
lsl  r0, r3, #0x4   @index*16
ldr  r0, [r2, r0]
cmp  r0, #0xff      @0xff 00 00 00 終端データの確認
beq  Exit

Loop:
@ldr  r0, =0x80d1440|1	@{J}
ldr  r0, =0x80cc724|1	@{U}
bx   r0

Exit:
@ldr  r0, =0x80d160c|1	@{J}
ldr  r0, =0x80cc8f0|1	@{U}
bx   r0

.align
.ltorg
Table:
@Table
