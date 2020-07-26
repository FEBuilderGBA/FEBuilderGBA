@1C710	{J}
@1CAA8	{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@r4 => Unit

push {r5}

@壊すコードを再送1
ldrb r0, [r0, #0x4]
blh 0x080a9190       @BWL_IncrementMoveValue	{J}
@blh 0x080a474c       @BWL_IncrementMoveValue	{U}


@音を再生する設定ですか?
ldr r0, =0x0202BCEC  @ChapterData	{J}
@ldr r0, =0x0202BCF0  @ChapterData	{U}
add r0, #0x41
ldrb r0, [r0]
                   @0x02 効果音 OFF
                   @0x80 字幕ヘルプ OFF
mov r1, #0x82
and r0, r1
cmp r0, #0x0
bne Term           @音を再生できないなら終了



ldr r5,Table
sub r5,#0x18

Loop:
add r5,#0x18
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
beq  CheckVoice1

blh #0x080860D0	@CheckFlag {J}
@blh #0x08083DA8	@CheckFlag {U}
cmp r0, #0x0
bne Loop

CheckVoice1:
ldrh r0,[r5,#0x4]
cmp  r0,#0x0
beq  Term

@音が2つ以上ある場合は、
@選択する度に、異なる音声を再生します。
@
ldr  r3, =0x203AFF0		@Counter	{J}
@ldr  r3, =0x203B1F0		@Counter	{U}
ldrb r0, [r3]
mov  r1,#0x01
add  r1,r0
strb r1, [r3]   @counter++

lsl  r0, r0, #0x1
mov  r1,#0x4
add  r0,r1

cmp  r0, #0x16 @22
bgt  PlayVoice1

CheckVoiceData:
ldrh r0,[r5,r0]
cmp  r0,#0x0
beq  PlayVoice1
b    Play

PlayVoice1:
mov  r1, #0x01   @counter = 1
strb r1, [r3]    @カウンターを初期値にリセットする

ldrh r0,[r5,#0x4]

Play:
ldr r3,=0x7fff   @7FFF以上は無音
cmp r0, r3
bge Term

blh 0x080d4ef4    @m4aSongNumStart	{J}
@blh 0x080D01FC    @m4aSongNumStart	{U}

Term:

mov r0 ,r6       @壊すコードを再送

pop {r5}

ldr r3, =0x0801C718+1	@{J}
@ldr r3, =0x0801CAB0+1	@{U}
bx r3

.ltorg
.align
Table:

