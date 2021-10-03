@Slot1で指定したBGMの曲名のTextIDをSlotCに返します。
@Slot1にFFFFを指定すると現在再生しているBGMを参照します。
@
@曲名はサウンドルームを利用します。
@その曲がサウンドルームに登録されていない場合は、SlotCには0が返ります
@
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}

ldr	r2, =0x030004B0 + 0x4 * 0x1	@MemorySlot C @{J}
@ldr	r2, =0x030004B8 + 0x4 * 0x1	@MemorySlot C @{U}
ldrh r0,[r2]
ldr  r1,=0xffff
cmp  r0,r1
bne  GetName

blh 0x080021a8   @再生しているBGMの取得	{J}
@blh 0x08002258   @再生しているBGMの取得	{U}

GetName:
cmp r0, #0x0
beq NotFound

ldr r3,=0x080B5044	@SoundroomPointer	{J}
@ldr r3,=0x0801BC14	@SoundroomPointer	{U}
ldr r3,[r3]

ldr  r2,=0xffff
Loop:
ldrh r1, [r3]
cmp  r0, r1
beq  Found

cmp  r1, r2   @r2=0xffff
beq  NotFound

add r3,#0x10
b   Loop

Found:
ldrh r0, [r3, #0xC]
b    Exit

NotFound:
mov r0, #0x0

Exit:
ldr	r2, =0x030004B0 + 0x4 * 0xC	@MemorySlot C @{J}
@ldr	r2, =0x030004B8 + 0x4 * 0xC	@MemorySlot C @{U}
str r0, [r2]

pop {r0}
bx r0
