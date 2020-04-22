@軍師の名前をコピーして設定する
@
@40 0D [X] [Y] [TEXT]
@
@Author 7743
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
.thumb
	push	{lr}

	ldr  r0, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrh r0, [r0, #0x2]       @引数0 [TEXT]

	blh 0x08009fa8   @GetStringFromIndex	{J}
	blh 0x08031438   @SetTacticianName	{J}
Term:
	pop {pc}
