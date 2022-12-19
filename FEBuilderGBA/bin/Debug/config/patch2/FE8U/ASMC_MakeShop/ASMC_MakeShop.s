.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Slot1 UnitID
@Slot3 Type 0=武器屋 1=道具屋 2=秘密の店
@Slot4 売り物リストPoinetr

ASMC_MakeShop:
push {r4,r5, lr}
mov  r4, r0 @ this procs
ldr  r5, =0x30004B8	@MemorySlot	{U}
@ldr  r5, =0x030004B0	@MemorySlot	{J}

ldr  r0, [r5, #4 * 0x01] @slot 1 as unit 
blh  0x0800BC50   @GetUnitFromEventParam	{U}
@blh  0x0800BF3C   @GetUnitFromEventParam	{J}
cmp  r0, #0x0
beq  Exit

ldrb r2, [r5, #4 * 0x3]	@Slot3	@StoreType
ldr  r1, [r5, #4 * 0x4]	@Slot4  @売り物リスト
mov  r3, r4   @this procs

blh 0x080b4240,r4   @MakeShop	{U}
@blh 0x080b8dc8,r4   @MakeShop	{J}
Exit:
pop {r4,r5}
pop {r0}
bx r0
