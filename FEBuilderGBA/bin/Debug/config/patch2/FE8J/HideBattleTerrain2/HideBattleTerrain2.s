//Hook 0x5B830 {J}
//Hook 0x5AA8C {U}
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

ldr	r1,	=0x0202BCEC	@ChapterData	{J}
@ldr	r1,	=0x0202BCF0	@ChapterData	{U}
mov	r3, #0x42
ldrb r1,[r1,r3]	@Get Anime settings
mov	r3, #0x6	@(if both 2 and 4 are set, combat animations are on with backgrounds on)
and r1,r3
cmp r1,#0x6
beq DrawBattleBackground

mov	r3, #0x4	@04=Combat animations solo
and r1,r3
cmp r1,#0x4
bne FalseReturn

@Battle Animation Solo Setting 
ldr r3,=0x0203a4e8	@BattleUnit	gBattleActor	{J}
@ldr r3,=0x0203A4EC	@BattleUnit	gBattleActor	{U}
ldrb r0,[r3,#0xD]	@BattleUnit->Status2
mov  r1,#0x40       @+40=単独アニメ1
and  r0,r1
cmp  r0,#0x40
beq  FalseReturn

DrawBattleBackground:

ldr r0, [r6, #0x20]
mov r1, #0x0
ldr r2, =0x2000  @0x1000 * 2
blh 0x080d6968   @memset	{J}
@blh 0x080d1c6c   @memset	{U}

ldr r3,=0x0805B860+1	@{J}
@ldr r3,=0x805aabc+1	@{U}
b BackR3

FalseReturn:
ldr r3,=0x0805B838+1	@{J}
@ldr r3,=0x0805AA94+1	@{U}

BackR3:
pop {r0}
bx r3
