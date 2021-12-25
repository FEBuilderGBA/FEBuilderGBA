@移動モーションをキャンセルして待機モーションに行こうさせます。

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ASMC_ChangeMoveToWaitMotion:
	push {lr}

	ldr  r3, =0x030004B0	@MemorySlot	{J}
	@ldr  r3, =0x30004B8	@MemorySlot	{U}
	ldr  r0, [r3, #4*0x01] @slot 1 as unit 

	blh  0x0800BF3C   @GetUnitFromEventParam	{J}
	@blh  0x0800BC50   @GetUnitFromEventParam	{U}
	cmp  r0, #0x0
	beq  Exit

	@UnitIDの保存
	mov  r3, r0 @ var r3 = unit

	@UNCR 0x47を実行
	ldr  r0,[r3,#0xC]
	mov  r1, #0x47
	BIC  r0, r1
	str  r0,[r3,#0xC]

	@移動Procsの終了
	blh 0x0807B4B8		@{J} ClearMOVEUNITs
	@blh 0x080790A4		@{U} ClearMOVEUNITs

	blh 0x08032114   @UpdateMapAndUnit	{J}
	@blh 0x080321C8   @UpdateMapAndUnit	{U}

Exit:
	pop  {r0}
	bx   r0
