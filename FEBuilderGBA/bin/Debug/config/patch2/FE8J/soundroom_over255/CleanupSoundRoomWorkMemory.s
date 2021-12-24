.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@ワークメモリのクリア
@Call B46C8	{J}
@     AFAA8	{U}
ldr r0, =0x0203A90C		@{J}
@ldr r0, =0x0203A910	@{U}

mov r1, #0x0
mov r2 ,#0x80   @ビットフラグで1000個入る領域を初期化します
blh 0x080D6968	@memset	{J}
@blh 0x080d1c6c		@memset	{U}

pop {r4}
pop {r0}
bx r0
