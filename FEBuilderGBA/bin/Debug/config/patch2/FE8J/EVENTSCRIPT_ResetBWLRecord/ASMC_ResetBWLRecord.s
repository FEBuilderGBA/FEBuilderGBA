@
@プレイヤーユニットの勝敗記録をリセットします
@
@

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

push {lr}

	ldr  r3, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

	ldrb r0, [r3, #0x2]       @引数1 [UNIT] dest unit
	cmp  r0,#0x46             @BWLテーブルは 0x45まで
	bge  Term
	cmp  r0,#0x00             @UnitID 0x00 は nullなので無視する.
	beq  Term

	sub  r0 ,r0 ,#0x1         @ unit_id -= 1;

	lsl  r0 ,r0 ,#0x4         @ unit_id * 16
	ldr  r3 , =0x0203E890     @ CharacterBattleStatistic	{J}
@	ldr  r3 , =0x0203E894     @ CharacterBattleStatistic	{U}
	add  r3 , r0

	mov  r0 , #0x00
	str  r0 , [r3 , #0x0]
	str  r0 , [r3 , #0x4]
	str  r0 , [r3 , #0x8]
	str  r0 , [r3 , #0xC]

	mov  r0 , #0x20
	strb r0 , [r3 , #0x2]    @ 00 00 20 00  00 00 00 00  00 00 00 00  00 00 00 00

Term:

pop {r1}
mov pc,r1
