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
	cmp r0, #0x00
	beq Exit_False

	@敵を捕らえている必要がある
	ldrb r0, [r0, #0x1b]
	mov r1, #0x80
	and r1, r0
	cmp r1, #0x0
	beq for_myself  @敵を捕らえていないので、自分のアイテムだけ確認する

	blh 0x08019108   @{J}  GetUnitStruct RET=RAM Unit:@UNIT
	@blh 0x08019430   @{U}  GetUnitStruct RET=RAM Unit:@UNIT
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

		@有効な武器か杖か？
		bl IsEnableWeaponOfStaff
		cmp  r0, #0x01
		beq  is_target_item_false
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

@有効な武器か杖か？
IsEnableWeaponOfStaff:
	push {r4,r5,lr}

	mov r4,r0 @ItemID

	@ユーザの特殊指定
	bl CheckSPItems
	cmp r0,#0x1
	beq IsEnableItems_True
	cmp r0,#0x2
	beq IsEnableItems_Flase

	@装備できる武器ですか？
	ldr	r0, =0x03004DF0	@操作キャラのワークメモリへのポインタ	{J}
@	ldr	r0, =0x03004E50	@操作キャラのワークメモリへのポインタ	{U}
	ldr	r5, [r0, #0x0]	@UnitRAM構造体

	mov r0, r5  @UnitID
	mov r1, r4  @ItemID
	blh 0x0801631c   @{J} CanUnitUseAsWeapon
	@blh 0x08016574   @{U} CanUnitUseAsWeapon
	cmp r0,#0x0
	bne IsEnableItems_True

	mov r0, r5  @UnitID
	mov r1, r4  @ItemID
	blh 0x0801654c   @{J} CanUnitUseAsStaff
	@blh 0x080167A4   @{U} CanUnitUseAsStaff
	cmp r0,#0x0
	bne IsEnableItems_True

	b IsEnableItems_Flase

IsEnableItems_True:
	mov r0, #0x01
	b IsEnableItems_Exit

IsEnableItems_Flase:
	mov r0, #0x00

IsEnableItems_Exit:
	pop {r4,r5}
	pop {r1}
	bx r1

@r0  ItemID
CheckSPItems:
	push {lr}
	ldr r3,Data+4
	sub r3,#0x2

CheckSPItems_Loop:
	add r3,#0x2
	ldrb r1,[r3]
	cmp r1,#0x00
	beq CheckSPItems_False

	ldrb r1,[r3]
	cmp  r0,r1
	bne  CheckSPItems_Loop

CheckSPItems_Found:
	ldrb r0,[r3,#0x1]
	b    CheckSPItems_Exit

CheckSPItems_False:
		mov r0, #0x00

CheckSPItems_Exit:
	pop {r1}
	bx r1


.ltorg
Data:
