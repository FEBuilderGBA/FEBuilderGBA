@指定地点がFogで隠れていれば1, されていなければ0が返される
@
@40 0D 00 00 [ASM+1]
@SlotB [XX,YY]
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

	ldr r3, =0x030004B0	@MemorySlot0	{J}
@	ldr r3, =0x030004B8	@MemorySlot0	{U}

	@そもそも霧が出ているか?
	ldr r0, =0x0202BCEC @ChapterData	{J}
@	ldr r0, =0x0202BCF0 @ChapterData	{U}
	ldrb r0, [r0, #0xd] @ChapterData@ステージの領域.霧の濃さ (0なら霧なし)
	cmp  r0, #0x0
	beq  ReturnFalse

	ldr r2, =0x0202E4E4	@gMapFog	{J}
@	ldr r2, =0x0202E4E8	@gMapFog	{U}
	ldr r2, [r2]
	
	mov  r0, #0xB * 4 + 2
	ldrb r0, [r3, r0]	@Y
	ldr  r1,  =0x0202E4D0 @gMapSize	{J}
@	ldr  r1,  =0x0202E4D4 @gMapSize	{U}
	ldrb  r1, [r1,#0x2]   @gMapSize.Height
	cmp  r0, r1
	bge  ReturnFalse

	lsl  r0, #0x2 @*4
	ldr r2, [r2, r0]

	mov  r0, #0xB * 4 + 0
	ldrb r0, [r3, r0]	@X
	ldr  r1,  =0x0202E4D0 @gMapSize	{J}
@	ldr  r1,  =0x0202E4D4 @gMapSize	{U}
	ldrb r1, [r1]         @gMapSize.Width
	cmp  r0, r1
	bge  ReturnFalse

	ldrb r0, [r2, r0]
	cmp  r0, #0x0
	bne  ReturnFalse

	mov  r0, #0x1
	b    Exit

ReturnFalse:
	mov  r0, #0x0
Exit:
	mov  r1, #0xC * 4 + 0
	strb r0, [r3, r1]	@storeC

	pop	{r1}
	bx	r1
