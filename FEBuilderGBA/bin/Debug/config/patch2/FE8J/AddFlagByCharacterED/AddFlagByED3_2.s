@Call 080BB5F8 FE8J

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

blh  0x080860D0     @FE8J CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq SkipData

Found:

@壊してしまうコードの再送
ldrb r0, [r4, #0x1]	@unit1
mov r1 ,r0			@unit1
cmp r1 ,r6
bne FlaseBrunch

ldr r3,=0x080BB600+1
bx r3

FlaseBrunch:
ldr r3,=0x080bb606+1
bx r3

SkipData:
ldr r3,=0x080BB610+1
bx r3
