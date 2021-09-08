.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}

FindBattleBGObject:
@ldr r0, =0x0203A610	@TrapData	@{J}
ldr r0, =0x0203A614	@TrapData	@{U}
ldr r3,=0x40*8
add r3,r0				@Term

Loop:
ldrb r1,[r0,#0x2] @Trap->Type
cmp  r1,#0xED     @TrapType 0xED
beq  Found
add  r0, #0x8

cmp  r0,r3
blt  Loop

@見つからないので、新規作成
NotFound:

mov r0 ,#0x0
mov r1 ,#0x0
mov r2 ,#0xED		@TrapType 0xED
mov r3 ,#0x0
@blh 0x0802E1F0		@AddTrap	@{J}
blh 0x0802E2B8		@AddTrap	@{U}

@新規作成に失敗したら即終了
cmp  r0 ,#0x0
beq  Exit

Found:
@ldr  r3, =0x030004B0	@MemorySlot0	@{J}
ldr r3, =0x030004B8	@MemorySlot0	@{U}

Store:
ldr  r2, [r3,#0x1 * 4]	@MemorySlot1
strb r2, [r0,#0x4]		@Trap.Data1 = [MemorySlot1]

Exit:
pop {r1}
bx r1
