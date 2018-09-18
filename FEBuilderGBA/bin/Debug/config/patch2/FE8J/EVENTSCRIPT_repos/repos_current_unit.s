@瞬間移動 操作中のユニット (REPOS)
@
@[SVALB] 40 0D [X] [Y] [ASM+1]
@[SVALB] 41 0D [X] [Y] [ASM+1]  移動先が塞がれていたらNG
@
@Author 7743
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
.thumb
	push	{r4,lr}

	ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

	ldrb r0, [r4, #0x0]       @引数0 [FLAG]
	mov  r1,#0x1
	and  r0,r1

	cmp  r0,#0x0
	beq  SelectUnit

CheckIfBlocked:
	@
	@[1] と [3] の命令コードの場合、塞がれていら移動させないのでチェックする.
	@
	ldr  r0,=0x0202E4D4       @gMapUnit //*gMapUnit[y][x] = 部隊表ID
	ldr  r0,[r0]

	ldrb r1, [r4, #0x2]       @引数1 [X]
	ldrb r2, [r4, #0x3]       @引数2 [Y]
	
	lsl  r2,r2,#0x2           @ y << 2
	ldr  r0,[r0,r2]           @gMapUnit[y]
	ldrb r0,[r0,r1]           @gMapUnit[y][x]

	cmp  r0,#0x00             @ワープ先に誰かいるか?
	bne  Term                 @誰かいる場合は、移動させない.

SelectUnit:
	ldr  r0,=#0x03004DF0      @操作中のユニットの情報を取得
	ldr  r0,[r0]
	                          @RAM UNIT POINTERの取得
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了

Change:
	ldrb r1, [r4, #0x2]       @引数1 [X]
	ldrb r2, [r4, #0x3]       @引数2 [Y]

	strb r1,[r0,#0x10]        @Unit@10	byte	X	座標
	strb r2,[r0,#0x11]        @Unit@11	byte	Y	座標

	@操作中のキャラクタの場合、常時イベントの後に、座標に位置を書き込む命令が来ます。
	@そのため、座標が上書きされてしまうので、
	@移動先の座標を格納しているメモリも改変する必要があります.
	ldr  r0,=#0x0203A954      @gActionData
	strb r1, [r0, #0xe]       @gActionData.X
	strb r2, [r0, #0xf]       @gActionData.Y

	blh  0x08019ecc   @RefreshFogAndUnitMaps 
	blh  0x08027144   @SMS_UpdateFromGameData 
	blh  0x08019914   @UpdateGameTilesGraphics 

Term:
	mov	r0, #0
	pop {pc,r4}
