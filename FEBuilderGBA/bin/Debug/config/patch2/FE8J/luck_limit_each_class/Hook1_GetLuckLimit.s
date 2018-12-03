.thumb
@Call 17f60    @FE8J

ldrb r0, [r4, #0x19] @RAMUnit->Luck
ldrb r1, [r5, #0x4]  @Class->ClassID  GetClassID
ldr r2, LuckTable
ldrb r1, [r2, r1]    @LuckTable[ClassID]
CMP r0 ,r1
BLE Exit
	@幸運上限に達成
	MOV r0 ,r1
	STRB r0, [r4, #0x19]

Exit:
ldr r3,=0x08017F6C
mov pc,r3

.ltorg
.align
LuckTable:
