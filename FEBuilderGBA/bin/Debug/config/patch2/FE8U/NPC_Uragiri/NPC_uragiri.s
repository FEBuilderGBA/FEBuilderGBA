@ローカルフラグ0x27をONにするとNPCが敵対する。
@When local flag 0x27 is turned on, NPC is hostile.
@Call 08024D8C

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

PUSH  {r0,r1,lr}

mov   r0, #0x27
blh   0x08083da8       @FE8U  CheckFlag
cmp   r0, #0x00
bne   Uragiri
mov   r2, #0x80        @ 仲間 (ﾟ∀ﾟ)人(ﾟ∀ﾟ) 
b     Join

Uragiri:
mov r2, #0xC0          @ 裏切り (・∀・) 

Join:
POP  {r1,r0}
and r1 ,r2
mov r3, #0x0
and r2 ,r0
cmp r2 ,r1
bne NotNPC

mov r3, #0x1

NotNPC:
mov r0 ,r3
POP  {pc}
