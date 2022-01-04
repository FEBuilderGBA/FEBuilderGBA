@タイルチェンジの存在確認	[TileChangeID]
@
@40 0D [ID] 00 [ASM+1]
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

	ldr  r0, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r0, [r0, #0x2]       @引数1 trap id

@	blh 0x0802E570	@AreMapChangeTriggered	{J}
	blh 0x0802e638	@AreMapChangeTriggered	{U}

@	ldr  r3, =0x030004B0      @FE8J MemorySlot 00
	ldr  r3, =0x030004B8      @FE8U MemorySlot 00
	str  r0, [r3, #0xC * 4]

Term:
	pop	{r1}
	bx	r1
