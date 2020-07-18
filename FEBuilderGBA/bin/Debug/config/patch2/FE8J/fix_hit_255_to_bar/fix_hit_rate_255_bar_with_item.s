@Call 0x1E5E0	{J}
@Call 0x1E980	{U}
@keep r0
.thumb
.macro blh to, reg=r3
  push {\reg}
  ldr \reg, =\to
  mov lr, \reg
  pop {\reg}
  .short 0xf800
.endm


mov r1 ,r8
mov r2 ,#0x60
ldsh r3, [r1, r2]	@BattleUnit@gBattleActor.battleHitRate

cmp  r3, #0xff
bne  display_number

@攻撃力がないならbar
mov r1 ,r8
mov r2 ,#0x5a
ldsh r2, [r1, r2]	@BattleUnit@gBattleActor.battleAttack
cmp r2, #0xff
beq display_bar
cmp r2, #0x1
blt display_bar

display_number:
mov r0 ,r4
mov r1, #0x24
mov r2, r9

push {r4,r5,r6}

mov r4 ,r0
mov r5 ,r2
mov r6 ,r3
blh 0x08003d84   @Text_SetXCursor	@{J}
@blh 0x08003E54   @Text_SetXCursor	@{U}

mov r0 ,r4
mov r1 ,r5
blh 0x08003d90   @Text_SetColorId	@{J}
@blh 0x08003E60   @Text_SetColorId	@{U}

mov r0 ,r4
mov r1 ,r6
blh 0x08003f98   @Text_AppendDecNumber	@{J}
@blh 0x08004074   @Text_AppendDecNumber	@{U}

pop {r4,r5,r6}

b Exit

display_bar:
mov r0 ,r4
mov r1, #0x24
mov r2, r9
#mov r3, #0xff
blh 0x080043dc   @Text_InsertNumberOr2Dashes	@{J}
@blh 0x080044A4   @Text_InsertNumberOr2Dashes	@{U}

Exit:
ldr r3,=0x0801E5EC|1	@{J}
@ldr r3,=0x0801E98C|1	@{U}
bx r3
