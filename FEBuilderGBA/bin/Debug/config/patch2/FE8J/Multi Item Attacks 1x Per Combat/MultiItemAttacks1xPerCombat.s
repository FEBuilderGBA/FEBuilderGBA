@Call 2AF78
@r0    Item   / RetCode

.thumb
.org 0

ldr r3, MultiItemAttacks1xPerCombatTable

Loop:
ldrb r1, [r3]
cmp r1, #0x00
beq NotFound
cmp r0, r1
beq Found

add r3, #0x1
b   Loop

Found:
mov r0, #0x0
b Exit

NotFound:
mov r0, #0x1
b Exit

Exit:
ldr r3,=0x0802AF82+1
bx	r3

.ltorg
MultiItemAttacks1xPerCombatTable:
