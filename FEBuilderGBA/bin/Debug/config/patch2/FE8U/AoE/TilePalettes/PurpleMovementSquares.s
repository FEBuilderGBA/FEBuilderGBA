@ org 1DA98

.thumb 

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb 

	.equ ProcFind, 0x08002E9C	@{U}
@	.equ ProcFind, 0x08002DEC	@{J}

push {r4-r5, lr} @ will pop when returned to vanilla function 


mov r5, r0 


ldr r4, =0x859AD50	@{U}
@ldr r4, =0x85C31D0	@{J}
mov r0, r4 
blh ProcFind
cmp r0, #0 
beq DisplayRangeSquares

add r0, #0x4a  @ range ? 
strh r5, [r0]


DisplayMoveSquares:
ldr r3, =0x801DAA8|1	@{U}
@ldr r3, =0x801D70C|1	@{J}
bx r3 


@ display range squares ig 
DisplayRangeSquares:
ldr r3, =0x801DAB8|1	@{U}
@ldr r3, =0x801D71C|1	@{J}
bx r3 





.align 4 
.ltorg 
