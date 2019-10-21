@
@メモリスロットの値を文字列で取得
@
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
	push	{lr}

	ldr  r1, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r1, [r1, #0x2]       @引数1 [MemorySlot]
	lsl  r1, r1 , #0x2        @ * 4


	@値を @0080@0005で取れるように書き込む
	@注意:.0080.0005は会話でしか取れない。システムメッセージでは読めないらしい
	ldr	r2, =0x030004B0	@{J}
@	ldr	r2, =0x030004B8	@{U}
	ldr	r0, [r2, r1]

	blh		0x08008914 ,r1	@{J}
@	blh		0x08008A18 ,r1	@{U}

exit_return:
	pop	{r0}
	bx r0
