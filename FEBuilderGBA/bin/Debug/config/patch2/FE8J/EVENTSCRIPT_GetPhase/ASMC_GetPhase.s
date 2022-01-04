@フェーズの取得 0=自軍,0x40=友軍,0x80=敵軍
@
@40 0D 00 00 [ASM+1]
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

	ldr  r3, =0x0202BCEC @ChapterData	{J}
@	ldr  r3, =0x0202BCF0 @ChapterData	{U}

	ldrb r0, [r3, #0xf] @ChapterData@0F	byte	フェイズ 0=自軍,0x40=友軍,0x80=敵軍

	ldr  r3, =0x030004B0      @FE8J MemorySlot 00
@	ldr  r3, =0x030004B8      @FE8U MemorySlot 00
	str  r0, [r3, #0xC * 4]

Term:
	pop	{r1}
	bx	r1
