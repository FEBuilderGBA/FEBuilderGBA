@Call		2AB28	{J}
@Call		2ABB8	{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ldrb	r1, [r4, #11]	@自軍は補正しない.
mov	r2, #0xC0
and	r2, r1
beq	Skip

push {r0,r5}

ldr r5, TABLE
sub r5, #0x8

Loop:
add r5, #0x8
ldr r0, [r5]
cmp r0, #0xff
beq Break         @終端

CheckMap:
ldrb r0, [r5,#0x1]
cmp r0,#0xff
beq CheckDifficulty

ldr r3,=0x0202BCEC	@ChapterData	{J}
@ldr r3,=0x0202BCF0	@ChapterData	{U}
ldrb r1,[r3,#0xe]  @Current->MapID
cmp  r1,r0
bne  Loop

CheckDifficulty:
ldrb r0, [r5,#0x2]
cmp r0,#0x0
beq CheckPromoted

ldr r3,=0x0202BCEC	@ChapterData	{J}
@ldr r3,=0x0202BCF0	@ChapterData	{U}
mov  r1,#0x42
ldrb r1,[r3,r1]  @難易度等
mov r2, #0x20       @難易度 簡単(eazy)
and r1,r2
cmp r1,#0x20
bne Loop            @難易度 簡単ならばボツ

ldr r3,=0x0202BCEC	@ChapterData	{J}
@ldr r3,=0x0202BCF0	@ChapterData	{U}
ldrb r1,[r3,#0x14]  @難易度等
mov r2, #0x40       @難易度 Hard
and r1,r2

cmp r0, #0x1
beq CheckDifficulty_Normal
cmp r0, #0x2     @CheckDifficulty_Normal_more
beq CheckPromoted
cmp r0, #0x3
beq CheckDifficulty_Hard

CheckDifficulty_Normal:
cmp r1,#0x40
beq Loop            @難易度Hardなのでボツ
b   CheckPromoted

CheckDifficulty_Hard:
cmp r1,#0x40
bne Loop            @難易度Hardではないのでボツ
@b CheckPromoted

CheckPromoted:
ldrb r0, [r5,#0x3]
cmp r0,#0x0
beq CheckPhase

ldr	r2, [r4, #0x0]
ldr	r1, [r4, #0x4]
ldr	r2, [r2, #0x28]
ldr	r1, [r1, #0x28]
orr	r2, r1
mov	r1, #0x80
lsl	r1, r1, #0x1
and	r2, r1

cmp r0, #0x1
beq CheckPromoted_Promoted

CheckPromoted_UnPromoted:
cmp	r2, #0x0
beq Loop     @下級職でなければボツ
b   CheckPhase

CheckPromoted_Promoted:
cmp	r2, #0x0
bne Loop     @上級職でなければボツ

CheckPhase:
ldrb r0, [r5,#0x4]
cmp r0,#0x0
beq CheckFlag

ldr r3,=0x0202BCEC	@ChapterData	{J}
@ldr r3,=0x0202BCF0	@ChapterData	{U}
ldrb r1,[r3,#0xF]  @フェイズ 0=自軍,0x40=友軍,0x80=敵軍

cmp  r0,#0x2
beq  CheckPhase_Enemy

CheckPhase_Player:
cmp  r1,#0x0
bne  Loop    @プレイヤーターンではないのでボツ
b    CheckFlag

CheckPhase_Enemy:
cmp  r1,#0x80
bne  Loop    @敵ターンではないのでボツ
@b    CheckFlag


CheckFlag:
ldrh r0, [r5,#0x6]
cmp r0,#0x0
beq Found

blh 0x080860d0   @CheckFlag {J}
@blh 0x08083da8   @CheckFlag {U}
cmp r0,#0x0
beq Loop


Found:
ldrb r1, [r5,#0x0]
b  Exit

Break:
mov r1,#0x0

Exit:

pop {r0,r5}
add r0,r1

Skip:
@壊すコードを再送
mov r2, #0x15
ldsb r2, [r4, r2]
lsl r2 ,r2 ,#0x1
add r2 ,r2, r0

ldr r3,=0x0802AB30|1	@{J}
@ldr r3,=0x0802ABC0|1	@{U}
bx  r3

.ltorg
TABLE:
