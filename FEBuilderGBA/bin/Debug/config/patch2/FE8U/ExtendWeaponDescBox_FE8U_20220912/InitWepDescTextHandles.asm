.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ Text_SetColorId, 0x8003E60	@{U}
@.equ Text_SetColorId, 0x8003D90	@{J}
.equ Text_Init2DLine, 0x80045D8	@{U}
@.equ Text_Init2DLine, 0x80044E0	@{J}

push {r4, lr} 
mov r4, r7 
blh Text_Init2DLine 
mov r0, r4
add r0, #0x28 
blh Text_Init2DLine 

@ added 
mov r0, r4
add r0, #0x38
blh Text_Init2DLine 

mov r0, r4 
add r0, #0x40 
blh Text_Init2DLine 

pop {r4} 
pop {r0} 
bx r0 

.ltorg 
