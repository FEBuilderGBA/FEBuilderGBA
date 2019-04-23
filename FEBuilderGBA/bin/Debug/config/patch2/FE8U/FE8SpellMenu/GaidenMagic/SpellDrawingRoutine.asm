.thumb

.macro blh to, reg=r7
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@08022D34 B570   PUSH {r4,r5,r6,r7,lr}   //MenuDef15_ Drawing Routine Pointer 
@08022D36 1C0D   MOV r5 ,r1
@08022D38 4810   LDR r0, [PC, #0x40] # pointer:08022D7C -> 03004E50 (Pointer to the work memory of the operation character )
@08022D3A 6800   LDR r0, [r0, #0x0] # pointer:03004E50 (Pointer to the work memory of the operation character ) r0=Unit r0=Unit
@08022D3C 313C   ADD r1, #0x3C
@08022D3E 2200   MOV r2, #0x0
@08022D40 568A   LDSB r2, [r1, r2]
@08022D42 0052   LSL r2 ,r2 ,#0x1
@08022D44 1C01   MOV r1 ,r0
@08022D46 311E   ADD r1, #0x1E //unit's first item
@08022D48 1889   ADD r1 ,r1, R2
@08022D4A 880C   LDRH r4, [r1, #0x0] r1=Unit
@08022D4C 1C21   MOV r1 ,r4 //r1 needs to contain 00 00 Durability ItemNumber
@08022D4E F7F3 FC11   BL 0x08016574   //CanUnitUseAsWeapon 
@08022D52 1C02   MOV r2 ,r0 //returns bool
@08022D54 1C28   MOV r0 ,r5
@08022D56 3034   ADD r0, #0x34
@08022D58 0612   LSL r2 ,r2 ,#0x18
@08022D5A 1612   ASR r2 ,r2 ,#0x18
@08022D5C 212C   MOV r1, #0x2C
@08022D5E 5E6B   LDSH r3, [r5, r1]
@08022D60 015B   LSL r3 ,r3 ,#0x5
@08022D62 262A   MOV r6, #0x2A
@08022D64 5FA9   LDSH r1, [r5, r6] r1=Unit
@08022D66 185B   ADD r3 ,r3, R1
@08022D68 005B   LSL r3 ,r3 ,#0x1
@08022D6A 4905   LDR r1, [PC, #0x14] # pointer:08022D80 -> 02022CA8 (BG0 Map Buffer )
@08022D6C 185B   ADD r3 ,r3, R1
@08022D6E 1C21   MOV r1 ,r4 //r1 needs to contain 00 00 Durability ItemNumber
@08022D70 F7F3 FD6A   BL 0x08016848   //DrawItemMenuCommand 
@08022D74 2000   MOV r0, #0x0
@08022D76 BC70   POP {r4,r5,r6,r7}
@08022D78 BC02   POP {r1}
@08022D7A 4708   BX r1
@08022D7C 4E50 0300   //LDRDATA
@08022D80 2CA8 0202   //LDRDATA

.equ GetItemWeaponExp, 0x8017798
.equ CanUnitUseAsWeapon, 0x08016574
.equ DrawItemMenuCommand, 0x08016848

@b32fb4

push {r4-r7,lr}
mov r5, r1
ldr r0, =0x03004E50
ldr r0, [r0, #0x0] @active character
mov r4, r0 @store active unit
ldr r7, SpellsGetter
mov lr, r7
.short 0xf800
mov r1, r5
add r1, #0x3C
mov r2, #0x0
ldsb r2, [r1, r2]
ldrb r1, [r0, r2] @get nth spell in list
mov r3, r1			@store in r3
mov r0, r1
blh GetItemWeaponExp @get spell cost
lsl r0, r0, #0x8
mov r1, r3
orr r1, r0  @format as 00 00 Cost WeaponID
mov r0, r4 @get active unit
mov r4, r1 @store spell as item
blh CanUnitUseAsWeapon
mov r2, r0
mov r0, r5
add r0, #0x34
lsl r2, r2, #0x18
asr r2, r2, #0x18
mov r1, #0x2C
ldsh r3, [r5, r1]
lsl r3, r3, #0x5
mov r6, #0x2A
ldsh r1, [r5, r6]
add r3, r3, r1
lsl r3, r3, #0x1
ldr r1, =0x02022CA8 @BG0 Map Buffer
add r3, r3, r1
mov r1, r4 @get item
blh DrawItemMenuCommand
mov r0, #0x0
pop {r4-r7}
pop {r1}
bx r1

.ltorg
.align

SpellsGetter:
@POIN SpellsGetter