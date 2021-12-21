.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@ConfigのColorを設定します
@ldr r3, =0x0202BCEC @ChapterData	@{J}
ldr r3, =0x0202BCF0 @ChapterData	@{U}
add r3, #0x41

ldr r1, ColorType
lsl r1, #0x2
mov r2, #0xC
and r2, r1

ldrb r0, [r3]
mov  r1, #0xF3
and  r0, r1
orr  r0, r2
strb r0, [r3]

@壊すコードの再送.
@blh 0x08003c50   @Font_ResetAllocation	@{J}
blh 0x08003D20   @Font_ResetAllocation	@{U}
@blh 0x08003bc4   @Font_InitForUIDefault	@{J}
blh 0x08003c94   @Font_InitForUIDefault	@{U}

@ldr r3, =0x080B4160|1	@{J}
ldr r3, =0x080AF540|1	@{U}
bx r3

.ltorg
.align
ColorType:
