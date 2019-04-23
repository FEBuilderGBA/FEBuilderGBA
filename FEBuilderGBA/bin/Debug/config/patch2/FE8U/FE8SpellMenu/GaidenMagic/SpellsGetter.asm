.thumb
@r0 = character data in ram
@return = pointer to 0-terminated list of spells
@b33078

.set SpellsBuffer, 0x202b6d0
.set BWLTable, 0x203e884

push {r4-r7, lr}
mov r4, r0 @save chardata to r4
ldr r5, =SpellsBuffer

mov r0, r4
ldrb r7, [r0, #0x8] @unit level
ldr r1, [r0, #0x4] @class
mov r2, #0x29
ldrb r1, [r1, r2] @class ability 2
mov r2, #0x1 @check "promoted"
and r1, r2
cmp r1, #0x1
bne NotPromoted
add r7, #0x80 @add 0x80 to level if promoted

NotPromoted:
ldr r1, [r0, #0x0] @character data
ldrb r1, [r1, #0x4] @character id
ldr r2, SpellAssociationTable
lsl r1, #0x2
add r1, r2
ldr r6, [r1] @pointer to spell list
cmp r6, #0x0
beq TerminateList

CheckLoop:
ldrb r0, [r6]
cmp r0, #0x0
beq TerminateList
cmp r0, r7 @is unit at level to use spell?
ble FoundSpell
add r6, #0x2
b CheckLoop

FoundSpell:
ldrb r1, [r6, #0x1]
strb r1, [r5]
add r5, #0x1
add r6, #0x2
b CheckLoop

TerminateList:
mov r0, #0x0
strb r0, [r5]
mov r1, r5 @end of spell buffer
ldr r0, =SpellsBuffer
sub r1, r0 @number of spells

pop {r4-r7}
pop {r2}
bx r2

.ltorg
SpellAssociationTable:
@POIN SpellAssociationTable