.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ proc_truehit, 0x802A558
.equ d100Result, 0x802a52c
@ r0 is attacker, r1 is defender, r2 is current buffer, r3 is battle data
push {r4-r7,lr}
mov r4, r0 @attacker
mov r5, r1 @defender
mov r6, r2 @battle buffer
mov r7, r3 @battle data

mov r1, #4
ldrsh r0, [r7,r1] @damage
cmp r0, #0x7f
ble UnderDMGCap
mov r0, #0x7f
strh r0, [r7,r1] @damage is capped at 127
UnderDMGCap:
cmp r0, #0
bge OverDMGFloor
mov r0, #0
strh r0, [r7,r1] @damage is floored at 0
OverDMGFloor:
mov r0, r4
add r0, #0x48
ldrb r0, [r0]
cmp r0, #0xb5 @stone
bne NotStone
mov r0, #0
strh r0, [r7, #4]
NotStone:
mov r0, #4
ldrsh r0, [r7, r0]
cmp r0, #0
beq End @tink = no exp for you
ldr r0, [r6]
mov r1, #2 @miss flag
tst r1, r0
bne End @missed = no exp for you
mov r1, r4
add r1, #0x7c
mov r0, #1
strb r0, [r1] @set hit flag 

End:
pop {r4-r7}
pop {r15}
