.thumb
.macro bll to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm

@0800450C から呼び出される. FE8U
@r5 fontbase?
@r4 text

@r5 fontbase?
@r4 text

mov r1,r4
bll UTF8_COMMON
mov r4,r1 @String
mov r1,r0 @Font
          @r3 TextParams

@r1 = fontdata ない場合は #0x00
@r3 = TextParams
@r4 = 次のtext
@r5 = fontbase
ldr r0,=0x0800451A @FE8U
mov pc,r0

.ltorg
.align
UTF8_COMMON:
