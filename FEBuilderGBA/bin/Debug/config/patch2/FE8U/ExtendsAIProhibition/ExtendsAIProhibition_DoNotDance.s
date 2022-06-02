@ORG 0x3ECD0	@{J}
@ORG 0x3ED40	@{U}
.thumb


@ldr r1, =0x0203AA00 @AiData@gAiData.aiUnits	@{J}
ldr r1, =0x0203AA04 @AiData@gAiData.aiUnits	@{U}

mov r2, #0x80
ldr r1, [r1, r2]	@gAiData.someFlag80

mov r2, #0x20
and r2, r1
beq Dance

mov r0, #0x0
b   Exit

Dance:
ldr r0, [r0, #0x0] @Current Unit
ldr r1, [r0, #0x0]
ldr r2, [r0, #0x4]
ldr r0, [r1, #0x28]
ldr r1, [r2, #0x28]
orr r0 ,r1

Exit:
@ldr r1, =0x0803ECDC|1	@{J}
ldr r1, =0x0803ED4C|1	@{U}
bx  r1
