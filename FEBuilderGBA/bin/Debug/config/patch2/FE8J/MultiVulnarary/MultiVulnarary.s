.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Hook 0802FE14	{J}
@Hook 0802FEC4	{U}

push {r4}
ldr r4, =0x0203A954	@ActionData	@{J}
@ldr r4, =0x0203A958	@ActionData	@{U}
ldrb r0, [r4, #0xc] @ActionData@gActionData.subjectIndex
blh 0x08019108   @GetUnitStruct	@{J}
@blh 0x08019430   @GetUnitStruct	@{U}

ldrb r1, [r4, #0x12]	@ActionData@gActionData.itemSlotIndex
lsl r1 ,r1 ,#0x1
add r0, #0x1e
add r0 ,r0, r1
ldrh r0, [r0, #0x0]
blh 0x08017294   @GetItemIndex	@{J}
@blh 0x080174ec   @GetItemIndex	@{U}

mov r2, r0  @ItemID

ldr r3, Table
sub r3, #0x2

Loop:
add r3, #0x2
ldrh r0, [r3]

cmp r0, #0x0
beq NotFound

ldrb r0, [r3,#0x0]
cmp  r0, r2
bne  Loop

Found:
ldrb r1, [r3,#0x1]
b    Exit

NotFound:
ldr r1, DeaultValue

Exit:
mov r0 ,r6
blh 0x0802f2a0   @ExecSomeSelfHeal	@{J}
@blh 0x0802f380   @ExecSomeSelfHeal	@{U}

pop {r4}
ldr r3,=0x802fec6|1	@{J}
@ldr r3,=0x802ff76|1	@{U}
bx r3

.ltorg
.align 4
Table:
.equ DeaultValue, Table + 4
