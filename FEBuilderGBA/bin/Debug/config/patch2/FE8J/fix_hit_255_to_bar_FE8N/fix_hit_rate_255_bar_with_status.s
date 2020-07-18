@Call 0x89800	{FE8N J}
@keep r0
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@mov r0, r4
add r0, #0x82

mov r1 ,r5	@{J}
mov r3 ,#0x60
ldsh r2, [r1, r3]	@BattleUnit@gBattleActor.battleHitRate

cmp  r2, #0xff
bne  display_number

@攻撃力がないならbar
mov r1 ,r5	@{J}
mov r3 ,#0x5a
ldsh r3, [r1, r3]	@BattleUnit@gBattleActor.battleAttack
cmp r3, #0xff
beq display_bar
cmp r3, #0x1
blt display_bar

display_number:
mov r1, #0x2
blh 0x08004a90   @DrawUiNumber	@{J}

b Exit

display_bar:
mov r1, #0x2
@mov r2, #0xff
blh 0x08004A9C @DrawDecNumber	@{J}

Exit:
ldr r3,=0x08089808|1	@{FE8N J}
bx r3
