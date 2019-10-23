

.thumb
.org 0

@@ DESCRIPTION:
@@ This FE8 hack will write the value of an offset stored in Slot 3 into Slot 2.
@@ Slot 4 value 0/1/2 determines word/short/byte.

Event_Reader:
ldr r0, Slot_Pointers
ldr r1, [r0, #0xC] @Load the address stored in Slot 3 into r1
ldr r3, [r0, #0x10] @Load the value of Slot 4 into r3
cmp r3,#0x0       @if 0, read a word
beq Read
cmp r3,#0x1       @if 1, read a short
beq Read_S
cmp r3,#0x2       @if 2, read a byte
beq Read_B
b End

Read:
ldr r2, [r1] @Load the value at that address into r2
str r2, [r0,#0x8] @write the value into Slot 2
b End

Read_S:
ldrh r2, [r1]
str r2, [r0,#0x8]
b End

Read_B:
ldrb r2, [r1]
str r2, [r0,#0x8]

End:
bx lr

.align
Slot_Pointers:
.long 0x030004B8 @address of slot 0

