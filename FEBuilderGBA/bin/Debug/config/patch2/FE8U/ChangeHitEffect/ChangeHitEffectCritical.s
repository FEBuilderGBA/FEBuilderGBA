@FE8J 0806EA44	@{J}
@FE8U 0806C720	@{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ StartDelayProcsFunction, Table+4

@r0 this
@r5 this
push {r4 ,r5 , r6}

@mov r0, r5  @もともとr5にはCurrent Procs (this)が入ってます
@blh 0x0805b058   @GetCoreAIStruct	@{J}
blh 0x0805a2b4   @GetCoreAIStruct	@{U}
mov  r6, r0

@blh 0x0805af10   		@GetAISSubjectId	{J}
blh 0x0805a16c   		@GetAISSubjectId	{U}
cmp r0, #0x0
bne LeftSize

RightSide:
@ldr  r0, =0x0203E184	@戦闘アニメで防御側へのRAMポインタ	@{J}
ldr  r0, =0x0203E188	@戦闘アニメで防御側へのRAMポインタ	@{U}
b    JoinGetActor

LeftSize:
@ldr  r0, =0x0203E188	@戦闘アニメで攻撃側へのRAMポインタ	@{J}
ldr  r0, =0x0203E18C	@戦闘アニメで攻撃側へのRAMポインタ	@{U}
@b    JoinGetActor

JoinGetActor:
ldr  r5, [r0]			@gBattleActor
cmp  r5, #0x0
beq  NotFound

ldr  r4, Table
sub  r4, #0x8

Loop:
add  r4, #0x8

ldr  r0, [r4]
cmp  r0, #0xFF
beq  NotFound

CheckUnit:
ldrb r0, [r4, #0x0] @Table->UnitID
cmp  r0, #0x0
beq  CheckClass

ldr  r1, [r5, #0x0] @gBattleActor->Unit
ldrb r2, [r1, #0x4] @gBattleActor->Unit->ID
cmp  r0, r2
bne  Loop

CheckClass:
ldrb r0, [r4, #0x1] @Table->ClassID
cmp  r0, #0x0
beq  CheckItem

ldr  r1, [r5, #0x4] @gBattleActor->Class
ldrb r2, [r1, #0x4] @gBattleActor->Class->ID
cmp  r0, r2
bne  Loop

CheckItem:
ldrb r0, [r4, #0x2] @Table->ItemID
cmp  r0, #0x0
beq  CheckAffiliation

mov  r1, #0x48
ldrb r1, [r5, r1] @gBattleActor->WeaponID
cmp  r0, r1
bne  Loop

CheckAffiliation:
ldrb r1, [r5, #0xB] @gBattleActor->部隊表ID

ldrb r0, [r4, #0x3] @Table->Aff
cmp  r0, #0x0	@All
beq  Check_Flag

cmp  r0, #0x2 @Enemy
beq  CheckAffiliation_Enemy

cmp  r0, #0x3 @NPC
beq  CheckAffiliation_NPC

CheckAffiliation_Player:
cmp  r1, #0x40
bge  Loop
b    Check_Flag

CheckAffiliation_Enemy:
cmp  r1, #0x80
blt  Loop
b    Check_Flag

CheckAffiliation_NPC:
cmp  r1, #0x40
blt  Loop
cmp  r1, #0x80
ble  Loop
@b    Check_Flag

Check_Flag:
ldrh r0, [r4, #0x4] @Table->Flag
cmp  r0, #0x0 @All
beq  Found

@blh  0x080860d0	@CheckFlag	@{J}
blh  0x08083DA8	@CheckFlag	@{U}
cmp  r0, #0x0
beq  Loop

Found:
ldrb r4, [r4, #0x7] @Table->EffectID2

cmp  r4 ,#0x10 @攻撃系のエフェクトは利用できません
ble  NotFound
@b    BreakExit
BreakExit:

mov  r0, r4    @effectID
mov  r1, r6    @AIS
ldr  r3, StartDelayProcsFunction @call StartDelayProcsFunction
mov lr, r3
.short 0xf800

pop {r4, r5 ,r6}

pop {r4, r5}  @親関数の強制終了
pop {r0}
bx r0

NotFound:
mov  r3, r6 @AISCore
pop  {r4, r5 ,r6}

@壊すコードの再送
mov r0, r3
mov r4, r3
@blh 0x0805af10   @GetAISSubjectId	@{J}
blh 0x0805a16c   @GetAISSubjectId	@{U}

@ldr r3, =0x0806EA4E|1	@{J}
ldr r3, =0x0806C72A|1	@{U}
bx r3

.align
.ltorg
Table:
@Table
@StartDelayProcsFunction
