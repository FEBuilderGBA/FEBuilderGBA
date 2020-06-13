.thumb
.org 0x0

@ r2: Unit
cmp r2, #0 @ If unit exists
bne Continue
ldr r3, =0x801027D
bx r3
Continue:
mov r1, #0x0B
ldsb r1, [r2, r1] @ allegiance byte
mov r0, #0xC0
and r1, r0 @ fetch allegiance
lsr r1, #6 @ Will return either 0(player), 1(npc), 2(enemy) or 3(purple)
ldr r0, =0x30004B8 @ gEventsSlot
str r1, [r0, #0x30] @ Store the result
ldr r3, =0x801028B
bx r3


.align 
.pool
