.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.macro blh_ to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm


ASMC_UseTorch:
	push {r4-r6, lr}

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

	@松明を実行するには座標が必要
	ldr r0, =0x0203A958 @ActionData@gActionData	{U}
	@ldr r0, =0x0203A954 @ActionData@gActionData	{J}
	mov  r1, #0x10
	ldrb r2, [r5, r1]  @Unit->X
	strb r2, [r0, #0x13] @ActionData@gActionData.xOther

	mov  r1, #0x11
	ldrb r2, [r5, r1]  @Unit->Y
	strb r2, [r0, #0x14] @ActionData@gActionData.yOther

	@松明の補正値を上書き
	mov  r0, r5
	add  r0, #0x31
	ldrb r2, [r0, #0x0]
	mov r1, #0x10
	neg r1 ,r1
	and r1 ,r2

	@ターンを取得
	ldr  r3, =0x30004B8	@MemorySlot	{U}
	@ldr  r3, =0x030004B0	@MemorySlot	{J}
	ldr  r3, [r3, #4*0x06] @ s6 as healing amount in r1 
	cmp  r3, #0xf
	ble  WriteTurn
		mov  r3, #0xf   @最大ターンFに自動補正
WriteTurn:
	@ターンの上書き
	orr r1 ,r3
	strb r1, [r0, #0x0]

	mov r0, r5		@Unit
	mov r1, #0x70	@ItemID	0x70 = 松明(Torch)
	blh_  FireAnime

	@アニメーションが終わるまでイベントを待機させる
	ldr r0, WaitForMotionEndProc
	mov r1 ,r4
	blh  0x08002ce0	@NewBlocking6C	@{U}
	@blh  0x08002C30	@NewBlocking6C	@{J}

	ldr r1, =0x89A3874	@MapAnimBattleWithMapEvents	{U}
	@ldr r1, =0x08A13EFC	@MapAnimBattleWithMapEvents	{J}
	str	r1, [r0, #0x2c]

Exit:
	pop  {r4-r6}
	pop  {r0}
	bx   r0



.ltorg
WaitForMotionEndProc:
.equ FireAnime, WaitForMotionEndProc+4
