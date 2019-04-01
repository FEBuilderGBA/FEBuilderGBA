@Call 0x08069B58 FE7J
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
blh 0x08018FC8       @FE7J GetUnitPortraitId
mov r4,r0

ldr r0,=0x08069BA0   @FE7J LDR元アドレスを参照することでリポイントに耐える.
ldr r0,[r0]

blh 0x080069e0       @FE7J SetupFaceGfxData

Exit:
ldr r3,=0x08069B60+1 @FE7J 戻るアドレス
bx  r3

.ltorg
.align
Table:
