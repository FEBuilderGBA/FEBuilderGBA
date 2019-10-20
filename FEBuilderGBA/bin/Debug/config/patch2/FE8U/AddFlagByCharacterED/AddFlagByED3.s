@Call 080B6AFC FE8U

@r0 演算値を返す責任がある
@r1 temp
@r2 unit1を返す責任がある
@r3 reserve
@r4 reserve
@r5 reserve

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ldrb r0, [r3, #0x3]
cmp r0,#0x00
beq Found

blh  0x08083DA8     @FE8U CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq SkipData

Found:
ldr r3, [r4, #0x30] @破壊してしまうr3の復元
                    @壊してしまうコードの再送
ldrb r2, [r3, #0x1] @unit1
lsr r0 ,r2 ,#0x5
lsl r0 ,r0 ,#0x2
add r0 ,r0, r4

ldr r1,=0x080B6B04+1
bx r1

SkipData:
ldr r0,=0x080b6bca+1
bx r0
