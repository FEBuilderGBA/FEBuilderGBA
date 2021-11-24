.thumb 

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ CurrentUnit, 0x3004E50
.equ CheckEventId,0x8083da8

push {r4-r7, lr}

mov r7, r0 @ some ram address that vanilla uses 
@r1 is item durability & id 

mov r3, #0
lsl r2, #24 
asr r2, #24 
mov r8, r2 @ allocated in the vanilla part before this 


@ do our custom stuff here 
ldr r5, =CurrentUnit 
ldr r5, [r5]


mov r4, #0xFF 
and r4, r1 @ item id 

mov r0, r4 @ item 
mov r1, r5 @ unit 
ldr r3, ItemSpecialEffectUsability
mov lr, r3
.short 0xf800 


@ if hover item = our item, return true 

cmp r0, #1 
bne Vanilla
mov r2, #1 
mov r8, r2 
Vanilla:

mov r2, r8 
mov r3, #0 



cmp r2, #0 
bne Skip
mov r3, #1 
Skip: 
mov r0, r7 
mov r1, #0 
mov r2, r3 
strb r1, [r0, #2] 
strb r2, [r0, #3] 








pop {r4-r7}
pop {r3} 
@ldr r3, =0x801686D 
bx r3 

.align 
.ltorg 
ItemSpecialEffectUsability:


