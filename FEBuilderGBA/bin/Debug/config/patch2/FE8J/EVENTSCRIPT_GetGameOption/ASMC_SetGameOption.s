@
@ゲームオプションの設定
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
ldrb r1, [r3, #0x3]       @引数2 [option new value]
blh 0x080B6B7C	@SetGameOption	r0=OptionIndex	r1=OptionValue	{J}
@blh 0x080B1F64	@SetGameOption	r0=OptionIndex	r1=OptionValue	{U}

pop {r0}
bx r0
