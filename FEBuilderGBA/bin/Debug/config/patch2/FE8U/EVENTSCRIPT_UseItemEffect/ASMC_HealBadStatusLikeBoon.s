.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ASMC_HealBadStatus:
	push {r4-r5, lr}

	mov  r4, r0 @ var r4 = proc

	ldr  r3, =0x30004B8	@MemorySlot	{U}
	@ldr  r3, =0x030004B0	@MemorySlot	{J}
	ldr  r0, [r3, #4*0x01] @slot 1 as unit 

	blh  0x0800BC50   @GetUnitFromEventParam	{U}
	@blh  0x0800BF3C   @GetUnitFromEventParam	{J}
	cmp  r0, #0x0
	beq  Exit

	@UnitIDの保存
	mov  r5, r0 @ var r5 = unit

	@Boon EFX
	@このエフェクトはちゃんと終了まで待ってくれます
	mov  r0, r5 @ arg1 unit
	mov  r1, r4 @ arg2 this procs
	blh 0x08035DDC	@BeginStatusClearFx__HealBadStatus	{U}
	@blh 0x08035EDC	@BeginStatusClearFx__HealBadStatus	{J}

	@Bad Statusを回復する
	mov  r0, #0x0
	mov  r1, #0x30
	strb r0, [r5, r1]

Exit:
	pop  {r4-r5}
	pop  {r0}
	bx   r0

