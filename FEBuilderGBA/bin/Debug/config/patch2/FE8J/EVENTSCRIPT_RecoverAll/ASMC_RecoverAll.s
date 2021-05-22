@全員を回復します
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr ,r4 ,r5 ,r6}

ldr r4, =0x030004B0	@Slot0	{J}
@ldr r4, =0x030004B8	@Slot0	{U}

ldr r0, [r4 , #0x4 * 1]	@Slot1 	00 is Player(blue), 01 Enemy(red), 02 NPC (green).
cmp r0, #0x1
beq SelectEnemy
cmp r0, #0x2
beq SelectNPC

@それ以外はPlayer
SelectPlayer:
mov r0, #0x0
b LoopStart

SelectNPC:
mov r0, #0x40
b LoopStart

SelectEnemy:
mov r0, #0x80
@b LoopStart

LoopStart:
mov r5, r0
mov r6, #0x40  @End
add r6, r0

Loop:

mov r0, r5
blh 0x08019108	@GetUnitStruct	@{J}
@blh 0x08019430	@GetUnitStruct	@{U}

ldr r1, [r0 , #0x0]
cmp r1, #0x0		@IsEmpty?
beq Continue

@死んでる人は無視
strb r1, [r0 , #0x13] @RAMUnit->CurrentHP
cmp  r1, #0x0
beq Continue

@回復
ldrb r1, [r0 , #0x12] @RAMUnit->MaxHP
strb r1, [r0 , #0x13] @RAMUnit->CurrentHP

@バットステータスの回復
mov  r1, #0x0
mov  r2, #0x30
strb r1, [r0 , r2] @RAMUnit->Status

Continue:
add r5, #0x1
cmp r5, r6
blt Loop

Exit:
	blh  0x08019ecc   @RefreshFogAndUnitMaps	@{J}
	blh  0x08027144   @SMS_UpdateFromGameData	@{J}
	blh  0x08019914   @UpdateGameTilesGraphics	@{J}
@	blh  0x0801a1f4   @RefreshFogAndUnitMaps	@{U}
@	blh  0x080271a0   @SMS_UpdateFromGameData	@{U}
@	blh  0x08019c3c   @UpdateGameTilesGraphics	@{U}
pop {r4,r5,r6}
pop {r0}
bx r0
