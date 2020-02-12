@Hook 0803E7B8	{J}
@Hook 0803E828	{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@r0 keep
@r1 keep
@r2 work
@r3 keep
@r4 escape point

@r6 current


ldrb r2, [r4, #0x3]  @Flag
cmp  r2, #0x00
beq  Join

push {r0,r1,r3}
mov r0,r2
@blh 0x080860a8   @フラグを立てる関数 r0=立てるフラグ:FLAG {J}
blh 0x08083D80   @フラグを立てる関数 r0=立てるフラグ:FLAG {U}

ldr r0,[r6]      @current unit
@blh 0x08086784   @RunMoveEventsMaybe	{J}
blh 0x080844B0   @RunMoveEventsMaybe	{U}

pop {r0,r1,r3}

Join:

@壊すコードの再送
ldrb r2, [r4, #0x1]
str r2,[sp, #0x0]
ldrb r2, [r4, #0x2]
str r2,[sp, #0x4]

@ldr r2,=0x0803E7C0+1	@{J}
ldr r2,=0x0803E830+1	@{U}
bx r2
