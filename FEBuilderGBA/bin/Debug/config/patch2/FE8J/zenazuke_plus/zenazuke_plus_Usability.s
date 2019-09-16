@全預け+
@GiveAll+
@
@全預け+が利用可能か判定する
@

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

	push	{lr}

	blh 0x8023F21 @SupplyUsability	{J}
@	blh 0x8023F65 @SupplyUsability	{U}
	cmp r0,#0x01
	bne Exit_False

for_aid:
	ldr	r1, =0x03004DF0	@操作キャラのワークメモリへのポインタ	{J}
@	ldr	r1, =0x03004E50	@操作キャラのワークメモリへのポインタ	{U}
	ldr	r0, [r1, #0x0]	@UnitRAM構造体

	bl  LoadUnitByAID
	cmp r0,#0x00
	beq for_myself

	bl  HasItem
	cmp r0 , #0x01
	beq Exit_True

for_myself:
	ldr	r1, =0x03004DF0	@操作キャラのワークメモリへのポインタ	{J}
@	ldr	r1, =0x03004E50	@操作キャラのワークメモリへのポインタ	{U}
	ldr	r0, [r1, #0x0]	@UnitRAM構造体

	bl  HasItem

	cmp r0 , #0x01
	beq Exit_True

Exit_False:
	mov r0,#0x3
	b   Exit
Exit_True:
	mov	r0,#0x1

Exit:
	pop {r1}
	bx  r1


HasItem:
	push	{r4,r5,r6,r7, lr}
	mov		r4,r0
	
	mov     r7, #0x0
	ldrb	r1,[r4, #0xB]		@所属
	cmp		r1,#0x40
	bge     HasItem_Start
	mov		r7, #0x01		@PlayerUnit

HasItem_Start:
	mov		r1,#0x1E	@アイテム1 アイテムIDへ
	add     r4,r1
	
	mov		r5,r4

	mov		r6,r4		@個数5つまでループする
	add		r6,#0xA		@5個*2バイト = 0xA

item_loop:
	cmp		r5,r6
	bge		HasItem_exit_false

	ldrb	r0,[r5,#0x00]	@アイテムID
	add		r5,#0x2

	cmp		r0,#0x00     @アイテム終端
	beq		HasItem_exit_false

	is_target_item:

		cmp r7,#0x00               @自軍でないなら全アイテムが対象
		beq HasItem_exit_true

		@アイテムテーブル
		blh 0x08017558		@GetROMItemStructPtr	{J}
@		blh 0x080177b0		@GetROMItemStructPtr	{U}

		@アイテムの属性の取得
		ldrb r1,[r0,#0x8]

		mov  r2, #0x01      @武器ですか？
		and  r2, r1
		cmp  r2, #0x00
		bne  item_loop

		mov  r2, #0x04      @杖ですか？
		and  r2, r1
		cmp  r2, #0x00
		bne  item_loop
		b    HasItem_exit_true

HasItem_exit_true:
	mov	r0, #0x01
	b	HasItem_exit

HasItem_exit_false:
	mov	r0, #0x00

HasItem_exit:
	pop	{r4,r5,r6,r7}
	pop {r1}
	bx r1


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
