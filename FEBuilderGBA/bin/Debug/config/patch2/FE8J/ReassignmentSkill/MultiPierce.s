.thumb
@複数の貫通
@Hook 0802B218	@{J}

ldr  r3, Table
ldrb r1, [r0, #0x4]	@ClassID

Loop:
ldrb r2, [r3]
cmp  r2, #0x0
beq  FalseExit

cmp  r2, r1
beq  TrueExit

add  r3, #0x01
b    Loop

FalseExit:
ldr r3, =0x0802b24a|1	@{J}
bx  r3

TrueExit:
ldr r3, =0x0802B222|1	@{J}
bx  r3

.align
.ltorg
Table:
