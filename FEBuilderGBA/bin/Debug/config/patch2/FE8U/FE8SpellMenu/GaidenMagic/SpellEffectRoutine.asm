.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@08022CF0 B510   PUSH {r4,lr}   //MenuDef15_ Effect Pointer Executed When Selected 
@08022CF2 4C0D   LDR r4, [PC, #0x34] # pointer:08022D28 -> 03004E50 (Pointer to the work memory of the operation character )
@08022CF4 6820   LDR r0, [r4, #0x0] # pointer:03004E50 (Pointer to the work memory of the operation character ) r4=Unit
@08022CF6 313C   ADD r1, #0x3C
@08022CF8 7809   LDRB r1, [r1, #0x0]
@08022CFA 0609   LSL r1 ,r1 ,#0x18
@08022CFC 1609   ASR r1 ,r1 ,#0x18
@08022CFE F7F3 FF5F   BL 0x08016BC0   //EquipUnitItemByIndex 
@08022D02 490A   LDR r1, [PC, #0x28] # pointer:08022D2C -> 0203A958 (gActionData )
@08022D04 2000   MOV r0, #0x0
@08022D06 7488   STRB r0, [r1, #0x12]
@08022D08 F02B FDBC   BL 0x0804E884   //ClearBG0BG1 
@08022D0C 6820   LDR r0, [r4, #0x0] # pointer:03004E50 (Pointer to the work memory of the operation character ) r4=Unit
@08022D0E 8BC1   LDRH r1, [r0, #0x1E] r0=Unit
@08022D10 F002 FA50   BL 0x080251B4   //MakeTargetListForWeapon 
@08022D14 4806   LDR r0, [PC, #0x18] # pointer:08022D30 -> 0859D3F8
@08022D16 F02C FE91   BL 0x0804FA3C   //NewTargetSelection 
@08022D1A F060 FAD5   BL 0x080832C8
@08022D1E 2027   MOV r0, #0x27
@08022D20 BC10   POP {r4}
@08022D22 BC02   POP {r1}
@08022D24 4708   BX r1

.equ GetItemWeaponExp, 0x8017798
.equ EquipUnitItemByIndex, 0x08016BC0
.equ ClearBG0BG1, 0x0804E884
.equ MakeTargetListForWeapon, 0x080251B4
.equ NewTargetSelection, 0x0804FA3C
.equ Func2, 0x080832C8
.equ SpellsGetter, SelectedSpellPointer+4

@b33028

push {r3,r4,r5,lr}
ldr r4, =0x03004E50
ldr r0, [r4, #0x0]
add r1, #0x3C
ldrb r1, [r1, #0x0]
lsl r1, r1, #0x18
asr r5, r1, #0x18	@store index in r5

@ get spell at index in r1
ldr r3, SpellsGetter
mov lr, r3
.short 0xf800

mov r1, r5			@get index back in r1
mov r5, r0			@store spell list in r5

ldrb r0, [r5, r1]	@load nth spell
mov r5, r0			@store it

push {r2}
blh GetItemWeaponExp @get spell cost
mov r2, r0				@store in r2
ldr r1, [r4, #0x0] @active character
mov r0, #0x13			@current HP
ldrsb r0, [r1, r0]		@load the byte
cmp r2, r0
blt Equip				@if cost is less than HP, equip the spell

@ check if enough hp to cast

@ if not enough hp, display textbox and exit

@ ldr r1, =0x00000858
@ mov r0, =ParentProcHeader
@ blh Menu_CallTextBox

mov r0, #0x8
b EndFunc

@equip the spell at index in r1

Equip:
ldr r2, SelectedSpellPointer
mov r0, #0xFF
lsl r0, r0, #0x8
orr r0, r5
strh r0, [r2, #0x0]				@store spell data as halfword
pop {r2}

ldr r1, =0x0203A958				@gActionData
mov r0, #0x0
strb r0, [r1, #0x12]
blh ClearBG0BG1
ldr r0, [r4, #0x0]
mov r1, #0xFF
lsl r1, r1, #0x8
orr r1, r5
blh MakeTargetListForWeapon
ldr r0, =0x0859D3F8
blh NewTargetSelection
blh Func2
mov r0, #0x27

EndFunc:
pop {r3,r4,r5}
pop {r1}
bx r1

.ltorg
.align

SelectedSpellPointer:
@POIN SelectedSpellPointer
@POIN SpellsGetter
