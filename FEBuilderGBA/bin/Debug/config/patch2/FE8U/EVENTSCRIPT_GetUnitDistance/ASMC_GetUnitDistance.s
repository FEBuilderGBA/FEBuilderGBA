.thumb
.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

.equ GetUnitStructFromEventParameter, 0x800BC51
.equ gEventSlot, 0x30004B8
.global GetUnitDistance
.type GetUnitDistance, %function
GetUnitDistance: @ ASMCed. Memory slots 0x1 and 0x2 are character IDs to check (supporting event parameters), returns in slot 0xC.

push { r4 - r6, lr }
ldr r6, =#gEventSlot
ldr r0, [ r6, #0x01 * 4 ] @ Memory slot 0x1.
blh GetUnitStructFromEventParameter, r1
cmp r0, #0x0
beq Error
mov r4, r0 @ Store the first unit struct in r4.

ldr r0, [ r6, #0x02 * 4 ] @ Memory slot 0x2.
blh GetUnitStructFromEventParameter, r1
cmp r0, #0x0
beq Error
mov r5, r0 @ Store the second unit struct in r5.

ldrb r0, [ r4, #0x10 ] @ X coords.
ldrb r1, [ r5, #0x10 ]
sub r3, r0, r1 @ Take the difference between the X coords.
cmp r3, #0x00
bge NotNegative1
	neg r3, r3
NotNegative1:
ldrb r0, [ r4, #0x11 ] @ Y coords.
ldrb r1, [ r5, #0x11 ]
sub r2, r0, r1
cmp r2, #0x00
bge NotNegative2
	neg r2, r2
NotNegative2:
add r2, r2, r3 @ Add the X and Y differences.
b   Exit

Error:
mov r2, #0x0   @ Error

Exit:
str r2, [ r6, #0xC * 4 ] @ Memory slot 0xC.

pop { r4 - r6 }
pop { r0 }
bx r0
