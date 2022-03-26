@
@クリアターンの記録を全部リセットする
@
@

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

push {r4,r5,r6,r7,lr}

	ldr  r4 , =0x0203ECF0     @ ClearTurn	{J}
@	ldr  r4 , =0x0203ECF4     @ ClearTurn	{U}

	mov r5, #0x24 * 4
	add r5, r4

	ldr  r3, =0x030004B0	@MemorySlot	{J}
	@ldr  r3, =0x30004B8	@MemorySlot	{U}
	ldrb r6, [r3, #4*0x01]  @slot 1 as unit 

	ldr  r3, =0x80a8bac  @sizeof(SaveTurn)
	ldrb r7, [r3]

	sub r4, r7  @事前に引いておく
Loop:
	add r4, r7  @次のデータへ
	cmp r4, r5
	bge Exit

	ldrb r0, [r4]
	mov  r1, #0x7f
	and  r0, r1
	
	cmp  r0, r6
	bne  Loop

	@これ以降を消去する
	mov r0, r4
	mov r1, #0x0
	sub r2, r5, r4
	blh 0x080d6968   @memset	{J}
@	blh 0x080d1c6c   @memset	{U}

Exit:

pop {r4,r5,r6,r7}
pop {r1}
mov pc,r1
