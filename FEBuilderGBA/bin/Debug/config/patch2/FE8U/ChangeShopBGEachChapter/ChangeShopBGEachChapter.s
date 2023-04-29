@Call 0x080B993C FE8J
@@Call 0x080B4DB4 FE8U
@r4   work  (use Table)
@r5   work  (use mapid)
@r6   work  (use shop type)

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


.thumb
push {r0,r6}

@ldr r0, =0x080B8DEC	@Pointer->ShopProcs	@{J}
ldr r0, =0x080B4264	@Pointer->ShopProcs	@{U}
ldr r0, [r0]
@blh 0x08002dec   @Find6C	@{J}
blh 0x08002e9c   @Find6C	@{U}
cmp r0, #0x0
beq default_bg

add r0, #0x61
ldrb r6, [r0, #0x0]  @店の種類の一時保存

IsPrepShop:    @編成準備店かどうか.
@ldr  r0, =0x0202BCAC @FE8J  gMainLoopEndedFlag	@{J}
ldr r0, =0x0202bcb0 @FE8U  gMainLoopEndedFlag	@{U}
ldrb r1, [r0, #0x4] @ gGameStatebits
mov r0, #0x10
and r0 ,r1
cmp r0, #0x0
beq IsWorldMapShop
	mov r6, #0x10    @Prep Shop
	b SetMapID

IsWorldMapShop:
@blh  0x080BEEE8       @FE8J IsWorldMap	@{J}
blh 0x080BA054       @FE8U IsWorldMap	@{U}
cmp r0,#0x00
beq SetMapID
	mov r5, #0xfe    @WorldmapShopなので章IDを0xfeにする
	b LoopStart

SetMapID:
	@ldr   r5,=0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)	@{J}
	ldr  r5,=0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)	@{U}
	ldrb  r5,[r5,#0xE]   @     ChapterData->MAPID

LoopStart:

ldr  r4,Table
sub  r4,#0x8        @     ループ処理が面倒なので、最初に0x8バイト差し引きます.

Loop:
add  r4,#0x8        @     次のデータへ
ldr  r0,[r4,#0x00]  @     Check EOF
cmp  r0,#0x0
beq  default_bg

CheckShopType:
ldrb r0,[r4,#0x02]  @     B2:$COMBO ShopType.txt=type
cmp  r0,r6
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

pop  {r0,r6}
ldrh r0,[r4,#0x00]  @     W0:BG=BG
mov r1, #12
mul r0, r1

@ldr r4, =0x0800EAA4 @Pointer->BG	@{J}
ldr r4, =0x0800E894 @Pointer->BG	@{U}
ldr r4, [r4]
add r4 , r0

ldr r0, [r4, #0x8]	@BG->Pal
mov r2, #0x80
lsl r2 ,r2 ,#0x1
mov r1 ,r2
@blh 0x08000d68   @CopyToPaletteBuffer	@{J}
blh 0x08000db8   @CopyToPaletteBuffer	@{U}

mov r0, #0x3
@blh 0x08000f3c   @GetBackgroundTileDataOffset	@{J}
blh 0x08000f8c   @GetBackgroundTileDataOffset	@{U}
mov r1 ,r0
mov r0, #0xc0
lsl r0 ,r0 ,#0x13
add r1 ,r1, r0

ldr r0, [r4, #0x0]	@BG->Image
@blh 0x08013008   @UnLZ77Decompress	@{J}
blh 0x08012f50   @UnLZ77Decompress	@{U}

ldr r0, =0x020244A8 @gBG3TilemapBuffer BG 3 wram buffer	@{J}	@{U}
ldr r1, [r4, #0x4]	@BG->TSA
mov r2, #0x80
lsl r2 ,r2 ,#0x8
@blh 0x080dc0dc   @CallARM_FillTileRect	@{J}
blh 0x080d74a0   @CallARM_FillTileRect	@{U}

@ldr   r3,=0x080B9968    @FE8J 復帰	@{J}
ldr   r3,=0x080B4DE0    @FE8J 復帰	@{U}
mov   pc,r3


default_bg:       @見つからなかったのでディフォルトのルーチンへ復帰する
pop {r0,r6}
mov r1, #0xe0
lsl r1 ,r1 ,#0x1
mov r2, #0x20
@blh 0x08000d68   @CopyToPaletteBuffer	@{J}
blh 0x08000db8   @CopyToPaletteBuffer	@{U}

@ldr   r3,=0x080B9946    @FE8J ディフォルトのルーチンへ復帰する	@{J}
ldr   r3,=0x080B4DBE    @FE8U ディフォルトのルーチンへ復帰する	@{U}
mov   pc,r3

.ltorg
.align
Table:
