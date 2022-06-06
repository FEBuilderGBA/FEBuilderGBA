@ORG 5792C {J}
@ 毒状態のユニットで、リザーブやラトナなどの全体回復を使った後で、 DeleteEach6C_efxStatusUnit を発行していないバグを修正します
@ efxStatusUnit procs が残った状態になり、顔画像が毒の色に染まってしまうバグが発生します。
@ これはFE8Jでのみ発生するバグです。 FE8Uでは既に修正されています
@

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@壊すコードの再送
ldr r0, [r5, #0x0] @02000000 (WRAM )
cmp r0, #0x0
beq Skip
	blh 0x08055800	@{J}

Skip:

@DeleteEach6C_efxStatusUnitを発行する
@DeleteEach6C_efxStatusUnitはFE8Jにはないので、同様の機能である EndEfxStatusUnitで代用します
blh 0x080752A4	@EndEfxStatusUnit	@{J}

@元に戻す
ldr r3, =0x08057936|1	@{J}
bx r3
