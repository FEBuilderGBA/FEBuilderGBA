@誰をCaptureしているのか?

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}

@ldr r1, =0x30004B8	@MemorySlot	{U}
ldr r1, =0x30004B0	@MemorySlot	{J}
ldr r0, [ r1, #0x01 * 4 ] @ Memory slot 0x1.
@blh 0x800bc50	@GetUnitStructFromEventParameter	{U}
blh 0x800BF3C	@GetUnitStructFromEventParameter	{J}
cmp r0, #0x0
beq FalseExit

ldr r1,[r0,#0x08]
mov r2,#0x10	@Resuce Flag
and r1,r2
bne FalseExit

ldrb r1, [r0, #0x1b] @AID
cmp r1,#0x00
beq FalseExit

sub r1,r1,#0x1
mov r2,#0x48
mul r1,r2

@ldr r2, =0x202be4c	@{U}
ldr r2, =0x202BE48	@{J}
add r2,r1

ldr r1,[r2,#0x0]
cmp r1, #0x0
beq FalseExit

ldrb r0,[r1,#0x04]

b   Exit

FalseExit:
mov r0, #0x0

Exit:
@ldr r1, =0x30004B8	@MemorySlot	{U}
ldr r1, =0x30004B0	@MemorySlot	{J}
str r0, [r1, #0x0C * 4]

pop {r1}
bx r1
