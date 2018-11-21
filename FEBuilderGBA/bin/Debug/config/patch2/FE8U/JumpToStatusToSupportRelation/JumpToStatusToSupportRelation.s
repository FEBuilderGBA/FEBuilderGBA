.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Call 8E60C FE8U
@r0 
@r1
@r2
@r3 keep
@r4 copy r5
@r5 parent Procs
@r6 
@r7 store [r3, #0x0]

ldr  r1, =0x0858791C @KeyStatusBuffer Pointer
ldr  r1, [r1, #0x0] @KeyStatusBuffer
ldrh r1, [r1, #0x8] @KeyStatusBuffer@KeyStatusBuffer.NewPresses:  1 For Press, 0 Otherwise 

mov r0, #0x80       @Lボタン
lsl r0 ,r0 ,#0x2

and r0 ,r1
cmp r0, #0x0
beq Exit

@Lボタンが押されている

@背景を消す.
mov r0, r5
blh 0x080ADDD4   @                  これは絶対に必要らしい。詳細不明
blh 0x08095a38   @EndBG3Slider_
mov r0, #0x0
blh 0x08005758   @DeleteFaceByIndex これはいらないかも

@支援関係を呼び出し
mov r1 ,r5
ldr r0, =0x080A1980
ldr r0, [r0] @(Procs SupportScreenMain )
blh 0x08002ce0   @NewBlocking6C
add r0, #0x42
mov r1, #0x2     @BGM Flag2
strb r1, [r0, #0x0]

@BlockingしたProcsが終了したら、親Procsも終わらせたいので、
@親Procsの終了させる6CLabelを選択する。これでいいらしい。
mov r0 ,r5
mov r1, #0x1
blh 0x08002f24   //Goto6CLabel

Exit:

@壊すコードの再送
mov r3 ,r5
add r3, #0x2e
ldrb r7, [r3, #0x0]
mov r4 ,r5
add r4, #0x3e

ldr r1,=0x0808E616+1
bx  r1
