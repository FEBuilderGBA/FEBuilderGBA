.thumb
@Call 68656
@r0 work
@r1 work
@r2 work
@r3 work
@r4 work
@r5 pointer:0203E06C (攻撃される側へユニットデータへのRAMポインタ(0203A46Cへ) )
@r6 pointer:0203E070 (攻撃する側へユニットデータへのRAMポインタ(0203A3ECへ) )
@r7 
@r8 結果を格納する song id
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
ldr	r4, =0x0202BBF4 @ @ChapterData

ldr r7,BattleBGM_Table
sub r7,r7,#0x08
Loop:
add	r7,r7,#0x08

ldrh r0,[r7,#0x0]
cmp r0,#0x0
beq   NOTFOUND_UnitBGM  @term 終端

@CHECKMAP
ldrb r0,[r7,#0x3]
cmp r0,#0xFF
beq CHECKUNIT

ldrb r1,[r4,#0xE] @@ChapterData->MAPID
cmp r0,r1
bne Loop



CHECKUNIT:

ldrb r0,[r7,#0x2]
cmp r0,#0x00
beq   CHECK_BATTLEANIME

ldrb	r1, [r4, #0xF]	@フェイズ 0=自軍,0x40=友軍,0x80=敵軍
cmp     r1,#0x80
beq		CHECKUNIT_CHECK_LEFT
ldr r1,[r6,#0x0] @ @Unit->ROM_UnitForm
b		CHECKUNIT_CHECK_JOIN

CHECKUNIT_CHECK_LEFT:
ldr r1,[r5,#0x0] @ @Unit->ROM_UnitForm

CHECKUNIT_CHECK_JOIN:
ldrb r1,[r1,#0x4] @ROM_UnitForm->id
cmp r0,r1
bne Loop


CHECK_BATTLEANIME:
ldrh r0,[r7,#0x4]
cmp r0,#0x00
beq CHECK_FLAG

sub r0,r0,#1    @戦闘アニメは ID-1 して格納されるため

ldrb	r1, [r4, #0xF]	@フェイズ 0=自軍,0x40=友軍,0x80=敵軍
cmp     r1,#0x80
beq		CHECK_BATTLEANIME_CHECK_LEFT
ldr r1,=#0x0203DFF0
ldrh r1,[r1,#0x2] @battle anime id - 1
b		CHECK_BATTLEANIME_CHECK_JOIN

CHECK_BATTLEANIME_CHECK_LEFT:
ldr r1,=#0x0203DFF0
ldrh r1,[r1,#0x0] @battle anime id - 1

CHECK_BATTLEANIME_CHECK_JOIN:

cmp r0,r1
bne Loop

CHECK_FLAG:
ldrh	r0,[r7,#0x06] @data->flag
cmp	r0,#0x00
beq	FOUND_THE_BGM

ldr	r2, =#0x0807A0C8
mov	r14, r2
.short	0xF800
cmp	r0,#0x00
beq	Loop

FOUND_THE_BGM:
ldrh r2,[r7,#0x0]
mov  r8,r2
b    EXIT;


@ユニットごとのBGMが見つからなかった場合
@章ごとに設定する戦闘音楽を参照する
NOTFOUND_UnitBGM:

@FE6,FE7の場合、章データに空きがないので、
@所属による切り替えはサポートできない。
@よって、通常のルーチンに移動する.

ldr r1, =#0x0203DFF8
ldr r0, =#0x0203DFE4
mov r2, #0x0
ldrh r0, [r0, #0x8]
lsl r0  ,r0 ,#0x1
add r0  ,r0, r1


ldr		r2,=#0x08068662+1
bx		r2


EXIT:
@元に戻す.
@r8にsong_idが入っている.
ldr		r0,=#0x08068670+1
bx		r0

.ltorg
.align
BattleBGM_Table:
@POIN BattleBGM_Table
