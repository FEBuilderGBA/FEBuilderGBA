@Call 0x08020252 FE8U
@r0
@r1
@r2
@r3
@r4 nazo

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


.thumb
push {r4}

ldr  r4,Table
sub  r4,#0xC        @     ループ処理が面倒なので、最初に0xC(12)バイト差し引きます.

Loop:
add  r4,#0xC        @     次のデータへ
ldr  r0,[r4,#0x04]  @     P4:ZIMAGE=Image
cmp  r0,#0x00       @     データのポインタがない場合、終端とみなす.
beq  load_default_bg     @データがないので、ディフォルトの背景をロードして終了!

CheckMapID:
ldrb r0,[r4,#0x00]  @     B0:MAPID=MAPID(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckEdition

ldr  r2,=#0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckEdition:
ldrb r0,[r4,#0x01]  @     B1=Editon(0xFF=ANY,0x00=序盤,0x01=エイリーク,0x02=エフラム)
cmp  r0,#0xFF       @     ANY Editon ?
beq  CheckFlag

ldr  r2,=#0x202BCF0 @FE8U Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0x1B]  @     ChapterData->Edition
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;



CheckFlag:
ldrh r0,[r4,#0x02]  @     W2:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

blh  0x08083DA8     @FE8U CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq  Loop           @     条件不一致なので、次のループへ continue;

Found:              @探索したデータにマッチした。
                    @ユーザが指定した背景をロードする
                    @r4 Table(current)

ldr r0,[r4,#0x04]   @     背景画像
ldr r1,=0xFFFFFFFF  @     FEBuilderGBAの都合 データが0件では困るのでダミーデータがあるよ
cmp r0,r1
beq load_default_bg

ldr r1, =0x06014000 @FE8U (OBJ_VRAM0_BITMAP )
blh 0x08012f50      @FE8U UnLZ77Decompress 

ldr r0,[r4,#0x08]   @     パレット


b Exit

@設定がない場合は、ディフォルトの背景をロードする.
load_default_bg:    @ディフォルトの設定をロードする

ldr r0, =0x080202B0 @FE8U 背景画像
ldr r0,[r0]         @     ポインタ参照することで、リポイントに耐える.

ldr r1, =0x06014000 @FE8U (OBJ_VRAM0_BITMAP )
blh 0x08012f50      @FE8U UnLZ77Decompress 
ldr r0, =0x080202B8 @FE8U (章タイルを出す時の素材@PALETTE_POINTER )
ldr r0,[r0]         @     ポインタ参照することで、リポイントに耐える.

Exit:
pop {r4}
ldr r3,=0x0802025C+1 @FE8U 戻るアドレス
bx  r3

.ltorg
.align
Table:
