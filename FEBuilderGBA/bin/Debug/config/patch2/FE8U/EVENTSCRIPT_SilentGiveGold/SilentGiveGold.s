.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
	push	{r4,r5,lr}
@	ldr  r4,=0x030004B0 @MemorySlot FE8J
	ldr  r4,=0x030004B8 @MemorySlot FE8U
@	ldr  r5,=0x0202BCF4 @Gold FE8J
	ldr  r5,=0x0202BCF8 @Gold FE8U

	ldr  r1, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r0, [r1, #0x2]       @引数1 [FLAG]

	cmp  r0, #0x01
	beq  SubGold
	cmp  r0, #0x02
	beq  SetGold

AddGold:
	ldr  r2,[r4,#0x02 * 4]    @MemorySlot2

	ldr  r0,[r5]    @Current Gold

	add  r0, r2        @add Gold.

AddGold_Main:
	ldr  r3, =999999   @limit Gold
	cmp  r0, r3
	ble  StoreGold
		mov r0,r3      @fix Maximum gold.
	b   StoreGold

SetGold:
	ldr  r2,[r4,#0x02 * 4]    @MemorySlot2

	mov  r0, r2        @set Gold.
	b    AddGold_Main

SubGold:
	ldr  r2,[r4,#0x02 * 4]    @MemorySlot2

	ldr  r0,[r5]    @Current Gold

	cmp  r0, r2
	bge  SubGold_Main
		mov r0, #0x0   @set 0 gold
        b StoreGold
SubGold_Main:
	sub r0, r2
	@b	StoreGold

StoreGold:
	str r0, [r5]
	@b   Term

Term:
	pop	{r4,r5}
	pop	{r1}
	bx	r1
