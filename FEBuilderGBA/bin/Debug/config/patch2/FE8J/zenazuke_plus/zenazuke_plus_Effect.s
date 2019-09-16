@全預け+
@GiveAll+
@
@武器と杖以外のアイテムを輸送隊に送ります。
@敵を救出している場合は、敵の全アイテムを輸送体に送ります。
@Send items other than weapons and staff to Supply.
@If rescuing enemy, send all enemy items to Supply.
@

.equ Error_FullText, Data+0

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

	push	{r4, lr}
	mov r4,r0
for_aid:
	ldr	r1, =0x03004DF0	@操作キャラのワークメモリへのポインタ	{J}
@	ldr	r1, =0x03004E50	@操作キャラのワークメモリへのポインタ	{U}
	ldr	r0, [r1, #0x0]	@UnitRAM構造体

	bl  LoadUnitByAID
	cmp r0,#0x00
	beq for_myself

	bl  SendSupply
	cmp r0 , #0x00
	beq full_supply

for_myself:
	ldr	r1, =0x03004DF0	@操作キャラのワークメモリへのポインタ	{J}
@	ldr	r1, =0x03004E50	@操作キャラのワークメモリへのポインタ	{U}
	ldr	r0, [r1, #0x0]	@UnitRAM構造体

	bl  SendSupply

	cmp r0 , #0x00
	beq full_supply

	ldr r0, =0x2BF
	bl   PlaySound

	mov	r0,#0x17
	b   Exit

full_supply:
	mov r0, #0x7B
	bl   PlaySound

	mov r0 ,r4
	ldr r1, Error_FullText

	blh 0x080502F4   @Menu_CallTextBox	{J}
@	blh 0x0804f580   @Menu_CallTextBox	{U}
	mov	r0,#0x08

Exit:

@輸送隊を呼び出した時と同じ挙動
	ldr r1, =0x0203A954    @(ActionData@gActionData._u00 ) {J}
@	ldr r1, =0x0203A958    @(ActionData@gActionData._u00 ) {U}

	ldr r2, =0x080A0658    @輸送隊とのアイテム交換時のActionIDをセット {J}
@	ldr r2, =0x0809E3B8    @輸送隊とのアイテム交換時のActionIDをセット {U}
	ldrb r2,[r2]           @ディフォルトは、1C

	strb r2, [r1, #0x11]   @ActionData@gActionData.unitActionType

	pop	{r4}
	pop {r1}
	bx  r1


SendSupply:
	push	{r4,r5,r6,r7, lr}
	mov		r4,r0
	
	mov     r7, #0x0
	ldrb	r1,[r4, #0xB]		@所属
	cmp		r1,#0x40
	bge     SendSupply_Start
	mov		r7, #0x01		@PlayerUnit

SendSupply_Start:
	mov		r1,#0x1E	@アイテム1 アイテムIDへ
	add     r4,r1

	mov		r5,r4		@書き込み用

	mov		r6,r4		@個数5つまでループする
	add		r6,#0xA		@5個*2バイト = 0xA

item_loop:
	cmp     r5,r6
	bge		character_item_zeropadding

	ldrb	r0,[r5,#0x00]	@アイテムID
	add		r5,#0x2

	cmp		r0,#0x00     @アイテム終端
	beq		character_item_zeropadding

	is_target_item:
	
		cmp r7,#0x02               @輸送隊フル
		beq is_target_item_false

		cmp r7,#0x00               @自軍でないなら全アイテムが対象
		beq character_item_pickup

		@アイテムテーブル
		blh 0x08017558		@GetROMItemStructPtr	{J}
@		blh 0x080177b0		@GetROMItemStructPtr	{U}

		@アイテムの属性の取得
		ldrb r1,[r0,#0x8]

		mov  r2, #0x01      @武器ですか？
		and  r2, r1
		cmp  r2, #0x00
		bne  is_target_item_false

		mov  r2, #0x04      @杖ですか？
		and  r2, r1
		cmp  r2, #0x00
		bne  is_target_item_false
		b   character_item_pickup

	is_target_item_false:

	sub		r0,r5,#0x02
	ldrh	r0,[r0,#0x00]	@アイテム

	@特定のアイテム以外はコピーする
	strh	r0,[r4,#0x0]
	add		r4,#0x2			@アイテムID 個数の2バイト配列のため

	b		character_item_next

character_item_pickup:

	sub		r0,r5,#0x02
	ldrh	r0,[r0,#0x00]	@アイテム

	bl AppendSupply
	cmp r0,#0x00
	beq SendSupply_supply_full

character_item_next:
	cmp		r4,r6
	blt		item_loop		@アイテム5を処理するまでループ

character_item_zeropadding:
	@アイテム欄も、ゼロ終端ではない。
	@余白ができていた場合、
	@5つのアイテム欄に余白ができないように0で埋めないとダメ。
	cmp		r6,r4
	ble		SendSupply_break

	mov		r0,#0x00
	strh	r0,[r4,#0x0]
	add		r4,r4,#0x2
	b		character_item_zeropadding

SendSupply_supply_full:
	mov		r7,#0x02	@輸送隊フル 

	@輸送隊フルになったアイテムは消してはいけないので復帰させる.
	b		is_target_item_false

SendSupply_break:
	
	cmp r7,#0x02               @輸送隊フル
	beq SendSupply_exit_false

SendSupply_exit_true:
	mov	r0, #0x01
	b	SendSupply_exit

SendSupply_exit_false:
	mov	r0, #0x00

SendSupply_exit:
	pop	{r4,r5,r6,r7}
	pop {r1}
	bx r1


@輸送隊に追加
AppendSupply:
	push {r4,r5,r6,lr}

	mov r4, r0

	@輸送隊アドレスの取得
	ldr		r0, =0x08031470	@{J}
@	ldr		r0, =0x08031524	@{U}
	ldr		r2, [r0]		@輸送隊アドレス
	ldrb	r3, [r0,#0x4]	@最大個数

	@ループ上限 = 開始アドレス+(個数*2バイト)
	lsl		r3,r3,#0x1		@r3 * 2
	add		r6,r3,r2		@終端

	@輸送隊アドレス
	mov		r5,r2

yusotai_loop:
	ldrb	r0,[r5,#0x0]	@アイテムID

	cmp		r0,#0x00		@アイテム終端
	beq     yusotai_store

	add		r5,#0x2			@アイテムID 個数の2バイト配列のため
	cmp		r5,r6
	blt		yusotai_loop		@上限アドレスを超えていれなければ続く

	mov		r0,#0x00		@輸送隊が満杯
	b       yusotai_exit

yusotai_store:
	@アイテムの格納
	strh	r4,[r5,#0x00]
	
	mov		r0, #0x01		@格納成功

yusotai_exit:
	pop	{r4,r5,r6}
	pop {r1}
	bx r1

PlaySound:
		push {lr}
		ldr r1, =0x0202BCEC  @ChapterData	{J}
@		ldr r1, =0x0202BCF0  @ChapterData	{U}
		add r1, #0x41
		ldrb r1, [r1, #0x0]
		lsl r1 ,r1 ,#0x1e
		cmp r1, #0x0
		blt PlaySound_Exit
			blh 0x080d4ef4   @効果音を鳴らす関数 m4aSongNumStart r0=鳴らしたい音番号:SOUND	{J}
@			blh 0x080D01FC   @効果音を鳴らす関数 m4aSongNumStart r0=鳴らしたい音番号:SOUND	{U}
PlaySound_Exit:
		pop {r0}
		bx r0

@救出しているユニット情報のロード
@救出していない場合は0を返す.
LoadUnitByAID:
	push {lr}
	mov r1, #0x1B       @救出してる？
	ldrb r3,[r0,r1]

	cmp r3,#0x00
	beq LoadUnitByAID_Exit_false

	ldr r0, =0x0202BE48 @UnitRAM {J}
@	ldr r0, =0x0202BE4C @UnitRAM {U}

	ldr r1,=#0x85 * 0x48 @Player+Enemy+NPC
	add r1,r0

LoadUnitByAID_next_loop:
	cmp r0,r1
	bgt LoadUnitByAID_Exit_false

	add r0,#0x48

	ldr r2, [r0]          @unitram->unit
	cmp r2, #0x00
	beq LoadUnitByAID_next_loop

	ldrb r2, [r0,#0xB]    @Get Unit Table ID
	cmp  r2, r3
	beq  LoadUnitByAID_Exit
	b    LoadUnitByAID_next_loop

LoadUnitByAID_Exit_false:
	mov  r0, #0x00

LoadUnitByAID_Exit:
	pop {r1}
	bx r1

.ltorg
Data:
