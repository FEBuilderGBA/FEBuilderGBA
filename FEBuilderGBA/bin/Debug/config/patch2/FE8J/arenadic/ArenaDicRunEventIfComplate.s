.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.macro blh_ to, reg=r3
  push {\reg}
  ldr \reg, =\to
  mov lr, \reg
  pop {\reg}
  .short 0xf800
.endm

@コンプリートイベント
ComplateEvent:
push {r4,lr}
mov  r4, r0	@this procs

@総数
ldrh r0, [r4, #0x2C]	@thisProcs->AllCount

@達成数
ldrh r1, [r4, #0x2E]	@thisProcs->ComplateCount

cmp r0, r1
bne ComplateEvent_Exit	@コンプリートしていないならボツ

ldr r3, ArenaDicConfig
ldrh r0, [r3, #0x8]	@ArenaDicConfig->全達成時の達成フラグ
cmp  r0, #0x0
beq  ComplateEvent_Exit	@達成フラグ0だと毎回起きてしまうのはよくないので、ボツにする

blh 0x080860D0	@CheckFlag
cmp r0, #0x0
bne ComplateEvent_Exit	@既に発生済みなのでボツ

@フラグを有効にしてイベントを呼び出す
ldr r3, ArenaDicConfig
ldrh r0, [r3, #0x8]	@ArenaDicConfig->全達成時の達成フラグ
blh  0x080860A8	@SetFlag

ldr r3, ArenaDicConfig
ldr r0, [r3, #0xC]	@ArenaDicConfig->全達成時のイベント
cmp r0, #0x1
ble ComplateEvent_Exit	@イベントアドレスが無効

mov r1, #0x2
blh 0x0800d340   @イベント命令を動作させる関数	{J}
@blh 0x0800d07c   @イベント命令を動作させる関数	{U}

ComplateEvent_Exit:
pop {r4}
pop {r0}
bx r0



.ltorg
DATA:
.equ	ArenaDicConfig,	DATA+0
