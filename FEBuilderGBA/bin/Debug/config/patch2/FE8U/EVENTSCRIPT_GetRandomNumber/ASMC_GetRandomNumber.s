@GetRandomNumber
@
@
@Author 7743
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

push {r4,lr}

@ldr  r3,=0x030004B0 @MemorySlot FE8J
ldr  r3,=0x030004B8 @MemorySlot FE8U
ldr r0, [r3, #0x01 * 0x4]	@Slot1
ldr r1, [r3, #0x02 * 0x4]	@Slot2
cmp r0, r1
bge Error

cmp r0, #0x0
beq Error

mov r4, r0

sub r0, r1, r0
add r0,#0x01

cmp r0, #0x0  @オーバーフローしたら嫌なのでもう一度確認する
beq Error

@blh 0x08000c58   @NextRN_N	@{J}
blh 0x08000c80   @NextRN_N	@{U}

add r0, r4
b Exit

Error:
mov r0,#0x0

Exit:
@ldr  r3,=0x030004B0 @MemorySlot FE8J
ldr  r3,=0x030004B8 @MemorySlot FE8U
str  r0,[r3,#0x0C * 4]    @MemorySlotC (Result Value)

pop {r4}
pop {r1}
bx r1
