.thumb
@Hook 0x248D0

ldrb r0, [r4, #0x0] @item id
ldr r1, Table

Loop:
ldrb r2,[r1]
cmp  r2,r0
beq  TrueExit
add  r1,#0x02
cmp  r2,#0x00
bne  Loop

FalseExit:
@規定がないので3倍特攻
mov r0, #0x0
ldsh r1, [r6, r0]
lsl r0 ,r1 ,#0x1		@ダメージ3倍
add r0 ,r0, r1
strh r0, [r6, #0x0]

b    Exit

TrueExit:
ldrb r2,[r1,#0x01]

mov r0, #0x0
ldsh r1, [r6, r0]	@ダメージ
mul  r1,r2			@N倍
strh r1, [r6, #0x0]	@修正後のダメージを格納
@b    Exit

Exit:
ldr  r3, =0x080248D8|1
bx   r3

.ltorg
.align 4
Table:
