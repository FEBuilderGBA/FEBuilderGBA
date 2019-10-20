@Call 080B6AB8 FE8U

@r0 unit1を返す責任がある
@r1 r0と同様に、unit1を返す責任がある
@r2 temp
@r3 temp
@r4 reserve
@r5 reserve
@r6 reserve
@r7 reserve

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ldrb r0, [r4, #0x3]
cmp r0,#0x00
beq Found

blh  0x08083DA8     @FE8U CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq SkipData

Found:

@壊してしまうコードの再送
ldrb r0, [r4, #0x1]	@unit1
mov r1 ,r0			@unit1
cmp r1 ,r6
bne FlaseBrunch

ldr r3,=0x080B6AC0+1
bx r3

FlaseBrunch:
ldr r3,=0x080B6AC6+1
bx r3

SkipData:
ldr r3,=0x080B6AD0+1
bx r3
