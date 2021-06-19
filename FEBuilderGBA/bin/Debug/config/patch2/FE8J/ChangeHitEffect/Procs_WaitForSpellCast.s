.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@work space
@2b effect ID
@2c frameCounter
@30 delayCount
@34 AISCore

push {r4,lr}
mov  r4, r0

CheckParentProcs:
ldr r3, [r4, #0x14]  @parent Procs
cmp r3, #0x7         @親がroot nodeなら何もできない
ble Break            @即終了

CheckEfxSpellCast:
ldr  r0, =0x02017778	@gpEfxSpellCast	{J}
@ldr  r0, =0x02017778	@gpEfxSpellCast	{U} @同じアドレスです

ldr r0, [r0]         @魔法が終了していたら、gpEfxSpellCastはnullになる
cmp r0, #0x0
beq Break

FreezeParentProcs:
@親のframeCounterに干渉して動きを止めます
ldrh r0, [r3, #0x2c]
cmp  r0, #0x0
beq  Exit            @0から引けないので念のため

sub  r0,#0x01
strh r0, [r3, #0x2c] @強制足踏み
b    Exit

Break:

ldr r3, [r4, #0x14]  @parent Procs
cmp r3, #0x7         @親がroot nodeなら何もできない
ble Break2

@親Procsを速やかに停止させる
blh 0x08056114   @SetSomethingSpellFxToFalse	{J}
@blh 0x0805516c   @SetSomethingSpellFxToFalse	{U}
ldr r0, [r4, #0x14]
blh 0x08002de4   @Break6CLoop	{J}
@blh  0x08002e94   @Break6CLoop	@{U}

Break2:
mov r0 ,r4
blh  0x08002de4   @Break6CLoop	@{J}
@blh  0x08002e94   @Break6CLoop	@{U}

Exit:
pop {r4}
pop {r0}
bx  r0
