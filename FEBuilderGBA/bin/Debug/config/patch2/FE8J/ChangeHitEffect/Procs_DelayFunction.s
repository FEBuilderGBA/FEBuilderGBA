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

push {r4,r7,lr}
mov  r4, r0

ldr  r0, [r4,#0x2c] @frameCounter++
add  r0, #0x01
str  r0, [r4,#0x2c]

ldr  r1, [r4,#0x30] @delayCount
cmp  r0, r1
blt  Exit       @delay待機中なら終了

@delay Countを超えたので魔法を実行する

@カメラの位置を強引に近接に調整する.
@これは行儀が悪い方法ではあるが、カメラを近場に強制します
ldr  r3, =0x0203E11C	@gSomethingRelatedToAnimAndDistance	@{J}
@ldr  r3, =0x0203E120	@gSomethingRelatedToAnimAndDistance	@{U}
mov  r2, #0x0    @カメラを強制的に近接に切り替えます
strb r2, [r3]

@gBattleSpellAnimationId1,2を共に書き換えます.
@range animationで逆が参照されることがあるためです。
mov  r0, #0x2b
ldrb r0, [r4,r0] @effectID
ldr  r3, =0x0203E114 @gBattleSpellAnimationId1	{J}
@ldr  r3, =0x0203E118 @gBattleSpellAnimationId1	{U}

ldr  r1, [r3]  @あとで書き戻せるように、一度保存しておきます
push {r1}

@CSAがたまに逆のAnimationIDを参照することがあるみたいなので、面倒なので両方とも同じ値で埋めます
@StartSpellAnimationが終わったら、元にもどします。
strh r0, [r3]       @gBattleSpellAnimationId1
strh r0, [r3,#0x2]  @gBattleSpellAnimationId2

ldr  r0, [r4, #0x34] @AIS
mov  r7, r0          @FEditorCSAはr7にAISが代入されていることを期待している
blh  0x0805C170             @StartSpellAnimation	{J}
@blh  0x0805B3CC             @StartSpellAnimation	{U}

pop {r1}
ldr r3, =0x0203E114 @gBattleSpellAnimationId1	{J}
@ldr r3, =0x0203E118 @gBattleSpellAnimationId1	{U}
str  r1, [r3]  @gBattleSpellAnimationId1,2 の書き戻し
@b    Break

Break:
mov  r0, r4
blh  0x08002de4   @Break6CLoop	@{J}
@blh  0x08002e94   @Break6CLoop	@{U}

Exit:
pop {r4,r7}
pop {r0}
bx r0
