@Call 9783C	{J}
@Call 9555C	{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


@壊すコードの再送
@blh 0x080977e4   @CanPrepScreenCheckMap	{J}
blh 0x08095504   @CanPrepScreenCheckMap	{U}
lsl r0 ,r0 ,#0x18
asr r0 ,r0 ,#0x18
cmp r0, #0x0
beq FlaseReturn

mov r0,#0xA4
@blh 0x080860d0   @フラグ状態確認関数 RET=結果BOOL r0=確認するフラグ:FLAG	{J}
blh 0x08083DA8   @フラグ状態確認関数 RET=結果BOOL r0=確認するフラグ:FLAG	{U}
cmp r0,#0x0
bne FlaseReturn

TrueReturn:
@ldr r3,=0x08097848|1	@{J}
ldr r3,=0x08095568|1	@{U}
bx r3

FlaseReturn:
@ldr r3,=0x08097870|1	@{J}
ldr r3,=0x08095590|1	@{U}
bx r3

.ltorg
