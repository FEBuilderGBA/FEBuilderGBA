.thumb
@Call 2C016 @FE8U

MOV r4, #0x4
ldrb r1, [r5, #0x4]  @Class->ClassID  GetClassID
ldr r4, LuckTable
ldrb r4, [r4, r1]   @LuckTable[ClassID]

cmp r0 ,r4
ble Exit
    @幸運上限に達成
    ldrb r1, [r2, #0x19] @RAMUnit->Luck
    sub r4 ,r4, r1
    strb r4, [r3, #0x0]  @上限との差分を格納する

Exit:
pop {r4,r5}
pop {r0}
bx r0

.ltorg
.align
LuckTable:
