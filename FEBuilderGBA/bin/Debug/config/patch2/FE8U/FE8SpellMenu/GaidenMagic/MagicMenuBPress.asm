.thumb

.macro blh to, reg=r6
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ HideMoveRangeGraphics, 0x0801DACC
.equ NewMenu_AndDoSomethingCommands, 0x0804F64C
.equ BG_EnableSyncByMask, 0x08001FAC
.equ BG_Fill, 0x08001220
.equ func1, 0x08003D20

@08022860 B500   PUSH {lr}   //MenuDef5__Destruction 
@08022862 480C   LDR r0, [PC, #0x30] # pointer:08022894 -> 02023CA8 (BG2 Map Buffer )   //Skill SkillSystems 20171130@00022862.bin@BIN 
@08022864 2100   MOV r1, #0x0
@08022866 F7DE FCDB   BL 0x08001220   //BG_Fill 
@0802286A 2004   MOV r0, #0x4
@0802286C F7DF FB9E   BL 0x08001FAC   //BG_EnableSyncByMask 
@08022870 F7E1 FA56   BL 0x08003D20
@08022874 4808   LDR r0, [PC, #0x20] # pointer:08022898 -> 0859D1F0
@08022876 4A09   LDR r2, [PC, #0x24] # pointer:0802289C -> 0202BCB0 (gMainLoopEndedFlag )
@08022878 231C   MOV r3, #0x1C
@0802287A 5ED1   LDSH r1, [r2, r3] # pointer:0202BCCC (gSomeRealCameraPosition )
@0802287C 230C   MOV r3, #0xC
@0802287E 5ED2   LDSH r2, [r2, r3] # pointer:0202BCBC (gCurrentRealCameraPos )
@08022880 1A89   SUB r1 ,r1, R2
@08022882 2201   MOV r2, #0x1
@08022884 2316   MOV r3, #0x16
@08022886 F02C FEE1   BL 0x0804F64C   //NewMenu_AndDoSomethingCommands 
@0802288A F7FB F91F   BL 0x0801DACC   //HideMoveRangeGraphics 
@0802288E 203B   MOV r0, #0x3B
@08022890 BC02   POP {r1}
@08022892 4708   BX r1
@08022894 3CA8 0202   //LDRDATA
@08022898 D1F0 0859   //LDRDATA
@0802289C BCB0 0202   //LDRDATA

push {r6, lr}
ldr r0, =0x02023CA8
mov r1, #0x0
blh BG_Fill
mov r0, #0x4
blh BG_EnableSyncByMask
blh func1
ldr r0, =0x0859D1F0
ldr r2, =0x0202BCB0
mov r3, #0x1C
ldsh r1, [r2, r3]
mov r3, #0xC
ldsh r2, [r2, r3]
sub r1, r1, r2
mov r2, #0x1
mov r3, #0x16
blh NewMenu_AndDoSomethingCommands
blh HideMoveRangeGraphics

ldr r6, SelectedSpellPointer
mov r1, #0x0
str r1, [r6, #0x0]

Exit:
mov r0, #0x3B
pop {r6}
pop {r1}
bx r1

.ltorg
.align

SelectedSpellPointer:
@POIN SelectedSpellPointer