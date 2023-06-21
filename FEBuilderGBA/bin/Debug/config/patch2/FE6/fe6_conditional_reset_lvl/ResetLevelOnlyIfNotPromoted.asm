.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr} 
mov r0, r4 
blh 0x80179EC @ guessing this is stat caps 
mov r0, #0
strb r0, [r4, #9] @ exp

ldr r3, =0x203956C @ action struct fe6 
ldrb r0, [r3, #0x12] @ inventory slot # 
lsl r0, #1 
add r0, #0x1c 
ldr r3, =0x2039214 @ bunit actor 
ldrb r0, [r3, r0] @ item used ? 

ldr r3, ResetLevelOnlyIfNotPromoted_Table
loop:
ldrb r1, [r3]
cmp r1, #0x0
beq ResetLevel

cmp r1, r0
beq NotResetLevel

add r3, #0x1
b   loop

ResetLevel: 
@ unsure of which check to use, as this is untested 
mov r0, #1 @ default case: store 1 
strb r0, [r4, #8] @ exp to 0 

NotResetLevel:
pop {r0} 
bx r0 

.ltorg 
ResetLevelOnlyIfNotPromoted_Table:
@sizeof(1)
@byte item_id
