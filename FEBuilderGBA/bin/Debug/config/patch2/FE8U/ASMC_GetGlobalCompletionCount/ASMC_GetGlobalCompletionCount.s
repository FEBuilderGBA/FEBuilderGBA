.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}
@blh 0x080A95F4	@GetGlobalCompletionCount	@{J}
blh 0x080a4bb0	@GetGlobalCompletionCount	@{U}

@ldr r1, =0x030004B0	@MemorySlot		@{J}
ldr r1, =0x030004B8	@MemorySlot		@{U}
str r0, [r1, #0xC * 4]	@SlotC

pop {r0}
bx r0
