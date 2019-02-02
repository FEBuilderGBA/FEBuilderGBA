.thumb
@ Hook  5A15C @ r1

LDR r0, =0x02029000
CMP r0 ,r7
BLS Step1
    LDR r1, =0x0203E114 @ gBattleSpellAnimationId1
    B Step2
Step1:
LDR r1, =0x0203E114 @ gBattleSpellAnimationId1
ADD r1, #0x2

Step2:
LDRB r0, [r1, #0x0] @ gBattleSpellAnimationId2
CMP r0, #0x1        @ アニメモーションで手斧以外
BGT Step3
   LDR r1, =0x02017758
   LDR r0, [r1, #0x0]
   CMP r0, #0x1     @ 手斧モーション
   BEQ Step3
      LDR r0, =0x0805A164
      MOV PC, r0
Step3:
LDR r0, =0x0805A164+2  @復帰 
LDR r1, =0x02017758
MOV PC, r0
