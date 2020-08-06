.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}

mov r0, #0x4
@blh 0x08002330	@FadeOutChannelForSoundSE	{J}
blh 0x080023e0	@FadeOutChannelForSoundSE	{U}

pop {r0}
bx r0
