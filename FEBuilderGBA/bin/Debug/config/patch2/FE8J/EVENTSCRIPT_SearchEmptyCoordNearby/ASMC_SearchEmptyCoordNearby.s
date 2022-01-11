@近くの空いている場所を探索
@Search empty coord nearby
@
@Slot1 Class ID(このクラスが移動できないタイルは除外)
@SlotB Base position
@

@この関数はスクラッチレジスタr12を使います.

.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
	push {r4,r5,r6,r7,lr}
	mov r7, r11
	mov r6, r10
	mov r5, r9
	mov r4, r8
	push {r4,r5,r6,r7}

	mov  r0, #0x0
	mov  r10, r0			@best XY

	ldr  r3, =0x030004B0	@MemorySlot	{J}
@	ldr  r3, =0x030004B8	@MemorySlot	{U}
	ldrh r0, [r3, #4*0x0B] @slot B as unit 
	mov  r8, r0		@base X
	ldrh r0, [r3, #4*0x0B+2] @slot B as unit 
	mov  r9, r0		@base Y

	GetMoveTable:
	ldrb r0, [r3, #4*0x1]	@Slot 1 as class id
	blh  0x0801911C	@GetROMClassStruct RET=class data table:CLASS r0=class_id	{J}
@	blh  0x08019444	@GetROMClassStruct RET=class data table:CLASS r0=class_id	{U}
	cmp  r0, #0x0
	beq  Break
	ldr  r1, [r0, #0x38]
	cmp  r1, #0x0
	beq  Break
	mov  r11, r1			@MoveTable

	ldr  r0, =0x0202E4D4	@gMapUnit	@{J}
@	ldr  r0, =0x0202E4D8	@gMapUnit	@{U}
	ldr  r6, [r0]

	ldr  r0, =0x0202E4D8	@gMapTerrain	@{J}
@	ldr  r0, =0x0202E4DC	@gMapTerrain	@{U}
	ldr  r7, [r0]

@現在地点をチェックする、ほとんどのケースは空いていると思われるためです
		CurrentCheckEmpty:
		mov  r4, r8			@X
		mov  r5, r9			@Y
		lsl  r2, r5, #0x2	@y*2	@キャッシュへ移動
		ldr  r1, [r6, r2]
		ldrb r1, [r1, r4]	@gMapUnit[Y][X]
		cmp  r1, #0x0
		bne  LoopStart			@Not Empty
		
		CurrentCheckTile:
		ldr  r1, [r7, r2]
		ldrb r1, [r1, r4]	@tileID = gMapTerrain[Y][X]
		add  r1, r11
		ldrb r1, [r1]		@movCost = Class->MoveCostPtr[tileID]
		cmp  r1, #0x20
		bge  LoopStart			@Not Move
	
		lsl r0, r5, #0x10   @bestXY
		add r0, r4
		mov r10, r0
		b   Break

@ループ開始。一番近い空き位置を探す
LoopStart:
	ldr  r0, =9999			@max distance
	mov  r12, r0

	ldr  r0, =0x0202E4D0	@gMapSize	@{J}
@	ldr  r0, =0x0202E4D4	@gMapSize	@{U}
	ldrh r5, [r0, #0x2]		@gMapSize.Heiht
LoopY:
	cmp  r5, #0x0
	ble  Break
	sub  r5, #0x1

	@y座標はx loopが回っている間は固定値なので計算結果をキャッシュしておきます。
		disY:
		mov  r3, r9
		cmp  r3, r5
		bge  disY2
			 sub r3, r5, r3
			 b   disYJoin
		disY2:
			 sub r3, r3, r5
		disYJoin:

		lsl  r2, r5, #0x2	@y*2

	ldr  r0, =0x0202E4D0	@gMapSize	@{J}
@	ldr  r0, =0x0202E4D4	@gMapSize	@{U}
	ldrh r4, [r0, #0x0]		@gMapSize.Width

	LoopX:
		cmp  r4, #0x0
		ble  LoopY
		sub  r4, #0x1

		@r0とr1だけ空いている

		CheckEmpty:
@@@		lsl  r2, r5, #0x2	@y*2	@キャッシュへ移動
		ldr  r1, [r6, r2]
		ldrb r1, [r1, r4]	@gMapUnit[Y][X]
		cmp  r1, #0x0
		bne  LoopX			@Not Empty
		
		CheckTile:
		ldr  r1, [r7, r2]
		ldrb r1, [r1, r4]	@tileID = gMapTerrain[Y][X]
		add  r1, r11
		ldrb r1, [r1]		@movCost = Class->MoveCostPtr[tileID]
		cmp  r1, #0x20
		bge  LoopX			@Not Move

		@distance=abs(basex-x)+abs(basey-y)
		disX:
		mov  r0, r8
		cmp  r0, r4
		bge  disX2
			 sub r0, r4, r0
			 b   checkDistance
		disX2:
			 sub r0, r0, r4

		checkDistance:
		add r0, r3
		cmp r0, r12
		bge LoopX

		mov r12, r0			@update best distance
		lsl r0, r5, #0x10   @bestXY
		add r0, r4
		mov r10, r0
		b   LoopX

Break:
	mov  r0, r10
	ldr  r3, =0x030004B0	@MemorySlot	{J}
@	ldr  r3, =0x030004B8	@MemorySlot	{U}
	str  r0, [r3, #4*0x0C] @slot C as BestXY

	pop {r0,r1,r2,r3,r4,r5,r6,r7}
	mov r8, r0
	mov r9, r1
	mov r10, r2
	mov r11, r3
	pop {r1}
	bx r1
