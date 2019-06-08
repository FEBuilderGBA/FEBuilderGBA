.thumb
.macro blh to, reg=r6
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ ClearMapWith, 0x080197E4
.equ GetUnitRangeMask, 0x080171E8
.equ FillRangeByRangeMask, 0x0801B460
.equ DisplayMoveRangeGraphics, 0x0801DA98

push {r4, r5, r6, lr}

@place this before any func calls
ldr r6, =0x0203F082     @marking this as using spell menu
mov r0, #0x1
strb r0, [r6]

ldr r0, =0x0202E4E0
ldr r0, [r0]
mov r5, #0x1
neg r5, r5
mov r1, r5
blh ClearMapWith
ldr r0, =0x0202E4E4
ldr r0, [r0]
mov r1, #0x0
blh ClearMapWith
ldr r4, =0x03004E50
ldr r0, [r4]
mov r1, r5
blh GetUnitRangeMask
mov r1, r0
ldr r0, [r4]
blh FillRangeByRangeMask
mov r0, #0x3
blh DisplayMoveRangeGraphics
mov r0, #0x0

Exit:
pop {r4, r5, r6}
pop {r1}
bx r1