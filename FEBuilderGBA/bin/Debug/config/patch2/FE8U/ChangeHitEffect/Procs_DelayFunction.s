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
blt  Exit       @delay�ҋ@���Ȃ�I��

@delay Count�𒴂����̂Ŗ��@�����s����

@�J�����̈ʒu�������ɋߐڂɒ�������.
@����͍s�V���������@�ł͂��邪�A�J�������ߏ�ɋ������܂�
@ldr  r3, =0x0203E11C	@gSomethingRelatedToAnimAndDistance	@{J}
ldr  r3, =0x0203E120	@gSomethingRelatedToAnimAndDistance	@{U}
mov  r2, #0x0    @�J�����������I�ɋߐڂɐ؂�ւ��܂�
strb r2, [r3]

@gBattleSpellAnimationId1,2�����ɏ��������܂�.
@range animation�ŋt���Q�Ƃ���邱�Ƃ����邽�߂ł��B
mov  r0, #0x2b
ldrb r0, [r4,r0] @effectID
@ldr  r3, =0x0203E114 @gBattleSpellAnimationId1	{J}
ldr  r3, =0x0203E118 @gBattleSpellAnimationId1	{U}

ldr  r1, [r3]  @���Ƃŏ����߂���悤�ɁA��x�ۑ����Ă����܂�
push {r1}

@CSA�����܂ɋt��AnimationID���Q�Ƃ��邱�Ƃ�����݂����Ȃ̂ŁA�ʓ|�Ȃ̂ŗ����Ƃ������l�Ŗ��߂܂�
@StartSpellAnimation���I�������A���ɂ��ǂ��܂��B
strh r0, [r3]       @gBattleSpellAnimationId1
strh r0, [r3,#0x2]  @gBattleSpellAnimationId2

ldr  r0, [r4, #0x34] @AIS
mov  r7, r0          @FEditorCSA��r7��AIS���������Ă��邱�Ƃ����҂��Ă���
@blh  0x0805C170             @StartSpellAnimation	{J}
blh  0x0805B3CC             @StartSpellAnimation	{U}

pop {r1}
@ldr r3, =0x0203E114 @gBattleSpellAnimationId1	{J}
ldr r3, =0x0203E118 @gBattleSpellAnimationId1	{U}
str  r1, [r3]  @gBattleSpellAnimationId1,2 �̏����߂�
@b    Break

Break:
mov  r0, r4
@blh  0x08002de4   @Break6CLoop	@{J}
blh  0x08002e94   @Break6CLoop	@{U}

Exit:
pop {r4,r7}
pop {r0}
bx r0
