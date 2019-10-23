

.thumb
.org 0

@@ DESCRIPTION:
@@ This FE8 hack will write the value of Slot 2 into the address at Slot 3.
@@ Slot 4 value 0/1/2 determines word/short/byte.

Event_Writer:
ldr r0, Slot_Pointers
ldr r1, [r0, #0x8] @Load the value of Slot 2 into r1
ldr r2, [r0, #0xC] @Load the value of Slot 3 into r2
ldr r3, [r0, #0x10] @Load the value of Slot 4 into r3
cmp r3,#0x0       @if 0, write a word
beq Write
cmp r3,#0x1       @if 1, write a short
beq Write_S
cmp r3,#0x2       @if 2, write a byte
beq Write_B
b End

Write:
str r1, [r2,#0x0] @write the value of Slot 2 into the offset at Slot 3
b End

Write_S:
strh r1, [r2,#0x0]
b End

Write_B:
strb r1, [r2,#0x0]

End:
bx lr

.align
Slot_Pointers:
.long 0x030004B8 @address of slot 0

