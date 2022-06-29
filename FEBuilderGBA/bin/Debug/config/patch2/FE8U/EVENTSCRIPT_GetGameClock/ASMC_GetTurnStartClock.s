@ゲームクリックの取得
@
@Author 7743
@
@
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
.thumb
	push	{lr}

@	ldr  r0, =0x0202BCEC @ChapterData	@{J}
	ldr  r0, =0x0202BCF0 @ChapterData	@{U}
	ldr  r0, [r0]	@ChapterData.TurnClock

@	ldr  r1, =0x030004B0      @FE8J MemorySlot 00
	ldr  r1, =0x030004B8      @FE8U MemorySlot 00

	str  r0, [r1,#0x4 * 0xC]	@SlotC

	pop	{r0}
	bx	r0

