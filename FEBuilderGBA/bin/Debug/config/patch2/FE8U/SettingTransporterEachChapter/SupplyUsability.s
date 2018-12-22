@Call 23F66         @FE8U

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
blh 0x0803161c      @FE8U HasConvoyAccess
cmp r0, #0x0
beq ReturnFalse

ldr r0,=0x0202BCF0  @FE8U (gChapterData )
ldrb r0, [r0, #0xe] @(ChapterData@ステージの領域.マップID )
ldr r1, Table
ldrb r1,[r1,r0]

cmp  r1,#0x2        @所定のユニットだけが輸送隊を使える    2(FE8ディフォルト)
beq  ReturnTrue

cmp  r1,#0x3        @全員輸送隊を呼べる    3  (西方の拠点と同じ)
beq  ReturnAlways

cmp  r1,#0x4        @フラグ0x27有効なら所定のユニットが利用可能
beq  CheckFlag27


ReturnFalse:        @利用できない
mov  r0, #0x3
ldr  r3,=0x08023FB2|1    @FE8U
bx   r3

ReturnAlways:       @全員利用できる
mov  r0, #0x1
ldr  r3,=0x08023FB2|1    @FE8U
bx   r3

CheckFlag27:
mov r0,#0x27
blh 0x08083DA8      @フラグ0x27の確認
cmp r0,#0x00
beq ReturnFalse

ReturnTrue:         @通常ルールで利用できる
ldr  r3,=0x08023F70|1    @FE8U
bx   r3


.ltorg
.align
Table:
