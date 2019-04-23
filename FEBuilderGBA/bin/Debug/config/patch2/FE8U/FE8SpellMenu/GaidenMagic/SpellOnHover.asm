.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ ClearMapWith, 0x080197E4
.equ GetUnitRangeMask, 0x080171E8
.equ FillRangeByRangeMask, 0x0801B460
.equ DisplayMoveRangeGraphics, 0x0801DA98

@08022D84 B530   PUSH {r4,r5,lr}   @MenuDef15_ Action Pointer When Selected 
@08022D86 1C0D   mov r5 ,r1
@08022D88 353C   ADD r5, #0x3C
@08022D8A 2000   mov r0, #0x0
@08022D8C 5628   ldsb r0, [r5, r0]
@08022D8E F7FB FCDB   BL 0x0801E748
@08022D92 480E   ldr r0, [PC, #0x38] # pointer:08022DCC -> 0202E4E0 (gMapMovement )
@08022D94 6800   ldr r0, [r0, #0x0] # pointer:0202E4E0 (gMapMovement )
@08022D96 2101   mov r1, #0x1
@08022D98 4249   neg r1 ,r1
@08022D9A F7F6 FD23   BL 0x080197E4   //ClearMapWith 
@08022D9E 480C   ldr r0, [PC, #0x30] # pointer:08022DD0 -> 0202E4E4 (gMapRange )
@08022DA0 6800   ldr r0, [r0, #0x0] # pointer:0202E4E4 (gMapRange )
@08022DA2 2100   mov r1, #0x0
@08022DA4 F7F6 FD1E   BL 0x080197E4   //ClearMapWith 
@08022DA8 4C0A   ldr r4, [PC, #0x28] # pointer:08022DD4 -> 03004E50 (Pointer to the work memory of the operation character )
@08022DAA 6820   ldr r0, [r4, #0x0] # pointer:03004E50 (Pointer to the work memory of the operation character ) r4=Unit
@08022DAC 2100   mov r1, #0x0
@08022DAE 5669   ldsb r1, [r5, r1]
@08022DB0 F7F4 FA1A   BL 0x080171E8   //GetUnitRangeMask 
@08022DB4 1C01   mov r1 ,r0
@08022DB6 6820   ldr r0, [r4, #0x0] # pointer:03004E50 (Pointer to the work memory of the operation character ) r4=Unit
@08022DB8 F7F8 FB52   BL 0x0801B460   //FillRangeByRangeMask 
@08022DBC 2002   mov r0, #0x2
@08022DBE F7FA FE6B   BL 0x0801DA98   //DisplayMoveRangeGraphics 
@08022DC2 2000   mov r0, #0x0
@08022DC4 BC30   pop {r4,r5}
@08022DC6 BC02   pop {r1}
@08022DC8 4708   bx r1

push {r3, r4,r5,lr}   @MenuDef15_ Action Pointer When Selected 
mov r5 ,r1
add r5, #0x3C

MarkAsSpellrange:
ldr r3, =0x0203F082 
mov r0, #0x1
strb r0, [r3]

mov r0, #0x0
ldsb r0, [r5, r0]

ldr r3, SpellHoverHelper
mov lr, r3
.short 0xf800

ldr r0, =0x0202E4E0 @(gMapMovement )
ldr r0, [r0, #0x0]
mov r1, #0x1
neg r1 ,r1
blh ClearMapWith 
ldr r0, =0x0202E4E4 @(gMapRange )
ldr r0, [r0, #0x0] 
mov r1, #0x0
blh ClearMapWith 
ldr r4, =0x03004E50 @(Pointer to the work memory of the operation character )
ldr r0, [r4, #0x0] 
mov r1, #0x0
ldsb r1, [r5, r1]
blh GetUnitRangeMask 
mov r1 ,r0
ldr r0, [r4, #0x0] 
blh FillRangeByRangeMask 
mov r0, #0x2
blh DisplayMoveRangeGraphics 
mov r0, #0x0

Exit:
pop {r3,r4,r5}
pop {r1}
bx r1

.ltorg
.align

SpellHoverHelper:
@POIN SpellHoverHelper
