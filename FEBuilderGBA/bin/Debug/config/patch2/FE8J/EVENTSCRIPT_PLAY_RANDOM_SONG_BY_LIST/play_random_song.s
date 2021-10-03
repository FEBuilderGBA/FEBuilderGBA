.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}

@まずは件数を数える
mov r0, #0x0  @counter
ldr	r3, =0x030004B0 + 0x4 * 0x1	@MemorySlot 1 @{J}
@ldr	r3, =0x030004B8 + 0x4 * 0x1	@MemorySlot 1 @{U}

Loop:
ldrh r1, [r3]
cmp  r1, #0x0
beq  ramdom_select
add  r0, #0x1
add  r3, #0x2
b    Loop

ramdom_select:
blh 0x08000c58	@{J} NextRN_N
@blh 0x08000C80	@{U} NextRN_N

lsl r0, #0x1  @ *2
ldr	r3, =0x030004B0 + 0x4 * 0x1	@MemorySlot 1 @{J}
@ldr	r3, =0x030004B8 + 0x4 * 0x1	@MemorySlot 1 @{U}
ldrh r0, [r3, r0]

ldr	r3, =0x030004B0 + 0x4 * 0xC	@MemorySlot C @{J}
@ldr	r3, =0x030004B8 + 0x4 * 0xC	@MemorySlot C @{U}
str r0, [r3]

mov r1, #0x0
blh 0x08002424   @BGMを切り替える(最上位) r0=BGM番号:MUSIC r1=不明	{J}
@blh 0x080024D4   @BGMを切り替える(最上位) r0=BGM番号:MUSIC r1=不明	{U}

pop {r0}
bx r0
