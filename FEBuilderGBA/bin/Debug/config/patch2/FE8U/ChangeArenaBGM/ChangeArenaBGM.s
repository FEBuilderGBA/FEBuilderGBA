@FE8J	08074BEC	@{J}
@FE8J	08072710	@{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


.equ DefautlArenaSongID, Table+4

@壊すコードの再送1

SelectArenaBGM:
push {r4, r5}

@ldr  r0, =0x0203A8EC	@ArenaData	@{J}
ldr  r0, =0x0203A8F0	@ArenaData	@{U}
ldr  r1, [r0, #0x0]
cmp  r1, #0x0
beq  NotFound

mov  r5, r0		@ArenaData

ldr  r4, Table
sub  r4, #0x8
Loop:
add  r4, #0x8

ldr  r0, [r4]
cmp  r0, #0xff	@Term
beq  NotFound

ldrb r0, [r4, #0x0] @Table->EnemyClass
cmp  r0, #0x0
beq  CheckPlayerUnit

ldr  r1, [r5, #0x4]	@ArenaData->opponentUnit
ldr  r1, [r1, #0x4] @ArenaData->opponentUnit->Class
ldrb r1, [r1, #0x4] @ArenaData->opponentUnit->Class->ID
cmp  r0, r1
bne  Loop


CheckPlayerUnit:
ldrb r0, [r4, #0x1] @Table->PlayerUnit
cmp  r0, #0x0
beq  EnemyCheck_LV

ldr  r1, [r5, #0x0]	@ArenaData->playerUnit
ldr  r1, [r1, #0x0] @ArenaData->playerUnit->Unit
ldrb r1, [r1, #0x4] @ArenaData->playerUnit->Unit->ID
cmp  r0, r1
bne  Loop


EnemyCheck_LV:
ldrb r2, [r4, #0x2] @Table->EnemyLV
cmp  r2, #0x0
beq  CHECK_OPTION

ldr  r1, [r5, #0x4]	@ArenaData->opponentUnit
ldrb r1, [r1, #0x8]	@ArenaData->opponentUnit->Level

cmp  r1, r2
blt  Loop


CHECK_OPTION:
ldrb r2, [r4, #0x3] @Table->Option
cmp  r2, #0x0
beq  CHECK_FLAG

ldr r1, [r5]		@ArenaData->playerUnit
ldr r0, [r1, #0xc]	@RAMUnit->Status
lsr r0 ,r0 ,#0x11
mov r1, #0x7
and r0 ,r1
cmp r0, #0x4
blt Loop

CHECK_FLAG:
ldrh r0,  [r4, #0x4]	@Table->Flag
cmp  r0, #0x0
beq  Found

@blh  0x080860d0	@CheckFlag	@{J}
blh  0x08083DA8	@CheckFlag	@{U}
cmp  r0, #0x0
beq  Loop

Found:
@見つかった
ldrh r0, [r4, #0x6] @Table->BGM
b    Exit

NotFound:
ldr  r0, DefautlArenaSongID	@Default Arena BGM

Exit:
pop {r4, r5}

@壊すコードの再送2
mov r1, #0x80
lsl r1 ,r1 ,#0x1
@blh 0x08073f38   @BGMを切り替える(別命令 その2)	{J}
blh 0x08071a54   @BGMを切り替える(別命令 その2)	{U}

@ldr r3, =0x08074d6e|1	@{J}
ldr r3, =0x08072892|1	@{U}
bx r3

.align 4
.ltorg
Table:
@Table
@DefautlArenaSongID

@EnemyClass		1
@PlayerUnit		1
@EnemyLV>=		1
@Option			1
@Flag			2
@BGM			2
