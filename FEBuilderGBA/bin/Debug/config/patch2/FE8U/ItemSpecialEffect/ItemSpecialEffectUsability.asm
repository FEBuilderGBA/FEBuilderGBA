.thumb 

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm




.equ CheckEventId,0x8083da8

.global ItemSpecialEffectUsability
.type ItemSpecialEffectUsability, %function
ItemSpecialEffectUsability:
push {r4-r6, lr} 

mov r4, r0 @ item ID 
mov r5, r1 @ unit 

ldr r6, ItemSpecialEffectTable
sub r6, #12
FindValidItemLoop:
add r6, #12

@ word 0 0 0 as terminator 
ldr r0, [r6] 
cmp r0, #0 
bne Continue 
ldr r0, [r6,#4] 
cmp r0, #0 
bne Continue 
ldr r0, [r6,#8] 
cmp r0, #0 
bne Continue 
b RetFalse

Continue:
ldrb r0, [r6] @ item id 
cmp r0, r4 @ if they match, return true 
bne FindValidItemLoop 
ldrh r0, [r6, #4] @ flag 
cmp r0, #0 
beq SkipFlagCheck @ Always true if flag is 0 
blh CheckEventId
cmp r0, #1 
bne FindValidItemLoop 

SkipFlagCheck:
ldrb r0, [r6, #1] @ unit id
cmp r0, #0 
beq SkipUnitCheck
ldr r1, [r5]
ldrb r1, [r1, #4] 
cmp r0, r1 
bne FindValidItemLoop


SkipUnitCheck:

ldrb r0, [r6, #2] @ class id
cmp r0, #0 
beq SkipClassCheck
ldr r1, [r5, #4]
ldrb r1, [r1, #4] 
cmp r0, r1 
bne FindValidItemLoop


SkipClassCheck:

ldrb r0, [r6, #3] @ hp percentage 
cmp r0, #0 
beq SkipHealthCheck
ldrb r1, [r5, #0x12] @ max hp 
ldrb r2, [r5, #0x13] @ current hp 


mov r3, #100 
mul r2, r3 

mov r3, #0 @ counter 

DivLoop:
add r3, #1 
sub r2, r1 
cmp r2, r1 
bgt DivLoop
sub r3, #1
cmp r3, r0
bgt FindValidItemLoop

SkipHealthCheck:


RetTrue: 
mov r0, #1
b ExitUsability 

RetFalse: 
mov r0, #3 @ 3 is false lol 
b ExitUsability
@ if hover item = our item, return true 

ExitUsability:
mov r1, r6 @ Table entry to use 
pop {r4-r6} 
pop {r2} 
bx r2


.align
.ltorg

ItemSpecialEffectTable:

