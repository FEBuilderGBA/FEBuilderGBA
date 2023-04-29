@Call 0x08018FCC FE8J
@r0 ram unit
@r1 
@r2 
@r3 
@r4 use stack. RAM UNIT
@r5 use stack. Table

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
.org 0x00

push {lr}           @     �t�b�N����]���̂��߂� push�̗̈���󂷂̂ōđ�����.
push {r4,r5}
mov  r4,r0          @     RAM UNIT
ldr  r5,Table
sub  r5,#0x8        @     ���[�v�������ʓ|�Ȃ̂ŁA�ŏ���0x8�o�C�g���������܂�.

Loop:
add  r5,#0x8        @     ���̃f�[�^��

ldrh r0,[r5,#0x00]  @     W0:Portrait
cmp  r0,#0x0        @     �f�[�^�̃|�C���^���Ȃ��ꍇ�A�I�[�Ƃ݂Ȃ�.
beq  load_default   @�f�[�^���Ȃ��̂ŁA�f�B�t�H���g�̊�摜�����[�h���ďI��!

ldrb r0,[r5,#0x02]  @     B2:UNITID
cmp  r0,#0x00       @     �f�[�^�̃|�C���^���Ȃ��ꍇ�A�I�[�Ƃ݂Ȃ�.
beq  load_default   @�f�[�^���Ȃ��̂ŁA�f�B�t�H���g�̊�摜�����[�h���ďI��!

ldr  r1,[r4,#0x00]  @     RAMUNIT->ROMUNIT
ldrb r1,[r1,#0x4]   @     ROMUNIT->ID
cmp  r0,r1
bne  Loop

CheckClass:
ldrb r0,[r5,#0x03]  @     B3:CLASSID
cmp  r0,#0x00       @     ANY?
beq  CheckMAP
cmp  r0,#0xFF       @     ANY?
beq  CheckMAP

ldr  r1,[r4,#0x04]  @     RAMUNIT->ROMCLASS
ldrb r1,[r1,#0x4]   @     CLASSUNIT->ID
cmp  r0,r1
bne  Loop

CheckMAP:
ldrb r0,[r5,#0x04]  @     B4:MAPID=MAPID(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckEdition

ldr  r2,=#0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     �����s��v�Ȃ̂ŁA���̃��[�v�� continue;

CheckEdition:
ldrb r0,[r5,#0x05]  @     B5:EDITION=��(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckFlag

ldr  r2,=#0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0x1B]  @     ChapterData->Edition
cmp  r0,r1
bne  Loop           @     �����s��v�Ȃ̂ŁA���̃��[�v�� continue;

CheckFlag:
ldrh r0,[r5,#0x06]  @     W6:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

blh  0x080860D0     @FE8J CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq  Loop           @     �����s��v�Ȃ̂ŁA���̃��[�v�� continue;

@@b    Found          @����!

Found:
ldr  r1,[r4,#0x00]  @     RAMUNIT->ROMUNIT  @�߂�l�ɕK�{�Ȃ̂ł����ő��.
ldrh r0,[r5,#0x00]  @     W0:Portrait���̗p
b    Exit

load_default:
ldr  r1,[r4,#0x00]  @     RAMUNIT->ROMUNIT
ldrh r0,[r1,#0x6]   @     ROMUNIT->ID

Exit:
mov  r2 ,r4         @     �󂷃R�[�h�̍đ�(push lr; �͐擪�Ŏ��s�ς݂ł�)
pop {r5,r4}         @     ���̊֐����ŗ��p�����X�^�b�N�̉��(lr�͉��������_����)

@r2 = ram unit
@r1 = rom unit
@r0 = portrait id
ldr r3,=0x08018FD4+1 @FE8J �߂�A�h���X
bx  r3

.ltorg
.align
Table:
