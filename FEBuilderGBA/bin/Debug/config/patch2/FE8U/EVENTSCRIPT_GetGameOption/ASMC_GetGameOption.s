@
@ゲームオプションの取得
@
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

push {lr}
ldr  r3, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

ldrb r0, [r3, #0x2]       @引数1 [game option]
@blh 0x080B6A00	@GetGameOption	RET=OptionValue	r0=OptionIndex	{J}
blh 0x080B1DE8	@GetGameOption	RET=OptionValue	r0=OptionIndex	{U}

@ldr r2, =0x030004B0  @MemorySlot {J}
ldr r2, =0x030004B8 @MemorySlot {U}
str	r0, [r2, #0x30]	@SlotCに結果を書き込む

pop {r0}
bx r0
