@HOOK 08085A64	{J}
@HOOK 0808372C	{U}
@支援会話がない場合でも支援レベルが上がったというポップアップだけは出すようにします。

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


@blh 0x08086a14	@{J}
blh 0x08084748	@{U}
mov r7 ,r0
cmp r7, #0x0
beq FlaseReturn

@支援会話がある場合
TrueReturn:
@ldr r3, =0x08085A6E|1	@{J}
ldr r3, =0x08083736|1	@{U}
bx r3

@支援会話がない場合
FlaseReturn:
ldr r0, NotifyEvent
mov r1, #0x1
@blh 0x0800d340   @イベント命令を動作させる関数 r0=イベント命令ポインタ:POINTER_EVENT r1=引数?1-3	{J}
blh 0x0800d07c   @イベント命令を動作させる関数 r0=イベント命令ポインタ:POINTER_EVENT r1=引数?1-3	{U}

@ldr r3, =0x08085A8C|1	@{J}
ldr r3, =0x08083754|1	@{U}
bx r3

.ltorg
NotifyEvent:
