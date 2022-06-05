
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ GetUnit, 0x08019430		@{U}
@.equ GetUnit, 0x08019108		@{J}

.equ MemorySlot,0x030004B8		@{U}
@.equ MemorySlot,0x030004B0		@{J}

GetTargetUnitID:
push {lr}
ldr r3, =0x203A958	@{U}
@ldr r3, =0x203A954	@{J}
ldrb r0, [r3, #0x0D] @ target 
blh GetUnit 
cmp r0, #0 
beq Error 
ldr r1, [r0] 
cmp r1, #0 
beq Error 
ldr r1, [r0, #0x0C] @ state 
ldr r2, =0x1000C @ escaped, dead, undeployed 
tst r1, r2 
bne Error 
ldr r1, [r0] @ unit pointer 
ldrb r0, [r1, #4] @ unit ID 
b Store 

Error:
mov r0, #1
lsl r0, #8 @ 0x100 if error 
Store: 
ldr r3, =MemorySlot 
add r3, #4*0x0C 
str r0, [r3] 

pop {r0}
bx r0 
.ltorg 

