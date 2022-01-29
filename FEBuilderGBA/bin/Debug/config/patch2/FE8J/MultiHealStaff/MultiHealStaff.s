.thumb

@use r0
@Hook 0x16D64	@{J}
@Hook 0x16FBC	@{U}
mov r2 ,r1
mov r0, #0xff
and r0 ,r2
mov r1 ,r0
push {r2,r3}
mov r2, r1 @ItemID

ldr r0, =0x03004DF0	@CurrentUnit	{J}
@ldr r0, =0x03004E50	@CurrentUnit	{U}
ldr r3, [r0]

ldr r4, Table
sub r4, #0x4
Loop:
add r4, #0x4
ldr r0, [r4]
cmp r0, #0x0
beq NotFound

ldrb r0, [r4,#0x0] @Table->Item
cmp r0, r2
bne Loop

CheckUnit:
ldrb r0, [r4,#0x2] @Table->Unit
cmp  r0, #0x0
beq  CheckClass

cmp  r3,#0x0
beq  Loop
ldr  r1, [r3, #0x0]
ldrb r1, [r1, #0x4]
cmp  r0, r1
bne  Loop

CheckClass:
ldrb r0, [r4,#0x3] @Table->Class
cmp  r0, #0x0
beq  Found

cmp  r3,#0x0
beq  Loop
ldr  r1, [r3, #0x4]
ldrb r1, [r1, #0x4]
cmp  r0, r1
bne  Loop

Found:
ldrb  r4, [r4, #0x1]

pop {r2,r3}
ldr r0, =0x8016da0|1	@{J}
@ldr r0, =0x8016ff8|1	@{U}
bx r0


NotFound:
pop {r2,r3}

mov r4, #0x0
mov r0, #0xff
and r0 ,r2  @r0=item id

ldr r1, =0x8016D6C|1	@{J}
@ldr r1, =0x8016FC4|1	@{U}
bx r1

.ltorg
.align 4
Table:
