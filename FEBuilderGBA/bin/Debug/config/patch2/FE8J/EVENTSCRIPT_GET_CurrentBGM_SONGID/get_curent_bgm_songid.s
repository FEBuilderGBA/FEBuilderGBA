@現在再生しているBGMのSongIDをSlotCに返します
@
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}

blh 0x080021a8   @再生しているBGMの取得	{J}
@blh 0x08002258   @再生しているBGMの取得	{U}

ldr	r2, =0x030004B0 + 0x4 * 0xC	@MemorySlot C @{J}
@ldr	r2, =0x030004B8 + 0x4 * 0xC	@MemorySlot C @{U}
str r0, [r2]

pop {r0}
bx r0
