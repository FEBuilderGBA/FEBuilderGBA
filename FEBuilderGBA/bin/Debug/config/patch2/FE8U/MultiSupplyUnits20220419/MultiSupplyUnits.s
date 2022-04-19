@Hook 23F70 {U}
.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4, r5}
ldr r0, =0x03004E50	@CurrentUnit {U}
ldr r5,[r0]


@召喚ユニットのチェック
CheckSummonUnit:
ldr r3, =0x08023F78	@{U}
ldrb r0, [r3, #0x1]
cmp  r0,#0x28
bne  Check7743Summon
ldrb r0, [r3, #0x0] @Summon Class ディフォルトは0x51 Phantom

ldr r1,[r5,#0x04]
ldrb r1,[r1,#0x04]   @Unit->Class->ID
cmp  r0, r1
beq  FalseExit       @Summonなので無効
b    LoopStart

Check7743Summon:
ldrb r0,[r5,#0x0F]
LSR  r0 ,r0 ,#0x7
CMP  r0, #0x1
BEQ  FalseExit       @Summonなので無効
@b    LoopStart


LoopStart:
ldr r4,Table
sub r4,#0x8  @先にひいておく

Loop:
add r4,#0x8
ldr r0,[r4]
cmp r0,#0x0
beq FalseExit

CheckMap:
ldrb r0,[r4,#0x02]
cmp  r0,#0xff
beq CheckEdition

ldr r1,=0x0202BCF0	@ChapterData	{U}
ldrb r1,[r1,#0xe]  @Current->MapID
cmp r0,r1
bne Loop

CheckEdition:
ldrb r0,[r4,#0x03]
cmp  r0,#0xff
beq CheckFlag

ldr r1,=0x0202BCF0	@ChapterData	{U}
ldrb r1,[r1,#0x1b]  @Current->Editon
cmp r0,r1
bne Loop

CheckFlag:
ldrh r0,[r4,#0x04]
cmp r0,#0x0
beq Found

blh	0x08083DA8     @フラグ状態確認関数	{U}
cmp r0,#0x01
bne Loop

Found:

CheckUnit:
ldrb r0,[r4,#0x00]
cmp r0,#0xff   @誰でも呼べる
beq TrueExit

ldr r1,[r5,#0x00]
ldrb r1,[r1,#0x04]   @Unit->Unit->ID
cmp r0,r1
beq TrueExit

blh 0x08023EF0	@連接チェック	{U}
cmp r0,#0x00
beq Loop

TrueExit:
MOV r0, #0x1
b Exit

FalseExit:
MOV r0, #0x3
@b Exit

Exit:
pop {r4,r5}
pop {r1}
bx r1


.ltorg
.align
Table:
