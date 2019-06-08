.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ HideMoveRangeGraphics, 0x0801DACC

@08022DD8 B500   PUSH {lr}   //MenuDef14_ Action Pointer When Canceled 
@08022DDA 3063   ADD r0, #0x63
@08022DDC 7801   LDRB r1, [r0, #0x0]
@08022DDE 2004   MOV r0, #0x4
@08022DE0 4008   AND r0 ,r1
@08022DE2 2800   CMP r0, #0x0
@08022DE4 D101   BNE #0x8022DEA
@    08022DE6 F7FA FE71   BL 0x0801DACC   //HideMoveRangeGraphics 
@08022DEA 2000   MOV r0, #0x0
@08022DEC BC02   POP {r1}
@08022DEE 4708   BX r1

push {r3, lr}
add r0, #0x63
ldrb r1, [r0, #0x0]
mov r0, #0x4
and r0, r1
cmp r0, #0x0
bne Exit
blh HideMoveRangeGraphics

ZeroOutSelectedSpell:
ldr r3, SelectedSpellPointer
mov r0, #0x0
strh r0, [r3, #0x0]

Exit:
mov r0, #0x0
pop {r3}
pop {r1}
bx r1

.ltorg
.align

SelectedSpellPointer:
@POIN SelectedSpellPointer