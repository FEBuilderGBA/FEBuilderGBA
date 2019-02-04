@Call 2ACB6
@r0    Item   / RetCode

.thumb
.org 0

ldr r3, MultiProhibitsCritByItemTable

Loop:
ldrb r1, [r3]
cmp r1, #0x00
beq NotFound
cmp r0, r1
beq Found

add r3, #0x1
b   Loop

Found:
strh r4, [r5, #0x0]
b Exit

NotFound:
b Exit

Exit:
mov r1, #0x0
ldsh r0, [r5, r1]
ldr r3,=0x0802ACC0+1
bx	r3

.ltorg
MultiProhibitsCritByItemTable:
