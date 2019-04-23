.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ GetUnitEquippedWeaponSlot, 0x08016B58
.equ GetBallistaItemAt, 0x0803798C
.equ pDefenderBattleStruct, 0x0203a56c

Start:
mov r5, r0
mov r3, r1
mov r0, #0x1
neg r0, r0
cmp r3, r0
bne EnemyExists
mov r0, r5
blh GetUnitEquippedWeaponSlot
mov r3, r0

EnemyExists:
	ldr r0, [r5, #0xC]
	mov r1, #0x80
	lsl r1, r1, #0x4
	and r0, r1
	cmp r0, #0x0
	beq NotBallista
	mov r3, #0x8    @#0x8 sets flag for Ballista
	
NotBallista:
	ldr r0, =pDefenderBattleStruct
	cmp r5, r0
	beq NotSpellMenu
	ldr r1, SelectedSpellPointer
	ldrh r0, [r1, #0x0]
	cmp r0, #0x0
	beq NotSpellMenu
	mov r3, #0x9 	@#0x9 sets flag for spell menu

NotSpellMenu:
mov r1, r5
add r1, #0x52
mov r0, #0x1
strb r0, [r1, #0x0]
mov r9, r1
cmp r3, #0x9
bhi FinalCase

cmp r3, #0x4
ble ItemIsInInventory @a79C

cmp r3, #0x5
beq ItemJustAdded  @a7b0

cmp r3, #0x6
beq ArenaAttackerCase  @a7cc

cmp r3, #0x7
beq ArenaDefenderCase  @a7ec

cmp r3, #0x8
beq BallistaCase  @a80c

cmp r3, #0x9
beq SpellMenuCase
b FinalCase

ItemIsInInventory:
mov r2, r5
add r2, #0x51
strb r3, [r2, #0x0]
ldrb r1, [r2, #0x0]
lsl r1, r1, #0x1
mov r0, r5
add r0, #0x1E
add r0, r0, r1
ldrh r0, [r0, #0x0]
b ContinueCase4And5

ItemJustAdded:
mov r2, r5
add r2, #0x51
mov r0, #0xFF
strb r0, [r2, #0x0]
ldr r0, =0x0202BCB0 		@gMainLoopEndedFlag
ldrh r0, [r0, #0x2C]		@gItemGotButNotYet

ContinueCase4And5:
mov r1, r5
add r1, #0x48				@equipped weapon after battle
strh r0, [r1, #0x0]
mov r8, r2					@byte for adding wexp?
mov r4, r1					@equipped weapon after battle -> r4
b Exit

ArenaAttackerCase:
mov r3, r5
add r3, #0x51
mov r0, #0x0
strb r0, [r3, #0x0]
ldr r0, =0x0203A8F0			@Arena data
ldrh r1, [r0, #0x1A]
mov r2, r5
add r2, #0x48
mov r0, #0x0
strh r1, [r2, #0x0]
mov r1, r9
strb r0, [r1, #0x0]
b StoreR3ToR8

ArenaDefenderCase:
mov r3, r5
add r3, #0x51
mov r0, #0x0
strb r0, [r3, #0x0]
ldr r0, =0x0203A8F0			@Arena data
ldrh r1, [r0, #0x1C]
mov r2, r5
add r2, #0x48
mov r0, #0x0
strh r1, [r2, #0x0]
mov r1, r9
strb r0, [r1, #0x0]
b StoreR3ToR8

BallistaCase:
mov r4, r5
add r4, #0x51
mov r0, #0xFE
strb r0, [r4, #0x0]		@weapon is not in inventory
mov r0, #0x10
ldsb r0, [r5, r0]		@unit X pos
mov r1, #0x11
ldsb r1, [r5, r1]		@unit Y pos
blh GetBallistaItemAt
mov r2, r5
add r2, #0x48
mov r1, #0x0
strh r0, [r2, #0x0]
mov r0, r9
strb r1, [r0, #0x0]
mov r8, r4
b StoreR2ToR4

SpellMenuCase:
mov r4, r5
add r4, #0x51
mov r0, #0xFF
strb r0, [r4, #0x0]		@weapon is not in inventory
ldr r1, SelectedSpellPointer
ldrh r0, [r1, #0x0]
mov r2, r5
add r2, #0x48
mov r1, #0x1
strh r0, [r2, #0x0]
mov r0, r9
strb r1, [r0, #0x0]
mov r8, r4
b StoreR2ToR4

FinalCase:
	mov r3, r5
	add r3, #0x51
	mov r0, #0xFE
	strb r0, [r3, #0x0]
	mov r2, r5
	add r2, #0x48
	mov r1, #0x0
	mov r0, #0x0
	strh r0, [r2, #0x0]
	mov r0, r9
	strb r1, [r0, #0x0]

StoreR3ToR8:
	mov r8, r3
	
StoreR2ToR4:
	mov r4, r2

Exit:
	pop {r3}
	ldr r1, =0x802A84B
	bx r1

.ltorg
.align

SelectedSpellPointer:
@POIN SelectedSpellPointer