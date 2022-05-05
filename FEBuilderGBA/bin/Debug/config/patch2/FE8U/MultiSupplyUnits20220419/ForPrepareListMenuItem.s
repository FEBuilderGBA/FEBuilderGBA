.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Hook	A1240	{J}
@Hook	9EFA0	{U}

@blh 0x08031568 @HasConvoyAccess	{J}
blh 0x0803161C @HasConvoyAccess	{U}
mov r2, #0x2
orr r2, r0

ldr r0, [r7, #0x2c]
mov r1, r8
ldrb r1, [r1, #0x0]

@blh 0x0809A4BC   @SomethingPrepListRelated	{J}
blh 0x080981e4   @SomethingPrepListRelated	{U}

@ldr r3, =0x080A124A|1	@{J}
ldr r3, =0x0809EFAA|1	@{U}
bx  r3
