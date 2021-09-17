@HOOK 0xBC5F6	@{J}
@HOOK 0xB7B96	@{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.macro blh_keep to
  push {r3}
  ldr r3, =\to
  mov lr, r3
  pop {r3}
  .short 0xf800
.endm


@壊すコードの再送
blh 0x080a8ce0   @GetEntryByChapterWinDataArray	{J}
@blh 0x080a429c   @GetEntryByChapterWinDataArray	{U}
@r0 == pointer (turn record)

push {r0,r6,r7}  @r0は大切なポインタなので保護します。
                 @r6は内部で使うので予約します。 TABLEのポインタの引き回し
                 @r7はMapIDを使うために予約します。
                 @r4は、元関数から引き継ぎます。 procが入ってます
                 @r5は、元関数から引き継ぎます。 スクロール情報が入ってます

@ポインタからMapIDを取得します
ldr r0, [r0, #0x0]
lsl r0 ,r0 ,#0x19
lsr r0 ,r0 ,#0x19
mov r7, r0         @MapIDの保存

@追加情報テーブルのindexにprocs->0x29を利用する
CheckMapIDLoop:
mov r1, #0x29
ldrb r1, [r4, r1]
cmp  r1, #0xff  @一応暴走防止のために追加しておく
beq  NotFound

ldr  r6, TABLE
lsl  r1, r1 ,#0x3  @sizeof()==0x8 なので、indexの数を8倍にする
add  r6, r1

CheckTerm:         @終端データ0xffだったら強制終了
ldrb r0, [r6,#0x0] @Table->MapID
cmp  r0, #0xff
beq  NotFound

CheckEditon:
ldrb r0, [r6,#0x1] @Table->Edtion
cmp  r0, #0xff
beq  CheckFlag
ldr  r2,=0x202BCEC @ChapterData	@{J}
@ldr  r2,=0x202BCF0 @ChapterData	@{U}
ldrb r1,[r2,#0x1B]  @ChapterData->Edition
cmp  r0, r1
bne  IgnoreData

CheckFlag:
ldrh r0,[r6,#0x02]  @     W2:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

blh  0x080860D0     @CheckFlag  Flag=r0  Result=r0:bool	{J}
@blh  0x08083da8     @CheckFlag  Flag=r0  Result=r0:bool	{U}
cmp  r0,#0x00
bne  Found

IgnoreData: @条件が不一致なので無視する
mov r1, #0x29
ldrb r0, [r4, r1]
add  r0, #0x1
strb r0, [r4, r1]
b    CheckMapIDLoop

Found:
ldrb r0, [r6,#0x0] @Table->MapID
cmp  r0, r7        @CheckMapID
bne  NotFound

ShowExtraInfo:
mov  r0, r6          @CurrentTable
ldrb r1, [r5, #0x0]  @Scroll
bl   ShowTextLine

NextEntry:
mov r1, #0x29
ldrb r0, [r4, r1]
add  r0, #0x1
strb r0, [r4, r1]

pop {r0,r6,r7}
ldr r3, =0x080BC60C|1	@{J}
@ldr r3, =0x080B7BAC|1	@{U}
bx  r3

@追加データがない場合は、通常の処理を行う
NotFound:
pop {r0,r6,r7}
ldrb r1, [r5, #0x0]
blh 0x080bc2a4   @クリアターン一覧の描画	{J}
@blh 0x080B7800   @クリアターン一覧の描画	{U}

ldr r3, =0x080BC600|1	@{J}
@ldr r3, =0x080B7BA0|1	@{U}
bx  r3


@r0 = CurrentTable
@r1 = scroll
ShowTextLine:
push {r4,r5,r6,r7,lr}
mov r7, r10
mov r6, r9
mov r5, r8
push {r5,r6,r7}
sub sp, #0x10
str r0,[sp, #0x8] @CurrentTable

mov r4 ,r1   @Scroll
mov r0, #0x0
str r0,[sp, #0xc]	@Scroll
mov r0 ,r4
mov r1, #0x9
blh 0x080d6690   @__modsi3	{J}
@blh 0x080d1994   @__modsi3	{U}

mov r9, r0
lsl r6 ,r4 ,#0x1
mov r0, #0x1f
and r6 ,r0
lsl r7 ,r6 ,#0x5
lsl r0 ,r6 ,#0x6
ldr r1, =0x020234A8 @BG 1 wram buffer	{J}	{U}
mov r10, r1
add r0, r10
mov r1, #0x1f
mov r2, #0x1
mov r3, #0x0
blh_keep 0x080dc0e4   @TileMap_FillRect	{J}
@blh_keep 0x080d74a8   @TileMap_FillRect	{U}

mov r0, #0x2
blh 0x08001efc   @BG_EnableSyncByMask	{J}
@blh 0x08001fac   @BG_EnableSyncByMask	{U}
ldr r3, =0x08AC0EDC		@{J}
@ldr r3, =0x08A3D674	@{U}
mov r8, r3
mov r0, r9
lsl r4 ,r0 ,#0x3
ldr r0, [r3, #0x0]
add r0 ,r0, r4
blh 0x08003cf8   @TextVRAMClearer	{J}
@blh 0x08003dc8   @TextVRAMClearer	{U}

mov r5 ,r4
add r5, #0x48
mov r1, r8
ldr r0, [r1, #0x0]
add r0 ,r0, r5
blh 0x08003cf8   @TextVRAMClearer	{J}
@blh 0x08003dc8   @TextVRAMClearer	{U}

ldr r3,[sp, #0x8]	@CurrentTable
ldrh r0, [r3, #0x4] @CurrentTable->TextID
blh 0x08009fa8   @GetStringFromIndex	{J}
@blh 0x0800a240   @GetStringFromIndex	{U}

mov r2 ,r0
mov r1, r8
ldr r0, [r1, #0x0]
add r0 ,r0, r5
mov r1 ,r7

ldr r3,[sp, #0x8]	@CurrentTable
ldrb r3, [r3, #0x7] @CurrentTable->AddX
add r1, r3
lsl r1 ,r1 ,#0x1
add r1, r10

ldr r3,[sp, #0xc]   @get scroll
str r3,[sp, #0x0]   @
str r2,[sp, #0x4]   @Text RAM Buffer

ldr r3,[sp, #0x8]	@CurrentTable
ldrb r2, [r3, #0x6] @CurrentTable->Color
mov r3, #0x0 @add X
blh_keep 0x08004374   @DrawTextInline	@{J}
@blh_keep 0x0800443C   @DrawTextInline	@{U}

add sp, #0x10
pop {r3,r4,r5}
mov r8, r3
mov r9, r4
mov r10, r5
pop {r4,r5,r6,r7}
pop {r0}
bx r0

.align
.ltorg
TABLE:
