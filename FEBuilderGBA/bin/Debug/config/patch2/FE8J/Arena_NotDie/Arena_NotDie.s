@闘技場で死なない
@FE4みたいにHP1で放り出します
@HOOK 080328D0	@{J}
@HOOK 08032984	@{U}
@r2 this keep
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r2}
lsl r0 ,r0 ,#0x18
asr r0 ,r0 ,#0x18
cmp r0, #0x0
bne Exit

@死んでしまうので止めます
ldr r0, =0x0203A4E8 @BattleUnit@gBattleActor	@{J}
@ldr r0, =0x0203A4EC @BattleUnit@gBattleActor	@{U}
mov r1, #0x1
strb r1, [r0, #0x13] @BattleUnit@gBattleActor.CurrentHP = 1

@カレントユニットのHPを1にする
ldr r0, =0x03004DF0	@Current	@{J}
@ldr r0, =0x03004E50	@Current	@{U}
ldr r0, [r0]
strb r1, [r0, #0x13] @Current->CurrentHP = 1

@ロードが死んでいる可能性があるのでフラグ0x65をキャンセルする
mov r0, #0x65
blh 0x080860bc	@ResetFlag	@{J}
@blh 0x08083d94	@ResetFlag	@{U}

Exit:
pop {r2}
ldr r3, =0x080328D8|1	@{J}
@ldr r3, =0x0803298C|1	@{U}
bx r3
