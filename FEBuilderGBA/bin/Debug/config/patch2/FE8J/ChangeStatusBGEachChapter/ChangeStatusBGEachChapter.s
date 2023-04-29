@Call 0x0808A820 FE8J
@r0 
@r1 
@r2 
@r3 
@r4 keep
@r5 Table領域と利用する.

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


.thumb
.org 0x00

push {r5}           @処理的には不要だが、一応、保存しておこう.
                    @r4は必要なデータが入っているので、そもそも変更しない.



ldr  r5,Table
sub  r5,#0x10        @     ループ処理が面倒なので、最初に0x10(16)バイト差し引きます.

Loop:
add  r5,#0x10       @     次のデータへ
ldr  r0,[r5,#0x04]  @     P4:ZIMAGE=Image
cmp  r0,#0x00       @     データのポインタがない場合、終端とみなす.
beq  load_default_bg     @データがないので、ディフォルトの背景をロードして終了!

CheckMapID:
ldrb r0,[r5,#0x00]  @     B0:MAPID=MAPID(0xFF=ANY)
cmp  r0,#0xFF       @     ANY MAPID ?
beq  CheckAllegiance

ldr  r2,=#0x202BCEC @FE8J Chaptor Pointer  (@ChapterData)
ldrb r1,[r2,#0xE]   @     ChapterData->MAPID
cmp  r0,r1
bne  Loop           @     条件不一致なので、次のループへ continue;

CheckAllegiance:
ldrb r3,[r5,#0x01]  @     B1=Allegiance(0xFF=ANY,0x00=Player,0x40=NPC,0x80=Enemy)
cmp  r3,#0xFF       @     ANY Allegiance ?
beq  CheckFlag

bl GetUnitAllegiance @所属の取得 r0,r1,r2 を破壊 結果は r0に戻る
cmp  r3,r0                @r0 == 0x00  Player
                          @r0 == 0x40  NPC
                          @r0 == 0x80  Enemy
bne  Loop           @     条件不一致なので、次のループへ continue;



CheckFlag:
ldrh r0,[r5,#0x02]  @     W2:Flag=Flag(0x00=ANY)
cmp  r0,#0x0        @     ANY Flag ?
beq  Found

blh  0x080860D0     @FE8J CheckFlag  Flag=r0  Result=r0:bool
cmp	r0,#0x00
beq  Loop           @     条件不一致なので、次のループへ continue;

b    Found          @発見!


GetUnitAllegiance:        @座標からユニットの所属を取得する
                          @Kirbのルーチンを元にした
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

pop {pc}                  @サブルーチンの終わり
                          @r0 == 0x00  Player
                          @r0 == 0x40  NPC
                          @r0 == 0x80  Enemy


Found:              @探索したデータにマッチした。
                    @ユーザが指定した背景をロードする
                    @r5 Table(current)
ldr r0,[r5,#0x04]   @     背景画像
ldr r1,=0xFFFFFFFF  @     FEBuilderGBAの都合 データが0件では困るのでダミーデータがあるよ
cmp r0,r1
beq load_default_bg

ldr r1,=0x0600B000  @FE8J 背景をロードするVRAM
blh 0x08013008      @FE8J UnLZ77Decompress

ldr r0,[r5,#0x0C]   @     背景パレット
mov r1, #0xc0
lsl r1 ,r1 ,#0x1
mov r2, #0x80
blh 0x08000d68      @FE8J CopyToPaletteBuffer 

ldr r0,[r5,#0x08]   @     背景TSA
mov r1 ,r4          @     ロードする領域はr4に書かれている。r4は壊さないように.
blh 0x08013008      @FE8J UnLZ77Decompress
b Exit

@設定がない場合は、ディフォルトの背景をロードする.
load_default_bg:    @ディフォルトの設定をロードする
ldr r0,=0x0808A8C4  @FE8J ディフォルトのステータス画面の背景が書いてあるアドレス
ldr r0,[r0]         @     ポインタ参照することで、リポイントに耐える.
ldr r1,=0x0600B000  @FE8J 背景をロードするVRAM
blh 0x08013008      @FE8J UnLZ77Decompress

ldr r0,=0x0808A8CC  @FE8J ディフォルトのステータス画面のパレットが書いてあるアドレス
ldr r0,[r0]         @     ポインタ参照することで、リポイントに耐える.
mov r1, #0xc0
lsl r1 ,r1 ,#0x1
mov r2, #0x80
blh 0x08000d68      @FE8J CopyToPaletteBuffer 

ldr r0,=0x0808A8D0  @FE8J TSA
ldr r0,[r0]         @     ポインタ参照することで、リポイントに耐える.
mov r1 ,r4          @     ロードする領域はr4に書かれている。r4は壊さないように.
blh 0x08013008      @FE8J UnLZ77Decompress

Exit:
pop {r5}
ldr r0,=0x0808A83C+1 @FE8J 戻るアドレス
bx  r0

.ltorg
.align
Table:
