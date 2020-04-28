@毒ダメージを変更
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@Call 259DC
@r0 work
@r1 work
@r2 RAMUnit
@r3 
@r4 temp
@r5 temp
@r6 temp
@r7 keep unit id
@
@
@struct 
@{
@struct{
@	byte	damage;		ダメージ		
@   byte	rand;		ブレ幅
@	byte	map_id;		章			FF=ANY
@	byte	unit_id;	ユニット	00=ANY
@	byte	class_id;	クラス		00=ANY
@	byte	maxhp;		MAXHP		00=ANY このMAXHP以上だったら
@	uhsort	flag;		フラグ		00=ANY
@}//sizeof(8bytes)
@
@
mov r5, r2  @RAMUnit
ldr	r4, =#0x202bcec @Chaptor Pointer  (@ChapterData)

ldr r6,ChangePoisonDamage_Table
sub r6,r6,#0x08
Loop:
add	r6,r6,#0x08

ldr r0,[r6,#0x0]
cmp r0,#0x0
beq   NOTFOUND  @term 終端

CHECKMAP:
ldrb r0,[r6,#0x2]
cmp r0,#0xFF
beq CHECKUNIT

ldrb r1,[r4,#0xE] @@ChapterData->MAPID
cmp r0,r1
bne Loop


CHECKUNIT:
ldrb r0,[r6,#0x3]
cmp r0,#0x00
beq   CHECK_CLASS

ldr r1,[r5,#0x0] @ @Unit->ROM_UnitForm
ldrb r1,[r1,#0x4] @ROM_UnitForm->id
cmp r0,r1
bne Loop


CHECK_CLASS:
ldrb r0,[r6,#0x4]
cmp r0,#0x00
beq   CHECK_MAPHP

ldr r1,[r5,#0x4] @ @Unit->ROM_ClassForm
ldrb r1,[r1,#0x4] @ROM_ClassForm->id
cmp r0,r1
bne Loop


CHECK_MAPHP:
ldrb r0,[r6,#0x5]

ldrb r1,[r5,#0x12] @ @Unit->MaxHP
cmp r0,r1
bgt Loop


CHECK_FLAG:
ldrh	r0,[r6,#0x06] @data->flag
cmp	r0,#0x00
beq	FOUND

blh 0x080860D0 @CheckFLag
cmp	r0,#0x00
beq	Loop

FOUND:
ldrb r0,[r6,#0x1]  @毒のブレ幅

blh 0x08000c58   //NextRN_N
mov r3 ,r0

ldrb r1,[r6,#0x0]  @毒ダメージ
add  r3, r1     @ダメージ量
b    EXIT;


@見つからなかった場合はディフォルト値
NOTFOUND:
ldr r3,=0x080259E6  @毒のブレ幅
ldrb r0, [r3]

blh 0x08000c58   //NextRN_N
mov r3 ,r0

ldr r1, =0x080259EE  @最低毒ダメージ
ldrb r1, [r1]
add  r3, r1     @ダメージ量

EXIT:

@r3 ダメージ量
mov r2, #0x10      @X
ldsb r0, [r5, r2]
mov r2, #0x11      @Y
ldsb r1, [r5, r2]
mov r2, #0xb       @部隊ID
ldsb r2, [r5, r2]

ldr r4,=0x080259F6+1   @AddTargetの位置に戻る
bx  r4

.ltorg
.align
ChangePoisonDamage_Table:
@POIN ChangePoisonDamage_Table
