@Hook	08019388	FE6

.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

@壊すコードの再送
lsl r4 ,r1 ,#0x18
asr r4 ,r4 ,#0x18

ldrb r3, [r5,#0xb] @RAMUnit->所属
cmp  r3, #0x40     @RAMUnit->所属 <= 0x40
blt NormalMove

@if (RAMUnit->AI2 == 0x03 && (RAMUnit->AI1 == 0x03 || RAMUnit->AI4 == 0x20) )
mov  r3, #0x44     @
ldrb r3, [r5,r3]  @RAMUnit->AI2
cmp  r3, #0x3      @AI2 == 03=Do not move
bne NormalMove

mov  r3, #0x42     @
ldrb r3, [r5,r3]  @RAMUnit->AI1
cmp  r3, #0x3      @AI1 == 03=隣接のみ
beq Match

mov  r3, #0x42     @
ldrb r3, [r5,r3]  @RAMUnit->AI1
cmp  r3, #0x6      @AI1 == 06=AI2のみ
beq Match

mov  r3, #0x41     @
ldrb r3, [r5,r3]  @RAMUnit->AI4
cmp  r3, #0x20     @AI4 == 20=BossAI
bne NormalMove

Match:
ldr r0,=0x084AA7B0	@0xFFがたくさんある楽器データの内部を再利用しています
b   Exit

NormalMove:
ldr r0, [r5, #0x4]
ldr r0, [r0, #0x34]

Exit:
ldr r3,=0x08019390|1
bx  r3
