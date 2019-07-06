.thumb
.org 0x58AD2
cmp  r0,#0x00
beq  FalseExit

ldr r1, Table

Loop:
ldrb r2,[r1]
cmp  r2,r0
beq  TrueExit
add  r1,#0x01
cmp  r2,#0x00
bne  Loop

FalseExit:
mov  r0,#0x00
b    Return

TrueExit:
mov  r0,#0x01

Return:
pop  {r1}
bx   r1



.ltorg
.align
Table:
