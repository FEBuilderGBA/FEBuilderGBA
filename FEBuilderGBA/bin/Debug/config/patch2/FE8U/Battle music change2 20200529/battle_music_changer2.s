.thumb
@Call 726CA
@r0 work
@r1 work
@r2 work
@r3 work
@r4 work
@r5 pointer:0203E188 (攻撃される側へユニットデータへのRAMポインタ(0203A56Cへ) )
@r6 結果を格納する song id
@r7 pointer:0203E18C (攻撃する側へユニットデータへのRAMポインタ(0203A4ECへ) )
@
@
@struct BattleBGM
@{
@	ushort	song_id;	音楽		00=term
@	byte	unit_id;	ユニット	00=ANY
@	byte	map_id;		章			FF=ANY
@	ushort	battle_id	戦闘アニメ	00=ANY
@	uhsort	flag;		フラグ		00=ANY
@}//sizeof(8bytes)
@
@
ldr	r4, =#0x202BCF0 @Chaptor Pointer  (@ChapterData)

ldr r6,BattleBGM_Table
sub r6,r6,#0x08
Loop:
add	r6,r6,#0x08

ldrh r0,[r6,#0x0]
cmp r0,#0x0
beq   NOTFOUND_UnitBGM  @term 終端

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
beq   CHECK_BATTLEANIME

ldrb	r1, [r4, #0xF]	@フェイズ 0=自軍,0x40=友軍,0x80=敵軍
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

ldrb	r1, [r4, #0xF]	@フェイズ 0=自軍,0x40=友軍,0x80=敵軍
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

ldr	r2, =#0x08083DA8
mov	r14, r2
.short	0xF800
cmp	r0,#0x00
beq	Loop

FOUND_THE_BGM:
ldrh r6,[r6,#0x0]
b    EXIT;


@ユニットごとのBGMが見つからなかった場合
@章ごとに設定する戦闘音楽を参照する
NOTFOUND_UnitBGM:

ldrb	r0, [r4, #0xE] @@ChapterData->マップID

ldr	r2, =#0x08034618	@マップ番号から、マップ設定のアドレスを返す関数	GetROMChapterStruct	RET=マップ設定のアドレス	r0=調べたいマップID:MAPCHAPTER
mov	r14, r2
.short	0xF800
add	r0, #0x1C	@MAP+0x1c = 味方フェーズBGM2

ldrb	r1, [r4, #0xF]	@フェイズ 0=自軍,0x40=友軍,0x80=敵軍
cmp     r1,#0x0
bne		CHECK_ALLY
ldrh	r6, [r0, #0x0] @自軍 player
b       EXIT

CHECK_ALLY:
cmp     r1,#0x40
bne		PLAY_ENEMY
ldrh	r6, [r0, #0x4] @友軍 ally
b       EXIT

PLAY_ENEMY:
ldrh	r6, [r0, #0x2] @敵軍 ememy

EXIT:
@元に戻す.
@r6にsong_idが入っている.
ldr		r0,=#0x080726E2+1
bx		r0

.ltorg
.align
BattleBGM_Table:
@POIN BattleBGM_Table
