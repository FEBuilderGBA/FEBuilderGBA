@81704	{J}
@7F3C4	{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@r4 => Unit

push {r4,r5}

@音を再生する設定ですか?
@ldr r0, =0x0202BCEC  @ChapterData	{J}
ldr r0, =0x0202BCF0  @ChapterData	{U}
add r0, #0x41
ldrb r0, [r0]
                   @0x02 効果音 OFF
                   @0x80 字幕ヘルプ OFF
mov r1, #0x82
and r0, r1
cmp r0, #0x0
bne Term           @音を再生できないなら終了

@ldr r4,=0x0203a4e8	@戦闘アニメの右側	{J}
ldr r4,=0x0203A4EC	@戦闘アニメの右側	{U}
mov r1,#0x0b
ldrb r0,[r4,r1]     @ID
cmp r0,#0x40        @自軍ユニットの方が欲しい
ble Inittable

@ldr r4,=0x0203a568	@戦闘アニメの左側	{J}
ldr r4,=0x0203A56C	@戦闘アニメの左側	{U}

Inittable:
ldr r5,Table
sub r5,#0x10

Loop:
add r5,#0x10
ldr r0, [r5]
cmp r0, #0x0
beq Term

CheckUnit:
ldrb r0, [r5,#0x0]
cmp  r0, #0x0
beq  CheckClass

ldr  r1, [r4]
ldrb r1, [r1, #0x04]
cmp  r0, r1
bne  Loop

CheckClass:
ldrb r0, [r5,#0x1]
cmp  r0, #0x0
beq  CheckFlag

ldr  r1, [r4, #0x4]
ldrb r1, [r1, #0x04]
cmp  r0, r1
bne  Loop

CheckFlag:
ldrh r0, [r5,#0x2]
cmp  r0, #0x0
beq  CheckVoice

@blh 0x080860D0	@CheckFlag {J}
blh 0x08083DA8	@CheckFlag {U}
cmp r0, #0x0
bne Loop

CheckVoice:

bl   CountGlowParam
@mov r0,r0          @levelアップした項目数

cmp r0,#02
ble BadGrow
cmp r0,#05
bge GoodGrow

NormalGrow:
ldrh r0,[r5,#0x4]
cmp  r0,#0x0
beq  Term
mov  r1,#0x4
b    PlayRand

BadGrow:
ldrh r0,[r5,#0x8]
cmp  r0,#0x0
beq  NormalGrow

mov  r1,#0x8
b    PlayRand

GoodGrow:
ldrh r0,[r5,#0xC]
cmp  r0,#0x0
beq  NormalGrow

mov  r1,#0xC
b    PlayRand


PlayRand:
ldrh r0,[r5,r1]
cmp  r0,#0x0
beq  Term

CheckVoice2:
add  r2,r1,#0x2

ldrh r0,[r5,r2]
cmp  r0,#0x0
beq  PlayVoice1

@ldr  r3, =0x203AFF0		@Counter	{J}
ldr  r3, =0x203B1F0		@Counter	{U}
ldrb r0, [r3]
add  r0,#0x01
strb r0, [r3]   @counter++

mov  r3,#0x1
and  r0,r3
cmp  r0,#0x0
beq  PlayVoice1

PlayVoice2:
add  r1,r1,#0x2
b    Play

PlayVoice1:
@mov  r1,r1
b    Play

Play:
ldrh r0,[r5,r1]
mov  r4,r0

@ldr r0, =0x087A8F7C @Procs  efxSoundSE {J}
ldr r0, =0x08758A48 @Procs  efxSoundSE {U}
mov r1, #0x3
@blh 0x08002c30   @NewBlocking6C	{J}
blh 0x08002ce0   @NewBlocking6C	{U}
                  @New6Cではダメ。NewBlocking6Cでないと、まれに無視される

mov r1, #0x80
lsl r1 ,r1 ,#0x1
str r1, [r0, #0x44]
str r4, [r0, #0x48]
mov r1,#0x0
strh r1, [r0, #0x2c]

Term:
pop {r4,r5}

mov r0 ,r7
@blh 0x08002de4   @Break6CLoop	{J}
blh 0x08002E94   @Break6CLoop	{U}

@ldr r3, =0x80817a8+1	@{J}
ldr r3, =0x807f468+1	@{U}
bx r3

@成長したパラメータの数を取得するため↑マークの数を数える
CountGlowParam:
push {r4,lr}
@ldr r3, =0x85B9424	@APProc	{J}
ldr r3, =0x859168C	@APProc	{U}
mov r4, #0x0
@ldr r1, =0x02024E68 @gProcStatePool	{J}
ldr r1, =0x02024E68 @gProcStatePool	{U}
mov r2, #0x0
CountGlowParam_Loop:
ldr r0, [r1, #0x0]
cmp r0 ,r3
bne CountGlowParam_Next
	add r4,#0x01
CountGlowParam_Next:
add r2, #0x1
add r1, #0x6c
cmp r2, #0x3f
ble CountGlowParam_Loop
mov r0, r4
lsr r0, #0x1  @/2
pop {r4}
pop {r1}
bx r1

.ltorg
Table:
