.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@ConfigのColorを設定します
ldr r3, =0x0202BCEC @ChapterData	@{J}
@ldr r3, =0x0202BCF0 @ChapterData	@{U}
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
ldr r0, =0x080A4C38	@軍師名Pointer	@{J}
@ldr r0, =0x080A0E50	@軍師名Pointer	@{U}

ldr r0,[r0]
blh 0x08009fa8   @GetStringFromIndex	@{J}
@blh 0x0800a240   @GetStringFromIndex	@{U}

blh 0x08031438   @SetTacticianName	@{J}
@blh 0x080314ec   @SetTacticianName	@{U}

ldr r3, =0x080A4BD2|1	@{J}
@ldr r3, =0x080A0DEA|1	@{U}
bx r3

.ltorg
.align
ColorType:
