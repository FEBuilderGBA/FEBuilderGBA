.thumb 

.macro blh to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm


.equ gActionData, 0x0203A954	@{J}
.equ CurrentUnit, 0x03004DF0	@{J}
.equ CheckEventId,0x080860D0	@{J}

push {lr} 




ldr r3, =CurrentUnit 
ldr r3, [r3]
ldr r1, =gActionData 
ldrb r2, [r1, #0x12] @ inventory slot # 
lsl r2, #1 @ 2 bytes per inv slot 
add r2, #0x1E 
add r2, r3 @ unit ram address of actual item to load 
ldrb r0, [r2] @ item id 

mov r1, r3 @ unit 

ldr r3, ItemSpecialEffectUsability
mov lr, r3 
.short 0xF800 



pop {r2} 
bx r2


.align
.ltorg

ItemSpecialEffectUsability:

