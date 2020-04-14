@0802A298
.thumb

and r0 ,r1         @‰ó‚·ƒR[ƒh‚ÌÄ‘—

ldrb r1, [r3,#0x4] @class id
lsl  r1,r1,#0x1    @ *2

cmp r0, #0x0
beq GetValue

Promotied:
add  r1,r1,#0x1

GetValue:
ldr  r2, Table
ldrb r2, [r2, r1]
add  r4, r2

mov r0 ,r4
pop {r4}
pop {r1}
bx r1

.ltorg
.align
Table:
