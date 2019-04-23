.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ GetItemAttributes, 0x0801756C
.equ CanUnitUseAsWeapon, 0x08016574
.equ MakeTargetListForWeapon, 0x080251B4
.equ GetTargetListSize, 0x0804FD28

@this checks if unit is high enough level to use this spell.
@r1 is index
@r4 will hold spell

@08022CA4 B530   PUSH {r4,r5,lr}   //MenuDef15_ Possibility Diagnostic Routine Pointer 
@08022CA6 4D0F   LDR r5, [PC, #0x3C] # pointer:08022CE4 -> 03004E50 (Pointer to the work memory of the operation character )
@08022CA8 6828   LDR r0, [r5, #0x0] # pointer:03004E50 (Pointer to the work memory of the operation character ) r5=Unit
@08022CAA 0049   LSL r1 ,r1 ,#0x1
@08022CAC 301E   ADD r0, #0x1E
@08022CAE 1840   ADD r0 ,r0, R1
@08022CB0 8804   LDRH r4, [r0, #0x0] r0=Unit   //spell stored to r4
@08022CB2 1C20   MOV r0 ,r4 			
@08022CB4 F7F4 FC5A   BL 0x0801756C   //GetItemAttributes 
@08022CB8 2101   MOV r1, #0x1
@08022CBA 4001   AND r1 ,r0
@08022CBC 2900   CMP r1, #0x0
@08022CBE D013   BEQ #0x8022CE8
@    08022CC0 6828   LDR r0, [r5, #0x0] # pointer:03004E50 (Pointer to the work memory of the operation character ) r5=Unit
@    08022CC2 1C21   MOV r1 ,r4
@    08022CC4 F7F3 FC56   BL 0x08016574   //CanUnitUseAsWeapon 
@    08022CC8 0600   LSL r0 ,r0 ,#0x18
@    08022CCA 2800   CMP r0, #0x0
@    08022CCC D00C   BEQ #0x8022CE8
@        08022CCE 6828   LDR r0, [r5, #0x0] # pointer:03004E50 (Pointer to the work memory of the operation character ) r5=Unit
@        08022CD0 1C21   MOV r1 ,r4
@        08022CD2 F002 FA6F   BL 0x080251B4   //MakeTargetListForWeapon 
@        08022CD6 F02D F827   BL 0x0804FD28   //GetTargetListSize Gets list size (used to check for empty lists in usability routines) Number of entries in the list
@        08022CDA 2800   CMP r0, #0x0
@        08022CDC D004   BEQ #0x8022CE8
@            08022CDE 2001   MOV r0, #0x1
@            08022CE0 E003   B 0x8022CEA
@            08022CE2 0000   NOP
@08022CE4 4E50 0300   //LDRDATA
@08022CE8 2003   MOV r0, #0x3
@08022CEA BC30   POP {r4,r5}
@08022CEC BC02   POP {r1}
@08022CEE 4708   BX r1

@b330d4

push {r3, r4, r5, lr}
ldr r5, =0x03004E50
ldr r0, [r5, #0x0]	@unit

mov r4, r1			@store index in r4

@ get spell at index in r1
ldr r3, SpellsGetter
mov lr, r3
.short 0xf800

mov r1, r4			@get index back in r1
mov r4, r0			@store spell list in r4

ldrb r0, [r4, r1]	@load nth spell
mov r4, r0			@store it

mov r0, r4
blh GetItemAttributes
mov r1, #0x1
and r1, r0
cmp r1, #0x0
beq CantUse
ldr r0, [r5, #0x0]
mov r1, r4
blh CanUnitUseAsWeapon
lsl r0, r0, #0x18
cmp r0, #0x0
beq CantUse
ldr r0, [r5, #0x0]
mov r1, r4
blh MakeTargetListForWeapon
blh GetTargetListSize
cmp r0, #0x0
beq CantUse
mov r0, #0x1
b Exit

CantUse:
mov r0, #0x3

Exit:
pop {r3, r4, r5}
pop {r1}
bx r1

.ltorg
.align

SpellsGetter:
@POIN SpellsGetter
