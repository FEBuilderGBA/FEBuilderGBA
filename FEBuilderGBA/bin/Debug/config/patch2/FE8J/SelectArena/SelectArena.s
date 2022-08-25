@闘技場のレア敵の選出のルーチンを改善する
@リストで置換するには数が多くなりすぎた。
@なんか偏りがある
@これらを全部解決する!
@Overaide 08031858

.thumb

.equ origin, 0x08031858
.equ CheckFlag, . + 0x080860d0 - origin
.equ NextRN_N, . + 0x08000c58 - origin

.equ Type_CannotPromoted, 0x00000100
@.equ Type_CannotSuperArena, 0x10000000	@エイリーク専用武器
@.equ Type_CannotAfterEpilogue, 0x20000000	@エフラム専用武器
.equ Type_CannotSuperArena, DATA+0
.equ Type_CannotWithFlag, DATA+4
.equ WithFlag_Flag, DATA+8

push {r4-r7,lr}
@r0 は武器タイプID 00=剣
mov r6, r0

@r5は戦えないクラスタイプです。
@この特徴ビットフラグが立っている敵とは戦えません
mov r5 ,#0x0

ldr r4, =0x0203A8EC @ArenaData
ldr r4, [r4, #0x0]  @ArenaData->playerUnit

@上級職チェックが必要か?
CheckPromoted:
ldr r2, [r4, #0x4]  @ArenaData->playerUnit->Class
ldr r3, [r4, #0x0]  @ArenaData->playerUnit->Unit
ldr r0, [r3, #0x28]
ldr r1, [r2, #0x28]
orr r0, r1
ldr r1, =Type_CannotPromoted
and r0, r1
bne CheckSuperArena

ImNotPromoted:
@私は下級職なので、上級職とは戦えません
@ldr r1, =Type_CannotPromoted
orr r5, r1

@SuperArenaのフラグが立っているか?
CheckSuperArena:

ldr r0, [r4, #0xc]	@ArenaData->playerUnit->Status
lsr r0 ,r0 ,#0x11
mov r1, #0x7
and r0 ,r1
cmp r0, #0x4
bhi CheckAfterEpilogue	@SuperArena

ImNotSuperArena:
@私はSuperArenaにいけません
ldr r0, Type_CannotSuperArena
orr r5, r0

@特種フラグのフラグが立っているか?
CheckAfterEpilogue:
ldr r0, WithFlag_Flag  @特種フラグフラグ
bl  CheckFlag   @CheckFlag フラグ状態確認関数 RET=結果BOOL r0=確認するフラグ:FLAG
cmp r0, #0x0
bne ClassTableCount

ImNotAfterEpilogue:
@私は特種フラグの敵とは戦えません
ldr r0, Type_CannotWithFlag
orr r5, r0
@b ClassTableCount

ClassTableCount:
mov r1, #0x0
ldr r3, =0x08017860	@class table pointer
ldr r3, [r3]
@sub r3, #84
ClassTableCount_Loop:
add r3, #84
add r1, #0x1
ldrb r0, [r3, #0x4]
cmp r0, r1	@class->id != id
beq ClassTableCount_Loop

@r7 class table count
mov r7, r1

ReSelect:
mov r0, r7       @r7=class table count
sub r0, #0x1     @ SkipClassID0
bl  NextRN_N
add r0, #0x1     @ SkipClassID0

mov r1, #84
mul r1, r0       @class_id * sizoef(class_table)

ldr r3, =0x08017860	@class table pointer
ldr r3, [r3]
add r4, r3, r1

@r4は選出されたクラスポインタ

@直前に戦った相手とは戦わない
ldr r2, =0x0203A8EC	@gArena
ldrb r2, [r2, #0x10]	@gArena->opponetClassID
cmp r0, r2		@直前に闘技場で戦った相手だったら再選し直す
beq ReSelect

@特徴ビットフラグのチェック
CheckFeatureBitFlags:
ldr r0, [r4, #0x28] @Class->FeatureBitFlags
and r0, r5         @r5は戦えない相手のビットマスク
bne ReSelect       @結果が0ではない 何かの理由によりこの敵とは戦うことができません


@反撃できますか?
CanCounterAttack:
cmp r6, #0x3
beq CanCounterAttack_Bow
cmp r6, #0x4
bge CanCounterAttack_Magic

@近接の敵としか戦えない 1-1
CanCounterAttack_Melee:
ldr r0, [r4, #0x2c]
lsl r0, #0x8 @上位8ビットには弓があるのでマスクする
lsr r0, #0x8
cmp r0, #0x0
bgt Fight @剣槍斧を相手が持ってる
b   CanCounterAttack_HasMagic_AnimaLightDark

CanCounterAttack_Bow:
mov  r0, #0x2F
ldrb r0, [r4, r0] @弓を相手が持ってる
cmp  r0, #0x0
bgt  Fight
b    CanCounterAttack_HasMagic_AnimaLightDark


CanCounterAttack_Magic:
ldr r0, [r4, #0x2c]
cmp r0, #0x0
bgt Fight @剣槍斧弓を相手が持ってる

CanCounterAttack_HasMagic_AnimaLightDark:
ldr r0, [r4, #0x30]
lsr r0, #0x8 @>>8
cmp r0, #0x0
beq ReSelect
@理光闇を相手が持ってる

Fight:
ldrb r0, [r4,#0x4]	@Class->ID
pop {r4-r7}
pop {r1}
bx r1

.ltorg
DATA:
