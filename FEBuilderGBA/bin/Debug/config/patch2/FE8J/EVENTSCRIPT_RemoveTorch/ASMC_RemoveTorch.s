@エリア内のトーチの杖光を消す
@
@40 0D 00 00 [ASM+1]
@Slot1 [XX,YY]
@Slot2 [XX,YY]
@Slot3 Sub
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

	ldr r6, =0x40 * 0x8
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

	mov  r0, #0x3 * 0x4
	ldsb r0, [r4, r0]	@sub
	ldrb r1, [r5, #0x3] @trap->torch_count
	sub  r1, r0
	cmp  r1, #0x0
	ble  Disappearance

	Store:
	strb r1, [r5, #0x3] @trap->torch_count
	mov  r7, #0x01  @update
	b    Loop

	Disappearance:
	mov  r0, r5
	blh  0x0802e234   @RemoveTrap	{J}
@	blh  0x0802E2FC   @RemoveTrap	{U}
	sub  r5, #0x8
	mov  r7, #0x01  @update
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
