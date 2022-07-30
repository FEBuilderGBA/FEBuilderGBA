.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@.equ Attacker, 0x203A4EC	@{U}
.equ Attacker, 0x0203A4E8	@{J}
@.equ GetUnitStruct, 0x8019430	@{U}
.equ GetUnitStruct, 0x08019108	@{J}

.type Steal3, %function 
.global Steal3 
Steal3: 
push {lr} 
ldr r3, =Attacker 
ldrb r0, [r3, #0x0B] 
blh GetUnitStruct 
cmp r0, #0 
beq Exit2 
ldr r3, =Attacker 
add r0, #0x1e 
add r3, #0x1e 
mov r2, #0 
Loop: 
cmp r2, #10 
bge Exit2 
ldrh r1, [r0, r2] 
strh r1, [r3, r2] 
add r2, #2 
b Loop 


Exit2: 
pop {r0} 
bx r0 
.ltorg 
