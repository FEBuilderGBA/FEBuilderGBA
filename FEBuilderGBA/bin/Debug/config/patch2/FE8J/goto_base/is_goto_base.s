@Call 9783C	{J}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


@壊すコードの再送
blh 0x080977e4   //CanPrepScreenCheckMap	{J}
lsl r0 ,r0 ,#0x18
asr r0 ,r0 ,#0x18
cmp r0, #0x0
beq FlaseReturn

mov r0,#0xA4
blh 0x080860d0   //フラグ状態確認関数 RET=結果BOOL r0=確認するフラグ:FLAG	{J}
cmp r0,#0x0
bne FlaseReturn

TrueReturn:
ldr r3,=0x08097848|1
bx r3

FlaseReturn:
ldr r3,=0x08097870|1
bx r3

.ltorg
