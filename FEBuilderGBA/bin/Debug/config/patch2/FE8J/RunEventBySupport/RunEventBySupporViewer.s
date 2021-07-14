@HOOK 08085AA0	{J}
@HOOK 08083768	{U}

@r4 level	keep
@r0 unit2	keep
@r1 level	keep

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@壊すコードの再送
lsl r0 ,r0 ,#0x18
lsr r0 ,r0 ,#0x18
lsl r1 ,r1 ,#0x18
lsr r1 ,r1 ,#0x18

ldr r3, Table
sub r3, #0xC

Loop:
add r3, #0xC
ldr r2, [r3]
cmp r2, #0xff
beq NotFound

CheckLevel:
ldrb r2, [r3,#0x2]
cmp  r4, r2
bne  Loop

CheckUnit1:
ldrb r2, [r3,#0x0]
cmp  r0, r2
beq  CheckUnit2_A

cmp  r1, r2
bne  Loop

CheckUnit2_B:
ldrb r2, [r3,#0x1]
cmp  r0, r2
bne  Loop
b    Match

CheckUnit2_A:
ldrb r2, [r3,#0x1]
cmp  r1, r2
bne  Loop
@b    Match

Match:
ldr  r2, [r3,#0x8]
cmp  r2, #0x0
beq  Loop

RunLevel:
mov r0, r2
mov r1, #0x3
blh 0x0800d340   @イベント命令を動作させる関数 r0=イベント命令ポインタ:POINTER_EVENT r1=引数?1-3	{J}
@blh 0x0800d07c   @イベント命令を動作させる関数 r0=イベント命令ポインタ:POINTER_EVENT r1=引数?1-3	{U}

ldr r3, =0x08085AC0|1	@{J}
@ldr r3, =0x08083788|1	@{U}
bx r3

NotFound:
mov r2 ,r4

ldr r3, =0x08085AA8|1	@{J}
@ldr r3, =0x08083770|1	@{U}
bx r3

.ltorg
Table:
