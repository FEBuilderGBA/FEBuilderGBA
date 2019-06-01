@
@r0 Procs
@r1 CurrentUnit
@
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

@Hook  08032108     @{J}
@      080321BC     @{U}

ldrb r2,[r1,#0x0b]  @Unit->所属
cmp  r2,#0x80
bge  Skip

blh  0x08037888     @{J}
@blh  0x080377f0    @{U}

lsl r0 ,r0 ,#0x18
asr r0 ,r0 ,#0x18
b    Exit

Skip:
mov r0,#0x00

Exit:
pop {r1}
bx r1
