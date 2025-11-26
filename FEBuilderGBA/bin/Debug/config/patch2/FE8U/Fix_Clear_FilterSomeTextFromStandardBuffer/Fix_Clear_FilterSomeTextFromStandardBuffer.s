.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


ClearAndCopyString:

@Hook 0800A3C8	@{U}

push {r0,r1}     @壊すので保護する
@mov r0 ,r5
mov r1, #0x0
ldr r2, =0x100    @とりあえず0x100バイトぐらい初期化する 255byte
blh 0x080d1c6c   @memset	@{U}

@その後に文字をコピーします
@これにより端数の問題は起きなくなります
pop {r0,r1}      @元に戻す

                 @本来のコピー処理の実施
blh 0x08012ec0   @CopyString	@{U}

GoBack:
ldr r3, =0x800a416|1	@{U}
bx  r3
