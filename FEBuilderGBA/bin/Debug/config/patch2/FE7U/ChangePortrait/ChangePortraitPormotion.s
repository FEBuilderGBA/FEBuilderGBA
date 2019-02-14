@Call 0x08069378 FE7U
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
blh 0x08018bd8       @FE7U GetUnitPortraitId
mov r4,r0

ldr r0,=0x080693C0   @FE7U LDR元アドレスを参照することでリポイントに耐える.
ldr r0,[r0]

blh 0x08006b50       @FE7U SetupFaceGfxData

Exit:
ldr r3,=0x08069380+1 @FE7U 戻るアドレス
bx  r3

.ltorg
.align
Table:
