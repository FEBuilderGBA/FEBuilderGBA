.thumb
@Hook 31644 HasConvoyAccess {U}
.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4}

ldr r4,Table
sub r4,#0x8  @��ɂЂ��Ă���

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

blh	0x08083DA8     @�t���O��Ԋm�F�֐�	{U}
cmp r0,#0x01
bne Loop

Found:

TrueExit:
MOV r0, #0x1
b Exit

FalseExit:
MOV r0, #0x0
@b Exit

Exit:
pop {r4}
pop {r1}
bx r1

.ltorg
.align
Table:
