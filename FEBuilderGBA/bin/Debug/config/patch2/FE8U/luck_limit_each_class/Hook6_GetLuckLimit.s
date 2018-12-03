.thumb
@Call 872A8 @FE8U

mov r3, #0x19
ldsb r3, [r1, r3]  @RAMUnit->Luck
str  r0,[sp, #0x0]    @nazo

ldr  r0, [r1, #0x04]  @RAUnit->Class
ldrb r0, [r0, #0x04]  @Class->ClassID
LDR r2, LuckTable
LDRB r0, [r2, r0]

ldr r1, =0x080872B0	@FE8U
mov pc, r1

.ltorg
.align
LuckTable:
