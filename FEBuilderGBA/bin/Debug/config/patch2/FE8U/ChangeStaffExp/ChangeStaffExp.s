@Hook 0x2C588	{J}
@Hook 0x2C650	{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

lsr r0 ,r0 ,#0xd
mov r1, #0x2
and r0 ,r1
cmp r0, #0x0
bne ExitMiss

mov r0 ,r4
add r0, #0x48
ldrb r0, [r0, #0x0] @GetItemID

ldr r3, Table
sub r3, #0x2
Loop:
add r3, #0x2
ldrh r1,[r3]
cmp  r1,#0x0
beq  NotFound

ldrb r1,[r3]
cmp  r1, r0
bne  Loop

Found:
ldrb r2,[r3,#0x01]
cmp  r2, #0x00 @Default
beq  NotFound

@ldr  r3, =0x0802C5CA|1	@{J}
ldr  r3, =0x0802C692|1	@{U}
bx   r3

NotFound:
@ldr  r3, =0x0802C59C|1	@{J}
ldr  r3, =0x0802C664|1	@{U}
bx   r3

ExitMiss:
@ldr  r3, =0x0802C592|1	@{J}
ldr  r3, =0x0802C65A|1	@{U}
bx   r3


.ltorg
Table:
