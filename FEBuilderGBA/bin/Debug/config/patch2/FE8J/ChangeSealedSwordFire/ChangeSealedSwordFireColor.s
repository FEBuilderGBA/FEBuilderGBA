@FE8J 0805E0FC	@{J}
@FE8U 0805CFFC	@{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


.equ PaletteTable, Table+4

@r4 this

push {r4, r5}
str r0, [r4, #0x60]  @壊すコードの再送

mov r0, r4
blh 0x0805af10   		@GetAISSubjectId	{J}
@blh 0x0805a16c   		@GetAISSubjectId	{U}
cmp r0, #0x0
bne RightSide

LeftSize:
ldr  r0, =0x0203E188	@戦闘アニメで攻撃側へのRAMポインタ	@{J}
@ldr  r0, =0x0203E18C	@戦闘アニメで攻撃側へのRAMポインタ	@{U}
b    JoinGetActor

RightSide:
ldr  r0, =0x0203E184	@戦闘アニメで防御側へのRAMポインタ	@{J}
@ldr  r0, =0x0203E188	@戦闘アニメで防御側へのRAMポインタ	@{U}

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

blh  0x080860d0	@CheckFlag	@{J}
@blh  0x08083DA8	@CheckFlag	@{U}
cmp  r0, #0x0
beq  Loop

Found:
ldrh r0, [r4, #0x6] @Table->PaletteID
b    Exit

NotFound:
mov  r0, #0x0        @Default Palette Index 0
@b    Exit

Exit:
pop  {r4, r5}
ldr  r3, PaletteTable
lsl  r0, r0, #0x2    @ r0=r0*4
ldr  r0, [r3 , r0]   @ Get Palette Pointer

mov r1, #0x20
blh 0x0805679c   @SomePaletteStoringRoutine_SpellAnim	@{J}
@blh 0x08055800   @SomePaletteStoringRoutine_SpellAnim	@{U}

ldr r3, =0x0805E106|1	@{J}
@ldr r3, =0x0805D006|1	@{U}
bx r3

.align
.ltorg
Table:
@Table
@PaletteTable
