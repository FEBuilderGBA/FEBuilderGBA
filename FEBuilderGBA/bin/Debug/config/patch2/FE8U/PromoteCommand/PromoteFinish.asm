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


ldr r3, =CurrentUnit
ldr r3, [r3] 
cmp r3, #0 
beq Break
ldr r2, =MemorySlot 
ldr r0, [r2, #0x4*3] @ s3 
strh r0, [r3, #0x26] @ Item slot 1 

Break: 


pop {r0}
bx r0 

.ltorg 
.align 

