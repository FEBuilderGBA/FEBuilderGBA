@誰をCaptureしているのか?

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}

ldr r1, =0x30004B8	@MemorySlot	{U}
@ldr r1, =0x30004B0	@MemorySlot	{J}
ldr r0, [ r1, #0x01 * 4 ] @ Memory slot 0x1.
blh 0x800bc50	@GetUnitStructFromEventParameter	{U}
@blh 0x800BF3C	@GetUnitStructFromEventParameter	{J}
cmp r0, #0x0
beq FalseExit

ldr r1,[r0,#0x0C]
mov r2,#0x10	@Resuce Flag
and r1,r2
@cmp r1,#0x0	@not required this.
beq FalseExit

ldrb r0, [r0, #0x1b] @AID
cmp r0,#0x00
beq FalseExit

ldr r1, =0x0859A5D0 @{U}	RAMSlotTable This is a table of pointers, with one for every deployment slot in RAM )
@ldr r1, =0x085C2A50 @{J}	RAMSlotTable This is a table of pointers, with one for every deployment slot in RAM )
lsl r0 ,r0 ,#0x2
add r0 ,r0, r1

ldr r0, [r0, #0x0]
ldr r0, [r0, #0x0]
cmp r0, #0x00
beq FalseExit

ldrb r0,[r0,#0x04]

b   Exit

FalseExit:
mov r0, #0x0

Exit:
ldr r1, =0x30004B8	@MemorySlot	{U}
@ldr r1, =0x30004B0	@MemorySlot	{J}
str r0, [r1, #0x0C * 4]

pop {r1}
bx r1
