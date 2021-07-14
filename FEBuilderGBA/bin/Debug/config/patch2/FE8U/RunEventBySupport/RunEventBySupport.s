@HOOK 08085A5C	{J}
@HOOK 08083724	{U}

@r4 unit1
@r5 unit2
@r6 level

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


@壊すコードの再送
lsl r1 ,r1 ,#0x18
lsr r4 ,r1 ,#0x18

ldr r3, Table
sub r3, #0xC

Loop:
add r3, #0xC
ldr r0, [r3]
cmp r0, #0xff
beq NotFound

CheckLevel:
ldrb r0, [r3,#0x2]
cmp  r6, r0
bne  Loop

CheckUnit1:
ldrb r0, [r3,#0x0]
cmp  r4, r0
beq  CheckUnit2_A

cmp  r5, r0
bne  Loop

CheckUnit2_B:
ldrb r0, [r3,#0x1]
cmp  r4, r0
bne  Loop
b    Match

CheckUnit2_A:
ldrb r0, [r3,#0x1]
cmp  r5, r0
bne  Loop
@b    Match

Match:
ldr  r0, [r3,#0x4]
cmp  r0, #0x0
beq  Loop

RunLevel:
mov r1, #0x1
@blh 0x0800d340   @イベント命令を動作させる関数 r0=イベント命令ポインタ:POINTER_EVENT r1=引数?1-3	{J}
blh 0x0800d07c   @イベント命令を動作させる関数 r0=イベント命令ポインタ:POINTER_EVENT r1=引数?1-3	{U}

@ldr r3, =0x08085A8C|1	@{J}
ldr r3, =0x08083754|1	@{U}
bx r3

NotFound:
mov r0 ,r5
mov r1 ,r4

@ldr r3, =0x8085A64|1	@{J}
ldr r3, =0x0808372C|1	@{U}
bx r3

.ltorg
Table:
