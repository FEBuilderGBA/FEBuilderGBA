.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@ 3E1B0 is Bl AiGetPositionUnitSafetyWeight 
@.equ AiGetPositionUnitSafetyWeight, 0x803E114 
@ .equ AiGetTileWeightForAttack, 0x803DE5C called by AiTrySimulateBattle 

@ FE8U
.equ AiData, 0x203AA04	@{U}
.equ ClearMapWith, 0x80197E4	@{U}
.equ gMapMove2, 0x202E4F0	@{U}
.equ FillAiDangerMap, 0x803E320	@{U}
.equ gpAiBattleWeightFactorTable, 0x30017D8	@{U}
.equ Attacker, 0x203A4EC	@{U}

@ FE8J
@.equ AiData, 0x203AA00	@{J}
@.equ ClearMapWith, 0x80194BC	@{J}
@.equ gMapMove2, 0x202E4EC	@{J}
@.equ FillAiDangerMap, 0x803E2B0	@{J}
@.equ gpAiBattleWeightFactorTable, 0x30017D0	@{J}
@.equ Attacker, 0x203A4E8	@{J}

@.equ ActionStruct, 0x203A958
@.equ gActiveUnit, 0x03004E50	@{U}
@
@.equ AiDecision, 0x203AA94 @ [203AA96..203AA97]!!

ldr r2, =AiData+0x7A 
ldrb r1, [r2] 
cmp r1, #0 
beq Start @ version that calls 
ldr r3, =gpAiBattleWeightFactorTable 
ldr r1, [r3] 
cmp r1, #0 
beq RetZero 
ldrb r1, [r1, #6] @ Penalty: range and attack power of opponents 
cmp r1, #0 
beq RetZero 
ldrb r0, [r0] 
lsr r1, r0, #3 
RetZero: 
bx lr 

Start: 
push {r4-r5, lr} 
@ [20304A0..2030800]?!!

mov r4, r0 @ coordinate of gMapMove2, which we may put damage taken into 
mov r0, #1 
strb r0, [r2] 
ldr r3, =gpAiBattleWeightFactorTable 
ldr r3, [r3] 
ldrb r5, [r3, #6] @ Penalty: range and attack power of opponents 
cmp r3, #0 
beq VanillaBehaviour @ if pointer is false, then vanilla 
cmp r5, #0 
bne VanillaBehaviour 
mov r4, #0 
b End 

VanillaBehaviour: 
@mov r11, r11 

ldr r0, =gMapMove2 
ldr r0, [r0] 
mov r1, #0 
blh ClearMapWith 
blh FillAiDangerMap 
ldrb r4, [r4] 
lsr r4, #3 
@mov r11, r11 
End: 
mov r0, r4 
mov r1, r5 
mul r1, r0 


pop {r4-r5} 
pop {r2} 
bx r2 
@ returns value in r1 
.ltorg 














