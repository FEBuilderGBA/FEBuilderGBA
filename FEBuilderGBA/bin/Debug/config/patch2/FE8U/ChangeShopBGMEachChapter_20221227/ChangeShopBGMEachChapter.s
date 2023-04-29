@@Call 0x080B9780 FE8J
@Call 0x080B4BF8 FE8U
@r0   procs
@r1   work
@r2   work
@r4   work  (use Table)
@r5   work  (use mapid)
@r6   (copy r0)
@r7   work  (use shop type)

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


.thumb

push {r7}
mov r6 ,r0
add r0, #0x61
ldrb r4, [r0, #0x0]  @店の種類の一時保存

IsPrepShop:    @編成準備店かどうか.
@ldr  r0, =0x0202BCAC @FE8J  gMainLoopEndedFlag
ldr r0, =0x0202bcb0 @FE8U  gMainLoopEndedFlag
ldrb r1, [r0, #0x4] @ gGameStatebits
mov r0, #0x10
and r0 ,r1
cmp r0, #0x0
beq IsWorldMapShop
	mov r4, #0x10    @Prep Shop
	b SetMapID

IsWorldMapShop:
@blh  0x080BEEE8       @FE8J IsWorldMap
blh 0x080BA054       @FE8U IsWorldMap
cmp r0,#0x00
beq SetMapID
	mov r5, #0xfe    @WorldmapShopなので章IDを0xfeにする
	b LoopStart

SetMapID:
	@ldr   r5,=0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)
	ldr  r5,=0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)
	ldrb  r5,[r5,#0xE]   @     ChapterData->MAPID

LoopStart:
mov  r7, r4         @     店の種類
ldr  r4,Table
sub  r4,#0x8        @     ループ処理が面倒なので、最初に0x4バイト差し引きます.

Loop:
add  r4,#0x8        @     次のデータへ
ldr  r0,[r4,#0x00]  @     W0:SONG=SongData
cmp  r0,#0x0
beq  default_bgm

CheckShopType:
ldrb r0,[r4,#0x02]  @     B2:$COMBO ShopType.txt=type
cmp  r0,r7
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckMapID:
ldrb r0,[r4,#0x03]  @     B3:MAPID=MAPID(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckFlag

cmp  r0,r5
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckFlag:
ldrh r0,[r4,#0x04]  @     B4:FLAG
cmp  r0,#0x00       @     フラグを利用しないなら発見
beq  Found

@blh 0x080860d0 @CheckFlag	@{J}
blh 0x08083da8 @CheckFlag	@{U}
cmp  r0, #0x0
beq  Loop


Found:              @探索したデータにマッチした。
pop {r7}

ldrh r0,[r4,#0x00]  @     W0:SONG=SongData
mov  r1,#0x0

@ldr   r3,=0x080B9798    @FE8J BGMを再生させるルーチンに復帰する.
ldr  r3,=0x080B4C10    @FE8U BGMを再生させるルーチンに復帰する.
mov   pc,r3


default_bgm:       @見つからなかったのでディフォルトのルーチンへ復帰する
pop {r7}

mov r0, r6
add r0, #0x61
ldrb r0, [r0, #0x0]
cmp r0, #0x0       @フラグレジスタだけ変化させて

@ldr   r3,=0x080B9788    @FE8J ディフォルトのルーチンへ復帰する
ldr  r3,=0x080B4C00    @FE8U ディフォルトのルーチンへ復帰する
mov   pc,r3

.ltorg
.align
Table:
