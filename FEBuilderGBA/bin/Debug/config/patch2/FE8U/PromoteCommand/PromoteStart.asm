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
.equ PromoteActiveUnit, 0x080CCA14 @//NewPromotion	@{U}
@.equ PromoteActiveUnit, 0x080D1730 @//NewPromotion	@{J}
.equ EndMMS, 0x080790a4 @//MU_EndAll //deselect unit	@{U}
@.equ EndMMS, 0x0807B4B8 @//MU_EndAll //deselect unit	@{J}
.equ ActionStruct, 0x0203A958	@{U}
@.equ ActionStruct, 0x0203A954	@{J}
.equ Attacker, 0x203A4EC	@{U}
@.equ Attacker, 0x203A4E8	@{J}
.equ Defender, 0x203A56C	@{U}
@.equ Defender, 0x203A568	@{J}
.equ GetUnitEquippedWeapon, 0x8016B28	@{U}
@.equ GetUnitEquippedWeapon, 0x80168D0	@{J}

push {r4, lr}
mov r4, r0 
ldr r3, =CurrentUnit
ldr r3, [r3] 
cmp r3,  #0 
beq Exit 

ldrh r0, [r3, #0x26] @ Item slot 1 
ldr r2, =MemorySlot 
str r0, [r2, #0x4*3] @s3 


ldr r1, =CurrentUnitFateData	@these four lines copied from wait routine
mov r0, #0x1
strb r0, [r1,#0x11]

ldr r3, =ActionStruct @0x203A958
mov r0, #4 
strb r0, [r3, #0x12] @ Inventory slot #4 

mov r0, r4 
blh PromoteActiveUnit

ldr r0, =CurrentUnit 
ldr r0, [r0] 
blh GetUnitEquippedWeapon 
ldr r3, =Defender  
add r3, #0x48 
@strh r0, [r3] 
strh r0, [r3, #2] 

blh EndMMS 




Exit: 
mov r0, #0xB7
pop {r4} 
pop {r1}
bx r1 


.ltorg 
.align 







