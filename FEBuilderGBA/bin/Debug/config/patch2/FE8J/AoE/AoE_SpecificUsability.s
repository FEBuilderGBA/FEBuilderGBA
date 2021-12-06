.align 4
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


AoE_SpecificUsability:
push {lr}

ldrb r0 , [r0,#0x9] @Menu->MenuID AoETableID
ldr r1, =AoE_EntrySize 
ldrb r1, [r1]
mul r1, r0 @ Offset for the entry we want 
ldr r0, =AoE_Table 
add r0, r1 @ Specific entry 

bl AoE_Usability



End: 

pop {r1} 
bx r1 

.ltorg
.align 4 

