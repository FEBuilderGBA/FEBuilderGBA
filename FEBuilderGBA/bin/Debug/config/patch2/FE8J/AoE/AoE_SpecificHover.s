.thumb 
.align 4

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.include "Definitions.s"

.type AoE_HoverEffect, %function 
AoE_HoverEffect:
push {r4, lr} 

ldr  r0, [r1, #0x30] @get current menu struct
ldrb r4, [r0, #0x9]  @Menu->MenuID AoETableID
bl AoE_ClearMoveMap

mov r0, r4
ldr r1, =AoE_EntrySize 
ldrb r1, [r1]
mul r1, r0 @ Offset for the entry we want 
ldr r0, =AoE_Table 
add r0, r1 @ Specific entry 
mov r4, r0 

@mov r0, r4 
@ Given r0 = Table Entry, construct range map 
bl AoE_RangeSetup_Hover @ also clears range map 
@could be green instead tho 

@ldrb r1, [r4, #ConfigByte] @ Stationary bool 
@mov r0, #HealBool
@tst r0, r1 
@mov r0, #2 @ red 
@beq DisplayColour
@mov r0, #0x40 @ Green 
@DisplayColour:

@mov r0, #2
@blh 0x801da98 @DisplayMoveRangeGraphics	@{U}
@@blh 0x0801D6FC @DisplayMoveRangeGraphics	@{J}


mov r2, r4 

ldr r3, =CurrentUnit
ldr r3, [r3] 
ldrb r0, [r3, #0x10] @ XX 
ldrb r1, [r3, #0x11] @ YY 
@given r0 = xx, r1 = yy, r2= table entry pointer, display movement squares in a template around it 
mov r3, #0 @ Rotation 
bl AoE_DisplayDamageArea
@mov r0, #42 
@blh 0x801da98 @DisplayMoveRangeGraphics	@{U}
@@blh 0x0801D6FC @DisplayMoveRangeGraphics	@{J}

Exit:

pop {r4}
pop {r0} 
bx r0 

.align 4
.ltorg 

