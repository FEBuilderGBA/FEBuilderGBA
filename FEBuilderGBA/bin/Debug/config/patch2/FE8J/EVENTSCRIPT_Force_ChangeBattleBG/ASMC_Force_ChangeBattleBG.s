.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push	{r4, lr}

ldr  r0, [r0, #0x38]      @イベント命令にアクセスらしい [r3,#0x38] でイベント命令が書いてあるアドレスの場所へ
ldrh r0, [r0, #0x2]       @引数1 40 0D [引数1] [引数2] [プログラム場所 XX XX XX 08]
cmp  r0,#0x0
beq  Exit

ldr  r3,=0x0203E0FA	@{J}
strh  r0, [r3]

sub r0, #0x1
blh 0x08077f0c   @DrawBattleBG	{J}

Exit:
pop {r4}
pop {r0}
bx r0
