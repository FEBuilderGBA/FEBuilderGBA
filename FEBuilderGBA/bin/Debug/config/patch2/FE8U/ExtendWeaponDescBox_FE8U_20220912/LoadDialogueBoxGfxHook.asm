.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ ProcFind, 0x8002e9c		@{U}
@.equ ProcFind, 0x8002DEC		@{J}
.equ gProc_StatScreen, 0x8a009d8	@{U}
@.equ gProc_StatScreen, 0x8A72A90	@{J}
.equ gProc_Shop, 0x8a39210			@{U}
@.equ gProc_Shop, 0x8ABC808			@{J}
push {lr} 
cmp r1, #0 
bge Param2Provided
mov r6, #5 
Param2Provided: 
cmp r0, #0 
bne VramProvided

ldr r0, =gProc_StatScreen 
blh ProcFind 
ldr r5, =0x6012000 
cmp r0, #0 
bne VramProvided 
ldr r0, =gProc_Shop
blh ProcFind 
cmp r0, #0 
bne VramProvided 
ldr r5, =0x6013000 @ use 12000 in the stat screen and shop 
VramProvided: 
cmp r6, #0 

mov r4, #0xF 

pop {r0} 
bx r0 

.ltorg 
