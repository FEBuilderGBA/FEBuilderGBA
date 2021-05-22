.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Hook 08085A88	{J}
@Hook 08083750	{U}
@
@r0 MusicID
@r1 TextID
@r4 unit1
@r5 unit2
@r6 level

push {r6, r7}
mov  r7, r8
push {r7}
mov  r8, r0
mov  r7, r1

ldr  r3, Table
sub  r3, #0x8
Loop:
add  r3, #0x8

ldr  r0, [r3,#0x0]
cmp  r0, #0xFF
beq  NotFound

@CheckLevel:
@ldrb r0, [r3,#0x2] @Table->Level
@cmp  r0, r6
@bne  Loop

CheckUnit1:
mov  r2, r4
lsl  r2, r2 , #0x8
add  r2, r5

ldrh r0, [r3,#0x0] @Table->Unit1 and Table->Unit2
cmp  r0, r2
beq  Match

CheckUnit2:
mov  r2, r5
lsl  r2, r2 , #0x8
add  r2, r4

ldrh r0, [r3,#0x0] @Table->Unit1 and Table->Unit2
cmp  r0, r2
bne  Loop

Match:
ldr  r0, [r3,#0x4] @Table->Event
cmp  r0, #0x01
ble  NotFound
b    RunEvent

NotFound:
ldr  r0, =0x0800D5EC		@Default Event Pointer	@{J}
@ldr  r0, =0x0800D328		@Default Event Pointer	@{U}
ldr  r0, [r0]

RunEvent:
mov  r1, #0x1
blh  0x0800d340   @イベント命令を動作させる関数	@{J}
@blh  0x0800d07c   @イベント命令を動作させる関数	@{U}
ldr  r0, =0x030004B0 @Slot0	@{J}
@ldr  r0, =0x030004B8 @Slot0	@{U}

mov  r3, r8
str  r3, [r0, #0x4 * 2]   @MemorySlot02	@MusicID
str  r7, [r0, #0x4 * 3]   @MemorySlot03	@TextID
str  r6, [r0, #0x4 * 4]   @MemorySlot04	@SupportLevel

Exit:
mov r2, r8
pop {r7}
mov r8, r7
pop {r6, r7}
@壊すコードの再送
mov r0 ,r5
mov r1 ,r4

ldr r3 ,=0x08085A92|1		@{J}
@ldr r3 ,=0x0808375A|1		@{U}
bx  r3

.ltorg
Table:
@Unit1	FF
@Unit2	FF
@00		00
@Event	0
