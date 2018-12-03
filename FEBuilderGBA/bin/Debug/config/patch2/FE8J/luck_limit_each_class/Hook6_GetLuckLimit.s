.thumb
@Call 89540 @FE8J

mov r3, #0x19
ldsb r3, [r1, r3]  @RAMUnit->Luck
str  r0,[sp, #0x0]    @nazo

ldr  r0, [r1, #0x04]  @RAUnit->Class
ldrb r0, [r0, #0x04]  @Class->ClassID
LDR r2, LuckTable
LDRB r0, [r2, r0]

ldr r1, =0x08089548
mov pc, r1

.ltorg
.align
LuckTable:
