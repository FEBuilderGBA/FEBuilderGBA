@トラップの存在確認	座標	[TrapID]
@
@40 0D [TrapID] 00 [ASM+1]
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
	push	{r4,lr}

	ldr  r0, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r2, [r0, #0x2]       @引数1 trap id

	ldr  r4, =0x030004DC      @FE8J MemorySlot 0B
@	ldr  r4, =0x030004E4      @FE8U MemorySlot 0B
	ldrh r0, [r4,#0x0]        @MemorySlot 0B -> X
	ldrh r1, [r4,#0x2]        @MemorySlot 0B -> Y

	@r0 x
	@r1 y
	@r2 type
	blh 0x0802e184            @FE8J GetSpecificTrapAt
@	blh 0x0802e24c            @FE8U GetSpecificTrapAt
	cmp r0,#0x00
	beq Store
	
	mov r0,#0x01              @トラップが存在する

Store:
	ldr	r2, =0x030004E0       @FE8J SlotC
@	ldr	r2, =0x030004E8       @FE8U SlotC
	str	r0, [r2, #0x0]

Term:
	mov r0 ,#0x0
	pop {r4}
	pop	{r1}
	bx	r1
