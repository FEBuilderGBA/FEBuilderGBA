.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

HealLikeVulnerary:
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

	@アニメーションを表示するので、一時的にマップ上の該当ユニットを消す
	mov  r0, r5	 @ arg r0 = Unit
	blh  0x0802810c   @HideUnitSMS	{U}
	@blh  0x080280A0   @HideUnitSMS	{J}

	@回復量を取得
	ldr  r3, =0x30004B8	@MemorySlot	{U}
	@ldr  r3, =0x030004B0	@MemorySlot	{J}
	ldr  r1, [r3, #4*0x06] @ s6 as healing amount in r1 
	mov  r0 ,r5       @arg1: Unit
	                  @arg2: Heal value
	blh  0x08035804   @ExecFortSelfHealMotion	{U}
	@blh  0x08035904   @ExecFortSelfHealMotion	{J}

	@アニメーションが終わるまでイベントを待機させる
	ldr r0, WaitForMotionEndProc
	mov r1 ,r4
	blh  0x08002ce0	@NewBlocking6C	@{U}
	@blh  0x08002C30	@NewBlocking6C	@{J}

	ldr r1, =0x89A3874	@MapAnimBattleWithMapEvents	{U}
	@ldr r1, =0x08A13EFC	@MapAnimBattleWithMapEvents	{J}
	str	r1, [r0, #0x2c]

Exit:
	pop  {r4-r5}
	pop  {r0}
	bx   r0

.ltorg
WaitForMotionEndProc:
