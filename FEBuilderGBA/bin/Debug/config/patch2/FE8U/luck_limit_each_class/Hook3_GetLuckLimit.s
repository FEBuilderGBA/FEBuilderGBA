.thumb
@Call 926F2 @FE8U

LDR r0, [r4, #0x0]
LDR r1, [r0, #0x0]

mov r0,#0x19
LDSB r0, [r1, r0] @RAMunit->Luck
MOV r6 ,r1
LDR r2, LuckTable

LDR r1, [r1, #0x4]  @RAMUnit->Class  GetClass
LDRB r1, [r1, #0x4] @Class->ClassID  GetClassID
LDRB r1, [r2, r1]   @LuckTable[ClassID]
CMP r0 ,r1
BNE Else
	MOV r1 ,r6
	MOV r6, #0x4
	b Exit

Else:
	MOV r1 ,r6
	MOV r6, #0x2

Exit:
ldr r3, =0x08092702
mov pc, r3

.ltorg
.align
LuckTable:
