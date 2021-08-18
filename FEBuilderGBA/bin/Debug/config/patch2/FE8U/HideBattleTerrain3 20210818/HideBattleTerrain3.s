@Hook 0x5B830 {J}
@Hook 0x5AA8C {U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


@壊すコードの再送
lsl r0 ,r0 ,#0x3
add r5 ,r0, r2
mov r1, #0x0
ldsh r0, [r6, r1]

push {r0}

@ldr r1,=0x0203E0FA	@CurrentBattleBG	{J}
ldr r1,=0x0203E0FE	@CurrentBattleBG	{U}
ldrb r1,[r1]
cmp r1, #0x00     @NO BG
beq FalseReturn
cmp r1, #0x3C     @Promotion Battle Background
beq FalseReturn

DrawBattleBackground:

ldr r0, [r6, #0x20]
mov r1, #0x0
ldr r2, =0x2000  @0x1000 * 2
@blh 0x080d6968   @memset	{J}
blh 0x080d1c6c   @memset	{U}

@ldr r3,=0x0805B860+1	@{J}
ldr r3,=0x805aabc+1	@{U}
b BackR3

FalseReturn:
@ldr r3,=0x0805B838+1	@{J}
ldr r3,=0x0805AA94+1	@{U}

BackR3:
pop {r0}
bx r3
