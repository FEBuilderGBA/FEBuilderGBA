.thumb
.org 0x533BE
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
b    0x0533E6

TrueExit:
mov  r0,#0x01
b    0x0533E6



.ltorg
.align
Table:
