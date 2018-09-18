@Call 0x08076244 FE8J
@r0 ram unit
@
@
@なぜかCC時は、直参照をしているので補正する.
@

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
.org 0x00

mov r0,r1
blh 0x08018FCC       @FE8J GetUnitPortraitId
mov r4,r0

ldr r0,=0x0807628C   @FE8J LDR元アドレスを参照することでリポイントに耐える.
ldr r0,[r0]

blh 0x0800544c       @FE8J SetupFaceGfxData

Exit:
ldr r3,=0x0807624C+1 @FE8J 戻るアドレス
bx  r3

.ltorg
.align
Table:
