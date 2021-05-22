@ROMUnitとROMClassのAbilityを取得します。
@Get ROMUnit and ROMClass Ability
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr ,r4 ,r5}

ldr r4, =0x030004B0	@Slot0	{J}
@ldr r4, =0x030004B8	@Slot0	{U}

ldr r0, [r4 , #0x4 * 1]	@Slot1

blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
@blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}
cmp  r0,#0x00
beq  Exit            @取得できなかったら終了
mov  r5, r0

ldr  r1, [r5, #0x0] @RAMUnit->Unit
ldr  r0, [r5, #0x4] @RAMUnit->Class
ldr  r2, [r1, #0x28]
ldr  r0, [r0, #0x28]
orr  r0 ,r2

ldr  r1, [r4 , #0x4 * 2]	@Slot2
and  r0 ,r1
cmp  r0 ,r1
bne  FalseExit

TrueExit:
mov  r0, #0x01
b    Exit

FalseExit:
mov  r0, #0x00
@b    Exit

Exit:
str r0, [r4 , #0x4 * 0xC]	@SlotC

pop {r4,r5}
pop {r0}
bx r0
