.thumb
@let hp stealing show skill anim
@jumptohack at 80585bc
cmp r1, #0
beq return_nosteal
ldr r0, [r7]
mov r1, #0x80
lsl r1, #7
and r0, r1
cmp r0, #0
beq NoSkillAnim
ldrh r1, [r4]
mov r0, #0x80
lsl r0, #4
orr r0, r1
strh r0, [r4]

NoSkillAnim:
ldr r0, =0x203e108
mov r3, #0
ldsh r0, [r0, r3]
ldr r5, =0x80585c5
bx r5

return_nosteal:
ldr r3, =0x80586a1
bx r3
