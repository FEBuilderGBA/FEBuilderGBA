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

push {lr}

@	ldr  r0 , =0x0203ECF0     @ ClearTurn	{J}
	ldr  r0 , =0x0203ECF4     @ ClearTurn	{U}

	mov r1, #0x0
	mov r2, #0x24 * 4
@	blh 0x080d6968   @memset	{J}
	blh 0x080d1c6c   @memset	{U}

Term:

pop {r1}
mov pc,r1
