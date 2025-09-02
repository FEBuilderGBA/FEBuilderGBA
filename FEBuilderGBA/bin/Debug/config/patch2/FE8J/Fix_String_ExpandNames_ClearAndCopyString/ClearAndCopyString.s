.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


ClearAndCopyString:

@Hook 0800A014	@{J}
push {r4,r5}
mov r4, r0 @keep
mov r5, r1 @keep

@まず作業メモリを綺麗にします
mov r0 ,r4
mov r1, #0x0
ldr r2, =0x100    @とりあえず0x100バイトぐらい初期化する 255byte
blh 0x080d6968   @memset	@{J}

@その後に文字をコピーします
@これにより端数の問題は起きなくなります
mov r0 ,r4
mov r1 ,r5
blh 0x08012f78   @CopyString	@{J}
pop {r4,r5}

GoBack:
ldr r3, =0x800a0fc|1	@{J}
bx  r3
