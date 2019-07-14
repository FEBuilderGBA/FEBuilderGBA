.thumb
.org 0x0
ldr r1, Table

Loop:
ldrb r2,[r1]
cmp  r2,r0
beq  TrueExit
add  r1,#0x02
cmp  r2,#0x00
bne  Loop

FalseExit:
ldr  r3,=0x0802AB3E+1   @規定がないので3倍特攻
bx   r3

TrueExit:
mov  r0,r5
ldrb r2,[r1,#0x01]
mul  r0,r2
ldr  r3,=0x0802AB46+1   @規定に従った特攻
bx   r3

.ltorg
.align
Table:
