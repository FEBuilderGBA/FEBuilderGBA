.thumb
.org 0x2AA88
ldr r1, Table

Loop:
ldrb r2,[r1]
cmp  r2,r0
beq  TrueExit
add  r1,#0x02
cmp  r2,#0x00
bne  Loop

FalseExit:
b    0x02AAAE   @規定がないので3倍特攻

TrueExit:
mov  r0,r4
ldrb r2,[r1,#0x01]
mul  r0,r2
b    0x02AAB2   @規定に従った特攻



.ltorg
.align
Table:
