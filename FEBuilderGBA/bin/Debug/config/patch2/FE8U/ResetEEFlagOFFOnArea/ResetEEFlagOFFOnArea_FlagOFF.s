.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

mov r0, #0xEE
@blh 0x080860bc	@ResetFlag	@{J}
blh 0x08083D94	@ResetFlag	@{U}

@blh 0x08024d98   @GetPartyGoldAmount	@{J}
blh 0x08024de8   @GetPartyGoldAmount	@{U}
mov r4 ,r0
@blh 0x08031e24   @“¬‹Zê‚Ì‡Œ‹‰Ê‚Ìæ“¾	@{J}
blh 0x08031ed8   @“¬‹Zê‚Ì‡Œ‹‰Ê‚Ìæ“¾	@{U}

@ldr r3, =0x080BA6AA|1	@{J}
ldr r3, =0x80B5B26|1	@{U}
bx r3
