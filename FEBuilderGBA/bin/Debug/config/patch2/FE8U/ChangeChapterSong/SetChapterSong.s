.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}

FindBGMObject:
@ldr r0, =0x0203A610	@TrapData	@{J}
ldr r0, =0x0203A614	@TrapData	@{U}
ldr r3,=0x40*8
add r3,r0				@Term

Loop:
ldrb r1,[r0,#0x2] @Trap->Type
cmp  r1,#0xEE     @TrapType 0xEE
beq  Found
add  r0, #0x8

cmp  r0,r3
blt  Loop

@見つからないので、新規作成
NotFound:

mov r0 ,#0x0
mov r1 ,#0x0
mov r2 ,#0xEE		@TrapType 0xEE
mov r3 ,#0x0
@blh 0x0802E1F0		@AddTrap	@{J}
blh 0x0802E2B8		@AddTrap	@{U}

@新規作成に失敗したら即終了
cmp  r0 ,#0x0
beq  Exit

Found:

@ldr  r3, =0x030004B0	@MemorySlot0	@{J}
ldr r3, =0x030004B8	@MemorySlot0	@{U}

StorePlayerBGM:
ldr  r2, [r3,#0x1 * 4]	@MemorySlot1

cmp  r2, #0x0
beq  StoreEnemyBGM
strh r2,[r0,#0x4]		@Trap.Data1 = [MemorySlot1]

StoreEnemyBGM:
ldr r2, [r3,#0x2 * 4]	@MemorySlot2

cmp  r2, #0x0
beq  StoreNPCBGM
strh r2,[r0,#0x6]		@Trap.Data2 = [MemorySlot2]

StoreNPCBGM:
ldr r2, [r3,#0x3 * 4]	@MemorySlot3

cmp  r2, #0x0
beq  StoreFlag
strh r2,[r0,#0x0]		@Trap.XY = [MemorySlot3]

StoreFlag:
ldr  r2, [r3,#0x4 * 4]	@MemorySlot4
cmp  r2, #0x2
bge  Exit

strb r2,[r0,#0x3]		@Trap.Data0 = [MemorySlot4]

Exit:
pop {r1}
bx r1
