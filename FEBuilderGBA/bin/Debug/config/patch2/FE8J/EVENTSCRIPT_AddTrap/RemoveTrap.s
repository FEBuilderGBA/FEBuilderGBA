@トラップの削除	座標	[TrapID]
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
	beq Term

	ldrb r2,[r0,#0x02]        @trap->type
	cmp  r2,#0x0D
	beq  RemoveLightRune

	blh 0x0802e234            @FE8J RemoveTrap
@	blh 0x0802e2fc            @FE8U RemoveTrap
	b    Join

RemoveLightRune:
	blh 0x0802E9C8            @FE8J RemoveLightRune
@	blh 0x0802ea90            @FE8U RemoveLightRune
@	b    Join

Join:
Term:
	mov r0 ,#0x0
	pop {r4}
	pop	{r1}
	bx	r1

