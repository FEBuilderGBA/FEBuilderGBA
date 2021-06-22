@HOOK 2CEF4	{J}
@HOOK 2CFBC	{U}
@
@トライアングルアタックをイベントバトルからでも有効にします
@このパッチをインストールした後で、イベントバトルの攻撃方法で、TirnagleAtack(require patch)にチェックしてください。
@そして、ペガサスかファルコン3体でユニットを囲んで、トライアングルアタックの条件を満たしてください。
@
@トライアングルアタック開始のセリフが不要の場合は、0x7F-0x81を、事前にONにしてください。
@
@Enable Triangle Attack even from Event Battles
@After installing this patch, check "TirnagleAtack(require patch)" in the attack method of Event Battle.
@Then, surround the unit with three Pegasus or Falcons to fulfill the conditions for the Triangle Attack.
@
@If you don't need the line to start the triangle attack, please turn on 0x7F-0x81 beforehand.
@

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@r6 currentRound

@壊すコードの再送
mov r0 ,r4
mov r1 ,r5
blh 0x0802b134   @UpdateBattleStats	@{J}
@blh 0x0802B1C4   @UpdateBattleStats	@{U}

@念のため、明確に指定されていない限り分岐しないようにする
ldr  r0, [r6]         @currentRound
ldrb r0, [r0, #0x1]   @currentRound->BattleRound@gRoundArray.Option2
mov  r1, #0x4         @TriangleAttack
and  r0, r1
cmp  r0, #0x0
beq  Exit

mov r0 ,r4
mov r1 ,r5
blh 0x0802b4cc   @UpdateBattleTriangleAttackData	{J}
@blh 0x0802B578   @UpdateBattleTriangleAttackData	{U}

Exit:
ldr r3,=0x0802CEFC|1	@{J}
@ldr r3,=0x0802CFC4|1	@{U}
bx  r3



