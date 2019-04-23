.thumb

.equ origin, 0x081c1f14
.equ Find6C, . + 0x08002E9C - origin
.equ BG_GetMapBuffer, . + 0x08001C4C - origin
.equ TextVRAMClearer, . + 0x08003DC8 - origin
.equ MakeUIWindowTileMap_BG0BG1, . + 0x0804E368 - origin
.equ GetItemWType, . + 0x08017548 - origin
.equ GetItemUseDescId, . + 0x08017530 - origin
.equ GetStringFromIndex, . + 0x0800A240 - origin
.equ Text_InsertString, . + 0x08004480 - origin
.equ String_GetEnd, . + 0x08003FF4 - origin
.equ Text_Draw, . + 0x08003E70 - origin
.equ SetupBattleStructFromUnitAndWeapon, . + 0x0802A400 - origin
.equ CanUnitUseAsWeapon, . + 0x08016574 - origin
.equ Text_InsertNumberOr2Dashes, . + 0x080044A4 - origin
.equ DrawIcon, . + 0x080036BC - origin
.equ BG_EnableSyncByMask, . + 0x08001FAC - origin
.equ SpellsGetter, SelectedSpellPointer+4

PUSH {r4,r5,r6,r7,lr}
MOV r7, r10
MOV r6, r9
MOV r5, r8
PUSH {r5,r6,r7}
SUB SP, #0xC
MOV r5 ,r0
LDR r0, =0x0859AE88 @(Procs  )
BL Find6C 
MOV r7 ,r0
MOV r0, #0x0
BL BG_GetMapBuffer 
MOV r4 ,r7
ADD r4, #0x30
LDRB r1, [r4, #0x0] 
LSL r1 ,r1 ,#0x1
ADD r0 ,r0, R1
MOV r1, #0x31
ADD r1 ,r1, R7
MOV r8, r1
LDRB r1, [r1, #0x0] 
LSL r1 ,r1 ,#0x6
ADD r0 ,r0, R1
STR r0,[SP, #0x4]
MOV r6 ,r7
ADD r6, #0x34
LDR r2, [r7, #0x2C] 
MOV r9, r2
MOV r0 ,r7
ADD r0, #0x32
LDRB r0, [r0, #0x0] 
STR r0,[SP, #0x8]
MOV r0 ,r6
BL TextVRAMClearer 
MOV r0 ,r7
ADD r0, #0x3C
BL TextVRAMClearer 
MOV r0 ,r7
ADD r0, #0x44
BL TextVRAMClearer 
LDRB r0, [r4, #0x0] 
MOV r2, r8
LDRB r1, [r2, #0x0] 
MOV r2, #0x0
STR r2,[SP, #0x0]
MOV r2, #0xE
MOV r3, #0x8
BL MakeUIWindowTileMap_BG0BG1 

CMP r5, #0x0		@here's where the meat happens
BLT label3
CMP r5, #0x4
BLE WithinListBounds
CMP r5, #0x5
BEQ ItemJustGot
B label3

WithinListBounds:
	mov r0, r9
	ldr r1, SpellsGetter
	mov lr, r1
	.short 0xf800

	mov r1, r5			@get index back in r1
	mov r4, r0			@store spell list in r4

	ldrb r0, [r4, r1]	@load nth spell
	mov r4, r0			@store it
	
	ldr r2, SelectedSpellPointer
	mov r0, #0xFF
	lsl r0, r0, #0x8
	orr r0, r4
	strh r0, [r2, #0x0]				@store spell data as halfword
	
	b ItemLoaded

ItemJustGot:
	LDR r0, =0x0202BCB0
	LDRH r4, [r0, #0x2C] 
	B ItemLoaded

label3:
	MOV r4 ,r5
	MOV r5, #0x8

ItemLoaded:
	MOV r0 ,r4
	BL GetItemWType 
	CMP r0, #0x9
	BEQ label4
	CMP r0, #0x9
	BGT ItemType9
	CMP r0, #0x4
	BEQ label4
	B label5

ItemType9:
	CMP r0, #0xC
	BGT label5
	CMP r0, #0xB
	BLT label5

label4:
MOV r0 ,r4
BL GetItemUseDescId 
BL GetStringFromIndex 
MOV r4 ,r0
MOV r5, #0x0
LDR r7,[SP, #0x4]
ADD r7, #0x42
MOV r0, #0x8
ADD r0 ,r0, R6
MOV r8, r0
LDR r1,[SP, #0x4]
ADD r1, #0xC2
MOV r9, r1
MOV r2, #0x10
ADD r2 ,r2, R6
MOV r10, r2
B label7

label6:
ADD r4, #0x1
ADD r5, #0x1

label7:
LSL r0 ,r5 ,#0x3
ADD r0 ,r6, R0
MOV r1, #0x0
MOV r2, #0x0
MOV r3 ,r4
BL Text_InsertString 
MOV r0 ,r4
BL String_GetEnd 
MOV r4 ,r0
LDRB r0, [r4, #0x0]
CMP r0, #0x0
BNE label6

LDR r3, =0x0203A4EC 
LDR r2, =0x0203A56C 
MOV r0 ,r2
ADD r0, #0x5A
LDRH r0, [r0, #0x0] 
MOV r1 ,r3
ADD r1, #0x5A
STRH r0, [r1, #0x0]
MOV r0 ,r2
ADD r0, #0x60
LDRH r1, [r0, #0x0] 
MOV r0 ,r3
ADD r0, #0x60
STRH r1, [r0, #0x0]
MOV r0 ,r2
ADD r0, #0x66
LDRH r0, [r0, #0x0] 
MOV r1 ,r3
ADD r1, #0x66
STRH r0, [r1, #0x0]
MOV r0 ,r2
ADD r0, #0x62
LDRH r1, [r0, #0x0] 
MOV r0 ,r3
ADD r0, #0x62
STRH r1, [r0, #0x0]
MOV r0 ,r6
MOV r1 ,r7
BL Text_Draw 
MOV r0, r8
MOV r1, r9
BL Text_Draw 
LDR r0,[SP, #0x4]
MOV r2, #0xA1
LSL r2 ,r2 ,#0x1
ADD r1 ,r0, R2
MOV r0, r10
BL Text_Draw 
B label10

label5:
LSL r1 ,r5 ,#0x18
ASR r1 ,r1 ,#0x18
MOV r0, r9
BL SetupBattleStructFromUnitAndWeapon 
CMP r5, #0x8
BNE label8

LDR r3, =0x0203A56C 
LDR r2, =0x0203A4EC 
MOV r0 ,r2
ADD r0, #0x5A
LDRH r0, [r0, #0x0] 
MOV r1 ,r3
ADD r1, #0x5A
STRH r0, [r1, #0x0]
MOV r0 ,r2
ADD r0, #0x60
LDRH r1, [r0, #0x0] 
MOV r0 ,r3
ADD r0, #0x60
STRH r1, [r0, #0x0]
MOV r0 ,r2
ADD r0, #0x66
LDRH r0, [r0, #0x0]
MOV r1 ,r3
ADD r1, #0x66
STRH r0, [r1, #0x0]
MOV r0 ,r2
ADD r0, #0x62
LDRH r1, [r0, #0x0] 
MOV r0 ,r3
ADD r0, #0x62
STRH r1, [r0, #0x0]
	
label8:
LDR r0, =0x0203A4EC 
MOV r8, r0
MOV r1, #0x48			@must get spell instead of inventory item
ADD r1, r8
MOV r10, r1
LDRH r1, [r1, #0x0] 
MOV r0, r9
BL CanUnitUseAsWeapon 
LSL r0 ,r0 ,#0x18
MOV r2, #0x1
MOV r9, r2
CMP r0, #0x0
BEQ label9
MOV r0, #0x2
MOV r9, r0
	
label9:
LDR r0, =0x000004F1
BL GetStringFromIndex 
MOV r3 ,r0
MOV r0 ,r6
MOV r1, #0x1C
MOV r2, #0x0
BL Text_InsertString 
MOV r5 ,r6
ADD r5, #0x8
LDR r0, =0x000004F3
BL GetStringFromIndex 
MOV r3 ,r0
MOV r0 ,r5
MOV r1, #0x2
MOV r2, #0x0
BL Text_InsertString 
MOV r4 ,r6
ADD r4, #0x10
LDR r0, =0x000004F4
BL GetStringFromIndex 
MOV r3 ,r0
MOV r0 ,r4
MOV r1, #0x2
MOV r2, #0x0
BL Text_InsertString 
LDR r0, =0x00000501
BL GetStringFromIndex 
MOV r3 ,r0
MOV r0 ,r5
MOV r1, #0x32
MOV r2, #0x0
BL Text_InsertString 
LDR r0, =0x000004F5
BL GetStringFromIndex 
MOV r3 ,r0
MOV r0 ,r4
MOV r1, #0x32
MOV r2, #0x0
BL Text_InsertString 
MOV r0, r8
ADD r0, #0x5A
MOV r1, #0x0
LDSH r3, [r0, r1] 
MOV r0 ,r5
MOV r1, #0x24
MOV r2, r9
BL Text_InsertNumberOr2Dashes 
MOV r0, r8
ADD r0, #0x60
MOV r2, #0x0
LDSH r3, [r0, r2] 
MOV r0 ,r4
MOV r1, #0x24
MOV r2, r9
BL Text_InsertNumberOr2Dashes 
MOV r0, r8
ADD r0, #0x66
MOV r1, #0x0
LDSH r3, [r0, r1] 
MOV r0 ,r5
MOV r1, #0x54
MOV r2, r9
BL Text_InsertNumberOr2Dashes 
MOV r0, r8
ADD r0, #0x62
MOV r2, #0x0
LDSH r3, [r0, r2] 
MOV r0 ,r4
MOV r1, #0x54
MOV r2, r9
BL Text_InsertNumberOr2Dashes 
MOV r0 ,r7
ADD r0, #0x34
MOV r6 ,r7
ADD r6, #0x31
LDRB r1, [r6, #0x0] 
ADD r1, #0x1
LSL r1 ,r1 ,#0x5
ADD r1, #0x1
MOV r5 ,r7
ADD r5, #0x30
LDRB r2, [r5, #0x0] 
ADD r1 ,r1, R2
LSL r1 ,r1 ,#0x1
LDR r4, =0x02022CA8 @(BG0 Map Buffer )
ADD r1 ,r1, R4
BL Text_Draw 
MOV r0 ,r7
ADD r0, #0x3C
LDRB r1, [r6, #0x0] 
ADD r1, #0x3
LSL r1 ,r1 ,#0x5
ADD r1, #0x1
LDRB r2, [r5, #0x0]
ADD r1 ,r1, R2
LSL r1 ,r1 ,#0x1
ADD r1 ,r1, R4
BL Text_Draw 
MOV r0 ,r7
ADD r0, #0x44
LDRB r1, [r6, #0x0]
ADD r1, #0x5
LSL r1 ,r1 ,#0x5
ADD r1, #0x1
LDRB r2, [r5, #0x0]
ADD r1 ,r1, R2
LSL r1 ,r1 ,#0x1
ADD r1 ,r1, R4
BL Text_Draw 
LDR r4,[SP, #0x4]
ADD r4, #0x50
MOV r1, r10
LDRH r0, [r1, #0x0] 
BL GetItemWType 
MOV r1 ,r0
ADD r1, #0x70
LDR r0,[SP, #0x8]
LSL r2 ,r0 ,#0xC
MOV r0 ,r4
BL DrawIcon 

label10:
MOV r0, #0x1
BL BG_EnableSyncByMask 
ADD SP, #0xC
POP {r3,r4,r5}
MOV r8, r3
MOV r9, r4
MOV r10, r5
POP {r4,r5,r6,r7}
POP {r0}
BX r0

.ltorg
.align

SelectedSpellPointer:
@POIN SelectedSpellPointer
@POIN SpellsGetter
