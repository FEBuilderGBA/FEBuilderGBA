@Hook 2BE96	{J}
@Hook 2BF4E {U}
.thumb

@r0  work
@r1  work
@r2  ramunit         keep
@r3  work levelupHP  keep
@r4
@r5


ldr  r0, [r2, #0x4] @class

@ここにはプレイヤーかNPCしか来ないことが確約されている
PlayerOrNPC:
mov   r1,#0x13
add   r1,r1,r0       @Class HP Caps

Join:
ldrb  r1,[r1]        @HPCaps

cmp  r3, r1
ble  NormalGrows

@上限値を超えないように補正するルーチンに戻す
FixGrows:
mov r0, r1  @HPCaps
mov r3, #0x12
ldsb r3, [r2, r3]  @CurrentMAXHP

@ldr r1, =0x0802BEAE+1	@{J}
ldr r1, =0x0802BF66+1	@{U}
bx  r1

@通常の成長をするルーチンに戻す
NormalGrows:
@ldr r1, =0x0802beb6+1	@{J}
ldr r1, =0x0802bf6e+1	@{U}
bx  r1
