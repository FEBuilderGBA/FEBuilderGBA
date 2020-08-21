@GetGameTotalTurnCount
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

push {lr}


@blh 0x080A8E1C	@GetGameTotalTurnCount	{J}
blh 0x080A43D8	@GetGameTotalTurnCount	{U}

@ldr  r3,=0x030004B0 @MemorySlot FE8J
ldr  r3,=0x030004B8 @MemorySlot FE8U
str  r0,[r3,#0x0C * 4]    @MemorySlotC (Result Value)

pop {r1}
bx r1

