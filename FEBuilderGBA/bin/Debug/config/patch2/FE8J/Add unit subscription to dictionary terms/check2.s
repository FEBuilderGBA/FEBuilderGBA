@Call D380C	{J}
@Call CEB10	{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ldrh r0, [r5, #0x6]
blh 0x080860d0	@CheckFlag	{J}
@blh 0x08083DA8	@CheckFlag	{U}
cmp r0, #0x0
beq FalseExit

ldrb r0, [r5, #0xA] @Unit
cmp  r0,#0x0
beq  TrueExit

blh 0x08017fb0  @GetUnitByCharId	{J}
@blh 0x0801829c  @GetUnitByCharId	{U}
cmp r0, #0x00
beq FalseExit

ldrb r1, [r0, #0xB]
cmp  r1,#0x40    @プレイヤーユニットかどうか
bge  FalseExit

ldrb r1, [r0, #0xC]
mov  r2, #0x04
and  r1, r2      @ユニットが死亡していたらダメ
bne  FalseExit

ldrb r1, [r5, #0xB] @Class
cmp  r1,#0x0
beq  TrueExit

ldr r2,[r0,#0x4] @Class
ldrb r2,[r2,#0x4] @Class->ID
cmp r1,r2
bne FalseExit

TrueExit:
ldr r3,=0x080D3818|1	@{J}
@ldr r3,=0x080CEB1C|1	@{J}
bx r3

FalseExit:
ldr r3,=0x080d381e|1	@{J}
@ldr r3,=0x080ceb22|1	@{J}
bx r3
