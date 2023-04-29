@Call 0x0808A820 FE8J
@r0 
@r1 
@r2 
@r3 
@r4 keep
@r5 Table�̈�Ɨ��p����.

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


.thumb
.org 0x00

push {r5}           @�����I�ɂ͕s�v�����A�ꉞ�A�ۑ����Ă�����.
                    @r4�͕K�v�ȃf�[�^�������Ă���̂ŁA���������ύX���Ȃ�.



ldr  r5,Table
sub  r5,#0x10        @     ���[�v�������ʓ|�Ȃ̂ŁA�ŏ���0x10(16)�o�C�g���������܂�.

Loop:
add  r5,#0x10       @     ���̃f�[�^��
ldr  r0,[r5,#0x04]  @     P4:ZIMAGE=Image
cmp  r0,#0x00       @     �f�[�^�̃|�C���^���Ȃ��ꍇ�A�I�[�Ƃ݂Ȃ�.
beq  load_default_bg     @�f�[�^���Ȃ��̂ŁA�f�B�t�H���g�̔w�i�����[�h���ďI��!

CheckMapID:
ldrb r0,[r5,#0x00]  @     B0:MAPID=MAPID(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckAllegiance

ldr  r2,=#0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     �����s��v�Ȃ̂ŁA���̃��[�v�� continue;

CheckAllegiance:
ldrb r3,[r5,#0x01]  @     B1=Allegiance(0xFF=ANY,0x00=Player,0x40=NPC,0x80=Enemy)
cmp  r3,#0xFF       @     ANY Allegiance ?
beq  CheckFlag

bl GetUnitAllegiance @�����̎擾 r0,r1,r2 ��j�� ���ʂ� r0�ɖ߂�
cmp  r3,r0                @r0 == 0x00  Player
                          @r0 == 0x40  NPC
                          @r0 == 0x80  Enemy
bne  Loop           @     �����s��v�Ȃ̂ŁA���̃��[�v�� continue;



CheckFlag:
ldrh r0,[r5,#0x02]  @     W2:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

blh  0x080860D0     @FE8J CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq  Loop           @     �����s��v�Ȃ̂ŁA���̃��[�v�� continue;

b    Found          @����!


GetUnitAllegiance:        @���W���烆�j�b�g�̏������擾����
                          @Kirb�̃��[�`�������ɂ���
push {lr}
ldr  r2,=0x0202bcc0       @FE8J gCursorMapPosition
ldrh r0,[r2]              @     gCursorMapPosition->xcoord
ldrh r1,[r2,#2]           @     gCursorMapPosition->ycoord

ldr  r2,=0x0202E4D4       @FE8J gMapUnit
ldr  r2,[r2]
lsl  r1,#2                @     y*4
add  r1,r2                @     row address
ldr  r1,[r1]
ldrb r0,[r1,r0]

cmp r0,#0
bne CheckAlleg
                          
ldr r0,=0x0202BE40        @FE8J gActiveUnitIndex
ldrb r0,[r0]
CheckAlleg:
mov r1,#0xc0
and r0,r1

pop {pc}                  @�T�u���[�`���̏I���
                          @r0 == 0x00  Player
                          @r0 == 0x40  NPC
                          @r0 == 0x80  Enemy


Found:              @�T�������f�[�^�Ƀ}�b�`�����B
                    @���[�U���w�肵���w�i�����[�h����
                    @r5 Table(current)
ldr r0,[r5,#0x04]   @     �w�i�摜
ldr r1,=0xFFFFFFFF  @     FEBuilderGBA�̓s�� �f�[�^��0���ł͍���̂Ń_�~�[�f�[�^�������
cmp r0,r1
beq load_default_bg

ldr r1,=0x0600B000  @FE8J �w�i�����[�h����VRAM
blh 0x08013008      @FE8J UnLZ77Decompress

ldr r0,[r5,#0x0C]   @     �w�i�p���b�g
mov r1, #0xc0
lsl r1 ,r1 ,#0x1
mov r2, #0x80
blh 0x08000d68      @FE8J CopyToPaletteBuffer 

ldr r0,[r5,#0x08]   @     �w�iTSA
mov r1 ,r4          @     ���[�h����̈��r4�ɏ�����Ă���Br4�͉󂳂Ȃ��悤��.
blh 0x08013008      @FE8J UnLZ77Decompress
b Exit

@�ݒ肪�Ȃ��ꍇ�́A�f�B�t�H���g�̔w�i�����[�h����.
load_default_bg:    @�f�B�t�H���g�̐ݒ�����[�h����
ldr r0,=0x0808A8C4  @FE8J �f�B�t�H���g�̃X�e�[�^�X��ʂ̔w�i�������Ă���A�h���X
ldr r0,[r0]         @     �|�C���^�Q�Ƃ��邱�ƂŁA���|�C���g�ɑς���.
ldr r1,=0x0600B000  @FE8J �w�i�����[�h����VRAM
blh 0x08013008      @FE8J UnLZ77Decompress

ldr r0,=0x0808A8CC  @FE8J �f�B�t�H���g�̃X�e�[�^�X��ʂ̃p���b�g�������Ă���A�h���X
ldr r0,[r0]         @     �|�C���^�Q�Ƃ��邱�ƂŁA���|�C���g�ɑς���.
mov r1, #0xc0
lsl r1 ,r1 ,#0x1
mov r2, #0x80
blh 0x08000d68      @FE8J CopyToPaletteBuffer 

ldr r0,=0x0808A8D0  @FE8J TSA
ldr r0,[r0]         @     �|�C���^�Q�Ƃ��邱�ƂŁA���|�C���g�ɑς���.
mov r1 ,r4          @     ���[�h����̈��r4�ɏ�����Ă���Br4�͉󂳂Ȃ��悤��.
blh 0x08013008      @FE8J UnLZ77Decompress

Exit:
pop {r5}
ldr r0,=0x0808A83C+1 @FE8J �߂�A�h���X
bx  r0

.ltorg
.align
Table:
