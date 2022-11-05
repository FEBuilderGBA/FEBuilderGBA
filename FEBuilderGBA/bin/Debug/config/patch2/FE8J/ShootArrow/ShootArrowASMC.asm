.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@.equ ProcStartBlocking, 0x8002CE0 @{U}
.equ ProcStartBlocking, 0x08002C30 @{J}
@.equ MemorySlot, 0x30004B8  @{U}
.equ MemorySlot, 0x030004B0  @{J}

push {lr} 

@The code is too short, so I'm going to puff it up a bit.
ldr r2, =MemorySlot 
ldrh r2, [r2, #4*1] @ damage 
cmp r2, #120
bgt Fix120

cmp r2, #0x0
beq Fix10
b   Fire

Fix120:
mov r1, #120
ldr r2, =MemorySlot 
strh r1, [r2, #4*1] @ damage 
b   Fire

Fix10:
mov r1, #10
ldr r2, =MemorySlot 
strh r1, [r2, #4*1] @ damage 
@b   Fire

Fire:
mov r1, r0 @ parent proc 
ldr r0, MyProc 
blh ProcStartBlocking 

pop {r0} 
bx r0 
.ltorg 

MyProc: 



