@HOOK	0800EA48	{J}
@HOOK	0800E838	{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

CountBGList:
ldr  r6, BGList
mov  r0, #0x0
Loop:
ldrb r3, [r6, r0]
cmp  r3, #0xFF
beq  ChooseRandBG
add  r0, #0x01
b    Loop

ChooseRandBG:
blh 0x08000c58   @NextRN_N	{J}
@blh 0x08000c80   @NextRN_N	{U}
ldrb r6, [r6, r0]

ldr r3, =0x0800EA52|1	@{J}
@ldr r3, =0x0800E842|1	@{U}
bx r3

.ltorg
BGList:
