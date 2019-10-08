@編を切り替える	[Edition]	Args
@
@40 0D [Edition] 00 [ASM+1]
@
@Author 7743
@
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

	ldr  r3, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r1, [r3, #0x2]       @引数1 edition

	ldr  r0, =0x0202BCF0      @ (ChapterData@ステージの領域.Clock ) {U}
	strb r1, [r0, #0x1B]      @ChapterData@ステージの領域.編指定 序盤=0x1,A編=0x2,F編=0x3

	MOV r0, #0x17

	pop	{r1}
	bx	r1
