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

push {r6, lr} 
@ we don't need to push/pop r4/r5 
str r5, [r4, #0x30] 
mov r0, r9 
str r0, [r4, #0x34] @ proc fields: pointer to textHandle 
mov r1, r8 
str r1, [r4, #0x38] 
str r6, [r4, #0x3C] 

@ [202534C..2025353]?!! no hits on the proc fields +0x40 / +0x44 being read or written to (how lucky!) 
@ added 
mov r6, r5 
add r6, #0x38 @ [203E7CC..203E7DB]!! no hits 

@ potential issue: these two new textHandles are not re-initialized when scrolling up/down/etc. 
@ we set the x cursor back to 0 here. The rest are left unchanged 
mov r0, #0 
strb r0, [r6, #2] @ X cursor back to 0 


mov r0, r6 
str r0, [r4, #0x40] @ r4 from the vanilla function 

mov r0, r6 
mov r1, #6 
blh Text_SetColorId 
mov r0, r6 

mov r0, #0 
strb r0, [r6, #10] 
add r6, #8 
mov r0, r6  
str r0, [r4, #0x44] 
mov r0, r6 
mov r1, #6 
blh Text_SetColorId 

Exit: 


pop {r6} 
pop {r0} 
bx r0 

.ltorg 
