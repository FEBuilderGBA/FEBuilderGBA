.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Hook	A13FC	{J}
@Hook	9F15C	{U}

blh 0x08031568 @HasConvoyAccess	{J}
@blh 0x0803161C @HasConvoyAccess	{U}
mov r2, #0x2
orr r2, r0

ldr r0, [r4, #0x2c]
add r5, #0x33
ldrb r1, [r5, #0x0]

blh 0x0809A4BC   @SomethingPrepListRelated	{J}
@blh 0x080981e4   @SomethingPrepListRelated	{U}

ldr r3, =0x080A1406|1	@{J}
@ldr r3, =0x0809F166|1	@{U}
bx  r3
