.thumb

push	{lr}

ldr  r0, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
ldrh r2, [r0, #0x2]       @引数1 songid

ldr r3, =0x02024E5C	@BGMSTRUCT	BGM	{J}
@ldr r3, =0x02024E5C	@BGMSTRUCT	BGM	{U}
strh r2, [r3, #0x4]		@再生しているBGM

pop {r0}
bx r0
