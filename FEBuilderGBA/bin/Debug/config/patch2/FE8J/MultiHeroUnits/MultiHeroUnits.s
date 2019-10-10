@Hook 3316C {J}
.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ldr r0, =0x03004DF0	@CurrentUnit {J}
ldr r5,[r0]

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

ldr r1,=0x0202BCEC	@ChapterData	{J}
ldrb r1,[r1,#0xf]  @Current->MapID
cmp r0,r1
bne Loop

CheckEdition:
ldrb r0,[r4,#0x03]
cmp  r0,#0xff
beq CheckFlag

ldr r1,=0x0202BCEC	@ChapterData	{J}
ldrb r1,[r1,#0x1b]  @Current->Editon
cmp r0,r1
bne Loop

CheckFlag:
ldrh r0,[r4,#0x04]
cmp r0,#0x0
beq Found

blh	0x080860d0     @フラグ状態確認関数	{J}
cmp r0,#0x01
bne Loop

Found:

TrueExit:
ldrb r0,[r4,#0x00] @Table->Unit
ldr r3,=0x080331d4+1
b Exit

FalseExit:
@主人公がいないので、フリーマップと同様に、Unitの並び順とする
ldr r3,=0x080331B0+1

Exit:
bx r3

.ltorg
.align
Table:
