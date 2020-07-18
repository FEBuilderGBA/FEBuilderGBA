@Call 0x89810	{J}
@Call 0x875A8	{U}
@keep r0
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


mov r1 ,r5	@{J}
@mov r1 ,r6	@{U}
mov r3 ,#0x60
ldsh r2, [r1, r3]	@BattleUnit@gBattleActor.battleHitRate

cmp  r2, #0xff
bne  display_number

@攻撃力がないならbar
mov r1 ,r5	@{J}
@mov r1 ,r6	@{U}
mov r3 ,#0x5a
ldsh r3, [r1, r3]	@BattleUnit@gBattleActor.battleAttack
cmp r3, #0xff
beq display_bar
cmp r3, #0x1
blt display_bar

display_number:
mov r1, #0x2
blh 0x08004a90   @DrawUiNumber	@{J}
@blh 0x08004B88   @DrawUiNumber	@{U}

b Exit

display_bar:
mov r1, #0x2
@mov r2, #0xff
blh 0x08004A9C @DrawDecNumber	@{J}
@blh 0x08004B94 @DrawDecNumber	@{U}

Exit:
ldr r3,=0x0808981A|1	@{J}
@ldr r3,=0x080875B2|1	@{U}
bx r3
