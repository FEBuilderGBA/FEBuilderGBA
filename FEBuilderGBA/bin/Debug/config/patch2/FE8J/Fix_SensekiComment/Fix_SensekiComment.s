@
@Fix_SensekiComment
@
@章が攻略評価に値するかどうかを判定しますが、
@攻略ターンに書き込まれている章はすべて評価するべきです。
@常に、対象の章だと回答した方が安全です。
@
@By 7743
@
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}

mov  r1 ,r0       @Parent Procs
ldr  r0, =0x080A2C0C
ldr  r0, [r0]
blh  0x08002c30   @NewBlocking6C

mov  r0,#0x00
pop  {r1}
bx   r1
