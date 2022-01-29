

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


.equ CurrentUnit, 0x03004E50	@{U}
@.equ CurrentUnit, 0x03004DF0	@{J}
.equ MemorySlot,0x030004B8	@{U}
@.equ MemorySlot,0x030004B0	@{J}
.equ EventEngine, 0x0800D07C	@{U}
@.equ EventEngine, 0x0800D340	@{J}
.equ CurrentUnitFateData, 0x0203A958	@{U}
@.equ CurrentUnitFateData, 0x0203A954	@{J}

push {lr}

ldr r0, PromotionMenuEvent 
mov r1, #1 
blh EventEngine 

mov r0, #0x17 


pop {r1}
bx r1 


.ltorg
.align 4

PromotionMenuEvent:
