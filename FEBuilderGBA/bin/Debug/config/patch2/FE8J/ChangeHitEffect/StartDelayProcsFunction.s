.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@遅延Procsを起動します。
@遅延Procsの目的は3つあります
@
@1.
@ヒットモーションの発生を遅延させます
@teono,teyari,bowは、命中するよりも早くHIT effect作成ルーチンを呼び出します。
@そのため、そこでVRAMに書き込んでしまうと、飛んでいる弓矢や槍のグラフィックが上書きされてしまいます。
@この事故を無くすために、一定フレーム秒が経過したら、魔法の初期化を行うようにします
@
@2.
@魔法モーションが動作している間は、親Procsがしなないように見張ります。
@具体的には、Procs->0x2C counter を 常に-1して、親procsを停滞させます。
@これにより親Procsの終了を阻害します。
@
@3.
@魔法が発動し終わったら、親Procsを迅速に終了させます。
@親Procsが残っていると、追撃したときに奇妙なことが起きるため、確実に終了させることが大切です。
@

push {r4,r5, r6, lr}
mov  r4, r0	@effectID	@assert(r0 > 10)
mov  r6, r1 @AIS

@AISからprocsが逆引きできればよいのだけど、無理なのであきらめ中

CheckEfxArrow:
ldr r0,=0x085FF328	@efxArrow	{J}
@ldr r0,=0x085D50F8	@efxArrow	{U}
blh 0x08002dec		@Find6C		{J}
@blh 0x08002e9c		@Find6C		{U}
cmp r0, #0x0
beq CheckEfxTeyari

mov r5, #0x9		@delay frame
b MakeDelayProcs

CheckEfxTeyari:
ldr r0,=0x085FF358	@efxTeyari	{J}
@ldr r0,=0x085D5128	@efxTeyari	{U}
blh 0x08002dec		@Find6C		{J}
@blh 0x08002e9c		@Find6C		{U}
cmp r0, #0x0
beq CheckEfxTeono

mov r5, #0xe		@delay frame
b MakeDelayProcs

CheckEfxTeono:
ldr r0,=0x085FF2B8	@efxTeono	{J}
@ldr r0,=0x085D5088	@efxTeono	{U}
blh 0x08002dec		@Find6C		{J}
@blh 0x08002e9c		@Find6C		{U}
cmp r0, #0x0
beq CheckBHATeono

mov r5, #0x1B		@delay frame
b MakeDelayProcs

@カスタム手斧をインストールしている場合は、以下のアドレスにProcsが記録されているので、それで検索する.
@パッチをインストールしていない場合は、09 48 03 21という値が記録されている。紛らわしいが奇数アドレスなので誤爆はしないだろう。
CheckBHATeono:
ldr   r0, =0x0805CCB0	@BHA Procs Pointer	{J}
@ldr   r0, =0x0805BF14	@BHA Procs Pointer	{U}
ldr   r0, [r0]
blh 0x08002dec		@Find6C		{J}
@blh 0x08002e9c		@Find6C		{U}
cmp r0, #0x0
beq MeleeAtack

mov r5, #0x20		@delay frame
b MakeDelayProcs


MeleeAtack:
mov r5, #0x0
mov r0, #0x3
@b MakeDelayProcs

MakeDelayProcs:
mov r1, r0
ldr r0, DelayProcs
blh 0x08002bcc		@New6C		{J}
@blh 0x08002c7c		@New6C		{U}

str  r6, [r0, #0x34]	@Copy AIS Pointer

mov  r1, #0x2b
strb r4, [r0, r1]    @EffectID

str  r5, [r0, #0x30] @delay count

mov  r1, #0x0
str  r1, [r0, #0x2c] @CurrentFrame

Exit:
pop {r4, r5, r6}
pop {r0}
bx  r0

.align
.ltorg
DelayProcs:
@DelayProcs
