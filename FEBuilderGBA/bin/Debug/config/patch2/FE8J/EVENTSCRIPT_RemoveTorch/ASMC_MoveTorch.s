@エリア内のトーチの杖光を移動させる
@
@40 0D 00 00 [ASM+1]
@Slot1 [XX,YY]
@Slot2 [XX,YY]
@Slot3 [ShiftXX,ShiftYY]
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
	push	{r4,r5,r6,r7,lr}

	ldr r4, =0x030004B0	@MemorySlot0	{J}
@	ldr r4, =0x030004B8	@MemorySlot0	{U}
	ldr r5, =0x0203A610	@TrapEntry		{J}
@	ldr r5, =0x0203A614	@TrapEntry		{U}

	ldr r6, =0x40 * 8
	add r6, r5			@Term
	
	mov r7, #0x0
	sub r5, #0x8
Loop:
	add r5, #0x8
	cmp r5, r6
	bge Break

	ldrb r0, [r5, #0x2]
	cmp r0, #0xA @torch staff
	bne Loop

	ldrb r0, [r4, #0x1 * 0x4 + 0x0]	@range1->x
	ldrb r1, [r5, #0x0] @trap->x
	cmp  r1, r0
	blt  Loop
	
	ldrb r0, [r4, #0x1 * 0x4 + 0x2]	@range1->y
	ldrb r1, [r5, #0x1] @trap->y
	cmp  r1, r0
	blt  Loop

	ldrb r0, [r4, #0x2 * 0x4 + 0x0]	@range2->x
	ldrb r1, [r5, #0x0] @trap->x
	cmp  r1, r0
	bgt  Loop
	
	ldrb r0, [r4, #0x2 * 0x4 + 0x2]	@range2->y
	ldrb r1, [r5, #0x1] @trap->y
	cmp  r1, r0
	bgt  Loop

	Update:
	mov  r7, #0x01  @update

	UpdateX:
	mov  r1, #0x3 * 0x4 + 0x0
	ldsh r1, [r4, r1]	@ShiftX
	ldrb r0, [r5, #0x0] @trap->x
	add  r0, r1
	cmp  r0, #0x0
	blt  Disappearance

	ldr  r3, =0x0202E4D0	@gMapSize		{J}
@	ldr  r3, =0x0202E4D4 @gMapSize		{U}
	ldrh r2, [r3, #0x0]	@gMapSize.Width
	cmp  r0, r2
	bge  Disappearance
	strb r0, [r5, #0x0] @trap->x
	
	UpdateY:
	mov  r1, #0x3 * 0x4 + 0x2
	ldsh r1, [r4, r1]	@ShiftY
	ldrb r0, [r5, #0x1] @trap->y
	add  r0, r1
	cmp  r0, #0x0
	blt  Disappearance

	ldrh r2, [r3, #0x2]	@gMapSize.Height
	cmp  r0, r2
	bge  Disappearance
	strb r0, [r5, #0x1] @trap->y
	b    Loop

	Disappearance:
	mov  r0, r5
	blh  0x0802e234   @RemoveTrap	{J}
@	blh  0x0802E2FC   @RemoveTrap	{U}
	sub  r5, #0x8
	b    Loop

Break:
	cmp r7 ,#0x0
	beq Term

	blh 0x08019ecc   @RefreshFogAndUnitMaps		{J}
@	blh 0x0801a1f4   @RefreshFogAndUnitMaps		{U}
	blh 0x08027144   @SMS_UpdateFromGameData	{J}
@	blh 0x080271a0   @SMS_UpdateFromGameData	{U}
	blh 0x08019914   @UpdateGameTilesGraphics	{J}
@	blh 0x08019c3c   @UpdateGameTilesGraphics	{U}

Term:
	pop	{r4,r5,r6,r7}
	pop	{r1}
	bx	r1
