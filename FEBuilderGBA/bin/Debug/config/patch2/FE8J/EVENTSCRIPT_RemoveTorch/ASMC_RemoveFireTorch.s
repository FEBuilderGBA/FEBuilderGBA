@エリア内のたいまつの光を消す
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
	ldr r5, =0x0202BE48 @UnitRAM FE8J
	@ldr r5, =0x0202BE4C @UnitRAM FE8U

	ldr r6,=0x85 * 0x48 @Player+Enemy+NPC
	add r6,r5

	mov r7, #0x0

	sub r5,#0x48        @Because it is troublesome, first subtract
Loop:
	add r5, #0x48
	cmp r5, r6
	bge Break

	ldr r0, [r5, #0x0]
	cmp r0, #0x0
	beq Loop

	ldrb r0, [r4, #0x1 * 0x4 + 0x0]	@range1->x
	ldrb r1, [r5, #0x10] @unit->x
	cmp  r1, r0
	blt  Loop
	
	ldrb r0, [r4, #0x1 * 0x4 + 0x2]	@range1->y
	ldrb r1, [r5, #0x11] @unit->y
	cmp  r1, r0
	blt  Loop

	ldrb r0, [r4, #0x2 * 0x4 + 0x0]	@range2->x
	ldrb r1, [r5, #0x10] @unit->x
	cmp  r1, r0
	bgt  Loop
	
	ldrb r0, [r4, #0x2 * 0x4 + 0x2]	@range2->y
	ldrb r1, [r5, #0x11] @unit->y
	cmp  r1, r0
	bgt  Loop

	mov  r1, #0x31
	ldrb r1, [r5, r1] @trap->purewater_and_torch
	mov  r0, #0xf
	and  r1, r0
	cmp  r1, #0x0
	beq  Loop

	mov  r2, r1

	mov  r0, #0x3 * 0x4
	ldsb r0, [r4, r0]	@sub
	sub  r1, r0

	cmp  r1, #0x0
	bgt  Store
	mov  r1, #0x0

	Store:
	cmp  r2, r1
	beq  Loop   @変更なし

	mov  r2, #0x31
	ldrb r2, [r5, r2] @trap->purewater_and_torch
	mov  r0, #0xf0
	and  r2, r0
	orr  r2, r1

	mov  r1, #0x31
	strb r2, [r5, r1] @trap->purewater_and_torch
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
