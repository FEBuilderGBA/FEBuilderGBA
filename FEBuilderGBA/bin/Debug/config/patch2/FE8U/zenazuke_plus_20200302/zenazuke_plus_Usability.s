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

@	blh 0x8023F21 @SupplyUsability	{J}
	blh 0x8023F65 @SupplyUsability	{U}
	cmp r0,#0x01
	bne Exit_False

for_aid:
@	ldr	r1, =0x03004DF0	@操作キャラのワークメモリへのポインタ	{J}
	ldr	r1, =0x03004E50	@操作キャラのワークメモリへのポインタ	{U}
	ldr	r0, [r1, #0x0]	@UnitRAM構造体
	cmp r0, #0x00
	beq Exit_False

	@敵を捕らえている必要がある
	ldrb r0, [r0, #0x1b]
	mov r1, #0x80
	and r1, r0
	cmp r1, #0x0
	beq for_myself  @敵を捕らえていないので、自分のアイテムだけ確認する

@	blh 0x08019108   @{J}  GetUnitStruct RET=RAM Unit:@UNIT
	blh 0x08019430   @{U}  GetUnitStruct RET=RAM Unit:@UNIT
	cmp r0,#0x00
	beq for_myself

	bl  HasItem
	cmp r0 , #0x01
	beq Exit_True

for_myself:
@	ldr	r1, =0x03004DF0	@操作キャラのワークメモリへのポインタ	{J}
	ldr	r1, =0x03004E50	@操作キャラのワークメモリへのポインタ	{U}
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

		@有効な武器か杖か？
		bl IsEnableWeaponOfStaff
		cmp  r0, #0x01
		beq  item_loop

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

@有効な武器か杖か？
IsEnableWeaponOfStaff:
	push {r4,r5,lr}

	mov r4,r0 @ItemID

	@装備できる武器ですか？
@	ldr	r0, =0x03004DF0	@操作キャラのワークメモリへのポインタ	{J}
	ldr	r0, =0x03004E50	@操作キャラのワークメモリへのポインタ	{U}
	ldr	r5, [r0, #0x0]	@UnitRAM構造体

	mov r0, r5  @UnitID
	mov r1, r4  @ItemID
@	blh 0x0801631c   @{J} CanUnitUseAsWeapon
	blh 0x08016574   @{U} CanUnitUseAsWeapon
	cmp r0,#0x0
	bne IsEnableItems_True

	mov r0, r5  @UnitID
	mov r1, r4  @ItemID
@	blh 0x0801654c   @{J} CanUnitUseAsStaff
	blh 0x080167A4   @{U} CanUnitUseAsStaff
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
