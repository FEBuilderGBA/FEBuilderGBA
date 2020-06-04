.thumb
@Call 74ba6
@r0 work
@r1 work
@r2 work
@r3 work
@r4 work
@r5 pointer:0203E184 (�U������鑤�փ��j�b�g�f�[�^�ւ�RAM�|�C���^(0203a568��) )
@r6 ���ʂ��i�[���� song id
@r7 pointer:0203E188 (�U�����鑤�փ��j�b�g�f�[�^�ւ�RAM�|�C���^(0203a4e8��) )
@
@
@struct BattleBGM
@{
@	ushort	song_id;	���y		00=term
@	byte	unit_id;	���j�b�g	00=ANY
@	byte	map_id;		��			FF=ANY
@	ushort	battle_id	�퓬�A�j��	00=ANY
@	uhsort	flag;		�t���O		00=ANY
@}//sizeof(8bytes)
@
@
ldr	r4, =0x0202bcec @ @ChapterData

ldr r6,BattleBGM_Table
sub r6,r6,#0x08
Loop:
add	r6,r6,#0x08

ldrh r0,[r6,#0x0]
cmp r0,#0x0
beq   NOTFOUND_UnitBGM  @term �I�[

@CHECKMAP
ldrb r0,[r6,#0x3]
cmp r0,#0xFF
beq CHECKUNIT

ldrb r1,[r4,#0xE] @@ChapterData->MAPID
cmp r0,r1
bne Loop



CHECKUNIT:

ldrb r0,[r6,#0x2]
cmp r0,#0x00
beq   CHECK_EQU_ITEM

ldrb	r1, [r4, #0xF]	@�t�F�C�Y 0=���R,0x40=�F�R,0x80=�G�R
cmp     r1,#0x80
beq		CHECKUNIT_CHECK_LEFT
ldr r1,[r7,#0x0] @ @Unit->ROM_UnitForm
b		CHECKUNIT_CHECK_JOIN

CHECKUNIT_CHECK_LEFT:
ldr r1,[r5,#0x0] @ @Unit->ROM_UnitForm

CHECKUNIT_CHECK_JOIN:
ldrb r1,[r1,#0x4] @ROM_UnitForm->id
cmp r0,r1
bne Loop


CHECK_EQU_ITEM:
ldrb r0,[r6,#0x4]
cmp r0,#0x00
beq CHECK_FLAG

ldrb	r1, [r4, #0xF]	@�t�F�C�Y 0=���R,0x40=�F�R,0x80=�G�R
cmp     r1,#0x80
beq		CHECK_EQU_ITEM_LEFT
mov		r1,#0x1E
ldrb	r1,[r7,r1] @RAMUnit->Item1
b		CHECK_EQU_ITEM_JOIN

CHECK_EQU_ITEM_LEFT:
mov  r1,#0x1E
ldrb r1,[r5,r1] @RAMUnit->Item1

CHECK_EQU_ITEM_JOIN:
cmp r0,r1
bne Loop


CHECK_FLAG:
ldrh	r0,[r6,#0x06] @data->flag
cmp	r0,#0x00
beq	FOUND_THE_BGM

ldr	r2, =#0x080860D0
mov	r14, r2
.short	0xF800
cmp	r0,#0x00
beq	Loop

FOUND_THE_BGM:
ldrh r6,[r6,#0x0]
b    EXIT;


@���j�b�g���Ƃ�BGM��������Ȃ������ꍇ
@�͂��Ƃɐݒ肷��퓬���y���Q�Ƃ���
NOTFOUND_UnitBGM:

ldrb	r0, [r4, #0xE] @@ChapterData->�}�b�vID

ldr	r2, =#0x08034520	@�}�b�v�ԍ�����A�}�b�v�ݒ�̃A�h���X��Ԃ��֐�	GetROMChapterStruct	RET=�}�b�v�ݒ�̃A�h���X	r0=���ׂ����}�b�vID:MAPCHAPTER
mov	r14, r2
.short	0xF800
add	r0, #0x1C	@MAP+0x1c = �����t�F�[�YBGM2

ldrb	r1, [r4, #0xF]	@�t�F�C�Y 0=���R,0x40=�F�R,0x80=�G�R
cmp     r1,#0x0
bne		CHECK_ALLY
ldrh	r6, [r0, #0x0] @���R player
b       EXIT

CHECK_ALLY:
cmp     r1,#0x40
bne		PLAY_ENEMY
ldrh	r6, [r0, #0x4] @�F�R ally
b       EXIT

PLAY_ENEMY:
ldrh	r6, [r0, #0x2] @�G�R ememy

EXIT:
@���ɖ߂�.
@r6��song_id�������Ă���.
ldr		r0,=#0x08074BBE+1
bx		r0

.ltorg
.align
BattleBGM_Table:
@POIN BattleBGM_Table
