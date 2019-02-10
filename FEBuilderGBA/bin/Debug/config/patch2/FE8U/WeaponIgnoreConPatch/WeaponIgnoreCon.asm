.thumb

MOV r2, #0x0
MOV r1, r0
MOV r0, r4

PUSH {r4}
LDR r4, WeaponList

ADD r0, #0x4A
LDRB r0, [r0, #0x0]
Loop:
  LDRB r3, [r4, r2]
  CMP r3, #0x0
  BEQ ConItem
    CMP r0, r3
    BNE NextIteration
      B NoConItem
    
  
NextIteration:
  ADD r2, #0x01
  B Loop
  
ConItem:
  POP {r4}
  MOV r0, #0x1A
  LDSB r0, [r4, r0]
  B Utils
  
NoConItem:
  POP {r4}
  MOV r0, #0x0

Utils:
  SUB r1, r1, r0
  ldr r3, BackToVanilla
  BX r3

.align
BackToVanilla:
  .long 0x802AB8B
.ltorg
WeaponList:
  @list of the weapons that ignore Con