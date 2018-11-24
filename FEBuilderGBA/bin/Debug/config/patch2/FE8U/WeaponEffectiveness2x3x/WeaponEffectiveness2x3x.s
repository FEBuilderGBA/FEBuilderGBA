.thumb
.org 0x2ab18
ldr r1, Table

Loop:
ldrb r2,[r1]
cmp  r2,r0
beq  TrueExit
add  r1,#0x02
cmp  r2,#0x00
bne  Loop

FalseExit:
b    0x02AB3E   @規定がないので3倍特攻

TrueExit:
mov  r0,r5
ldrb r2,[r1,#0x01]
mul  r0,r2
b    0x02AB46   @規定に従った特攻



.ltorg
.align
Table:
