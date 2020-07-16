@Call 080CA278	{J}
@Call 080C54B0	{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4}

ldr r4, ARGS
ldrb r0, [r4, #0x3]	@format
cmp r0,#0x00
beq Exit

ldr r0, =0x06017800	@{J} {U}   VRAMの下の方
ldrb r1, [r4, #0x2]	@pal
@blh 0x08003a70   @SetupDebugFontForOBJ	{J}
blh 0x08003B24   @SetupDebugFontForOBJ	{U}

ldrb r0, [r4, #0x0]	@x
lsl r0,r0,#0x3      @*8
ldrb r1, [r4, #0x1]	@y
lsl r1,r1,#0x3      @*8
@ldr r2, =0x080DC110 @(ビルド時刻 =>　2004/09/09(THU) 13:12:56 )	{J}
ldr r2, =0x080D74D0 @(ビルド時刻 =>　2004/09/09(THU) 13:12:56 )	{U}
@blh 0x08003AFC @PrintDebugStringAsOBJ	{J}
blh 0x08003BB0 @PrintDebugStringAsOBJ	{U}

Exit:

pop {r4}

@壊すコードの再送
mov r0 ,r5
add r0, #0x4c
mov r1, #0x0
ldsh r0, [r0, r1]
mov r1, #0x3

@ldr r3, =0x080CA282|1	@{J}
ldr r3, =0x080C54BA|1	@{U}
bx r3

.ltorg
ARGS:
